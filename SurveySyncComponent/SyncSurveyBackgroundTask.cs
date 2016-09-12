using CMS_Survey.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace SurveySyncComponent
{
    public sealed class SyncSurveyBackgroundTask : IBackgroundTask
    {
        private static SQLiteConnection conn;
        Assignment_table assignTable;
        BackgroundTaskDeferral _deferral;
        internal string userKey=null;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            conn = new SQLiteConnection("Surveydb.sqlite");
            try
            {
                assignTable = new Assignment_table(conn);
               Task t= SyncAllLocalSurveys();
                await t;
                Task t2 = AssignSurveys();
                await t2;

            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
            _deferral.Complete();
        }


        private IStorageFile textFile { get; set; }
        public bool isSaveSuccessful { get; private set; }
        public bool isAssignSuccessful { get; private set; }
        static string HostUrl = @"https://cms-specialtysurveys-internal.org/";
        private string SurveyUrl = HostUrl + @"SurveyRest/rest/myresource/questionanswers";
        private string AssignSurveyUrl = HostUrl + @"SurveyRest/rest/myresource/assignsurvey?userKey={0}&surveyKey={1}&userEmail={2}";
        private async Task SaveSurveyLocal(string jsonRequest, string SurveyKey, string SubFolder)
        {
            var usrfolder = ApplicationData.Current.LocalFolder;
            StorageFolder folder = await usrfolder.CreateFolderAsync(SubFolder,
                   CreationCollisionOption.OpenIfExists);
            var path = folder.Path;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string FilePath = Path.Combine(path, string.Format("{0}_{1}.json", SurveyKey,userKey));

            textFile = (IStorageFile)null;
            await WriteFile(jsonRequest, SurveyKey, folder, FilePath);

        }
        private async Task WriteFile(string jsonRequest, string SurveyKey, StorageFolder folder, string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                textFile = await folder.CreateFileAsync(string.Format("{0}_{1}.json", SurveyKey, userKey));
            }
            else
            {
                textFile = await folder.GetFileAsync(string.Format("{0}_{1}.json", SurveyKey, userKey));
            }
            await FileIO.WriteTextAsync(textFile, jsonRequest);
        }
        private async Task<bool> CallSurveyService(string jsonSectionList)
        {
            bool isSuccess = false;
            string jsonString = null;
            SectionHelp.Rootobject SecList = null;
            // var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(SectionList);

            try
            {
                var client = new HttpClient();
                isSaveSuccessful = false;
                HttpContent content = new StringContent(jsonSectionList, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(SurveyUrl, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    jsonString = await response.Content.ReadAsStringAsync();

                    SecList = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionHelp.Rootobject>(jsonString);
                    isSuccess = true;
                    isSaveSuccessful = true;
                    await SaveSurveyLocal(jsonSectionList, SecList.sections.First().surveyKey.ToString(), "Surveys");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSaveSuccessful;
        }

        private async Task SyncAllLocalSurveys()
        {

            DirectoryInfo Dinfo = await GetTempSurveyDirectory();
            foreach (FileInfo SurveyFile in Dinfo.GetFiles("*.json"))
            {
                string json = "";

                userKey = SurveyFile.Name.Replace(".json", "").Substring(SurveyFile.Name.IndexOf('_') + 1);
                using (StreamReader sr = SurveyFile.OpenText())
                {
                   
                    json = sr.ReadToEnd();
                }
                    if (await CallSurveyService(json))
                    {
                        await DeleteFiles(SurveyFile.Name);
                    }
              
            }
        }

        private async Task<DirectoryInfo> GetTempSurveyDirectory()
        {
            var usrfolder = ApplicationData.Current.LocalFolder;
            StorageFolder folder = await usrfolder.CreateFolderAsync("TempSurveys",
                   CreationCollisionOption.OpenIfExists);
            var path = folder.Path;
            if (!Directory.Exists(path))
                return null;
            DirectoryInfo Dinfo = new DirectoryInfo(path);
            return Dinfo;
        }

        private async Task DeleteFiles(string FileName)
        {
            DirectoryInfo dinfo = await GetTempSurveyDirectory();
            FileInfo fin = dinfo.GetFiles(FileName).FirstOrDefault();
            if (fin != null && fin.Exists)
                fin.Delete();
        }

        private async Task AssignSurveys()
        {
            List<Assignment> assignments = GetAllAssignments();
            foreach (Assignment assign in assignments)
            {
               bool assigned= await CallAssignSurvey(assign.EmailID, Convert.ToString(assign.Survey_Key),assign.User_Key);
                if(assigned)
                DeleteAssignment(assign.Assignment_ID);
            }
        }
        private List<Assignment> GetAllAssignments()
        {
            List<Assignment> AssignmentList = new List<Assignment>();
          
           AssignmentList= assignTable.GetAssignments();
            return AssignmentList;
        }
        private void DeleteAssignment(long id)
        {
            assignTable.deleteAssignment(id);
        }
        private async Task<bool> CallAssignSurvey(string EmailId, string SurveyKey,long userKey)
        {
            bool isSuccess = false;
            string jsonString = null;
            SectionHelp.Rootobject SecList = null;
            //var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(SectionList);


            try
            {
                var client = new HttpClient();
                isAssignSuccessful = false;
                //var values = new Dictionary<string, string>();

                var content = new StringContent("", Encoding.UTF8, "application/json");

                //HttpContent content = new StringContent(UserEmail, Encoding.UTF8, "application/json");
                string mailid = EmailId.Substring(EmailId.IndexOf('(') + 1).Replace(")", "");
                HttpResponseMessage response = await client.PutAsync(string.Format(AssignSurveyUrl, userKey, SurveyKey, mailid), content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    jsonString = await response.Content.ReadAsStringAsync();

                    //SecList = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionHelp.Rootobject>(jsonString);
                   
                    isAssignSuccessful = true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAssignSuccessful;
        }
    }
}
