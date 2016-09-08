using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using CMS_Survey.Models;
namespace SurveySyncComponent
{
    public sealed class SyncSurveyHelper: IDisposable
    {
        private IStorageFile textFile { get; set; }
        public bool isSaveSuccessful { get; private set; }
        static string HostUrl = @"https://cms-specialtysurveys-internal.org/";
        private string SurveyUrl = HostUrl + @"SurveyRest/rest/myresource/questionanswers";
        public SyncSurveyHelper()
        {
            SyncAllLocalSurveys();
        }
        private async Task SaveSurveyLocal(string jsonRequest, string SurveyKey, string SubFolder)
        {
            var usrfolder = ApplicationData.Current.LocalFolder;
            StorageFolder folder = await usrfolder.CreateFolderAsync(SubFolder,
                   CreationCollisionOption.OpenIfExists);
            var path = folder.Path;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string FilePath = Path.Combine(path, string.Format("{0}.json", SurveyKey));

            textFile = (IStorageFile)null;
            await WriteFile(jsonRequest, SurveyKey, folder, FilePath);

        }
        private async Task WriteFile(string jsonRequest, string SurveyKey, StorageFolder folder, string FilePath)
        {
            if (!File.Exists(FilePath))
            {
                textFile = await folder.CreateFileAsync(string.Format("{0}.json", SurveyKey));
            }
            else
            {
                textFile = await folder.GetFileAsync(string.Format("{0}.json", SurveyKey));
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
                using (StreamReader sr = SurveyFile.OpenText())
                {
                    string json = "";
                    json = sr.ReadToEnd();
                    if(await CallSurveyService(json))
                    {
                        await DeleteFiles(SurveyFile.Name);
                    }
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
            DirectoryInfo dinfo =await  GetTempSurveyDirectory();
            FileInfo fin = dinfo.GetFiles(FileName).FirstOrDefault();
            if (fin != null && fin.Exists)
                fin.Delete();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
           
        }
    }
}
