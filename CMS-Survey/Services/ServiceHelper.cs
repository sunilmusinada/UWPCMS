using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using System.IO;
using System.Net;
using CMS_Survey.Models;
using System.Collections.ObjectModel;
using CMS_Survey.Helpers;
namespace CMS_Survey.Services
{
    internal class ServiceHelper
    {
        private static ServiceHelper _serviceHelper;
        //public bool isOffline = false;
        public static bool _OfflineMode = false;
        private bool _isOffline;

        public bool isOffline
        {
            get {

                CheckHeartbeat();
               //CallLoginService(currentUser.UserId,currentUser.Password);
                return _isOffline;
            }
            set { _isOffline = value; }
        }
        public async Task<bool> IsOffline()
        {
            bool isOfflineUser = false;
            isOfflineUser = await CheckHeartbeat();
            return isOfflineUser;
          
        }
        public bool isSaveSuccessful = false;
        internal ObservableCollection<UserSurvey> UserSurveyList;
        public List<Models.State> StateCode
        {
            get { return _StateCode; }
            set { _StateCode = value; }
        }
        internal List<Hospital> Hospitals;

        internal User currentUser;
        private IStorageFile textFile { get; set; }
        public static  string HostUrl = @"https://cms-specialtysurveys-internal.org/";
        public static string CitationUrl = HostUrl + "Survey-web";
        #region UrlStrings
        private string LoginServiceUrl = HostUrl+ @"SurveyRest/rest/myresource/authentication?userName={0}&password={1}";

        private string HospitalServiceUrl = HostUrl + @"SurveyRest/rest/myresource/providers?state={0}";

        private string StateServiceUrl = HostUrl + @"SurveyRest/rest/myresource/states";

        private string SurveyUrl = HostUrl + @"SurveyRest/rest/myresource/questionanswers";

        private string UserSurveyUrl = HostUrl + @"SurveyRest/rest/myresource/usersurveys?userKey={0}";

        private string ServeyDataUrl = HostUrl + @"SurveyRest/rest/myresource/questionanswers?userKey={0}&surveyKey={1}";

        private string DeleteSurveyUrl = HostUrl + @"SurveyRest/rest/myresource/questionanswers?surveyKey={0}";

        private string HeartbeatUrl = HostUrl + @"SurveyRest/rest/myresource/status";

        private string  HospitalJsonUrl = HostUrl + @"SurveyRest/rest/myresource/questions";

        private string UsersUrl = HostUrl+@"SurveyRest/rest/myresource/users?state={0}";
        #endregion
        public static ServiceHelper ServiceHelperObject
        {
            get
            {
                if (_serviceHelper == null)
                    _serviceHelper = new ServiceHelper();
                return _serviceHelper;
            }
        }
       
        #region State Service

        private List<Models.State> _StateCode;
        internal List<Models.State> GetStates()
        {
            var folder = ApplicationData.Current.LocalFolder;
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "States.json");
            if (!File.Exists(FilePath))
            {
                CallStateService();
            }
            if (StateCode == null||StateCode.Count==0)
            {
                StateCode = new List<Models.State>();
                CreateStateDictionary();
                return StateCode;
            }
            else
            {
                return StateCode;
            }

        }

        private async void CallStateService()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(StateServiceUrl));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var folder = ApplicationData.Current.LocalFolder;
                string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "States.json");
                textFile = (IStorageFile)null;
                if (!File.Exists(FilePath))
                {
                    textFile = await folder.CreateFileAsync("States.json");
                }
                else
                {
                    textFile = await folder.GetFileAsync("States.json");
                }
                await FileIO.WriteTextAsync(textFile, jsonString);
            }
        }

        private void CreateStateDictionary()
        {
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "States.json");
            try
            {
                using (StreamReader file = File.OpenText(FilePath))
                {
                    var json = file.ReadToEnd();
                    StateCode = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.State>>(json);
                    // System.Diagnostics.Debug.WriteLine(result);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string GetCodeforState(string StateName)
        {
            var Code = StateCode.Where(e => e.stateName.ToUpper().Equals(StateName.ToUpper())).Select(e => e.stateCode).FirstOrDefault();
            return Code;
        }
        #endregion

        #region Hospital Service
        internal async Task<List<Hospital>> GetHospitalsForState(string StateCode)
        {
            //await CallHospitalService(StateCode);
            if(await this.IsOffline())
            {
                return GetHospitalForStateOffline(StateCode);
            }
            var client = new HttpClient();
            Hospitals = new List<Hospital>();
            HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(HospitalServiceUrl, StateCode)));
           
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                var folder = ApplicationData.Current.LocalFolder;
               // SaveHospitalsLocal(jsonString);

                Hospitals = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Hospital>>(jsonString);
                //    await FileIO.WriteTextAsync(textFile, jsonString);
            }
            return Hospitals;
            //return Hospitals;
        }

        private void SaveHospitalsLocal(string jsonString)
        {
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Hospitals.json");
            textFile = (IStorageFile)null;
            using (var fileStream = new FileStream(FilePath, FileMode.Create))
            {
                byte[] byteData = null;
                byteData = Encoding.ASCII.GetBytes(jsonString);
                fileStream.Write(byteData, 0, byteData.Length);
            }
        }

        internal  List<Hospital> GetHospitalForStateOffline(string StateCode)
        {
            Hospitals = new List<Hospital>();

            //string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Hospitals.json");
            try
            {
                ProvidersLu proVlu = new Models.ProvidersLu();
               Hospitals= proVlu.GetHospitalsForState(StateCode);
            }
            catch (Exception ex)
            {

            }
            return Hospitals;
        }
        internal Hospital GetHospitalForProviderKey(int HospitalKey)
        {
            Hospital hsp = new Models.Hospital();
            ProvidersLu plu = new Models.ProvidersLu();
            hsp=plu.GetHospitalForProviderKey(HospitalKey);
            return hsp;
        }
        #endregion

        #region Survey
        internal async Task<SectionHelp.Rootobject> CallSurveyService(List<Models.SectionHelp.Section> SectionList)
        {
            bool isSuccess = false;
            string jsonString = null;
            SectionHelp.Rootobject SecList = null;
            var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(SectionList);
           
            if (isOffline)
            {
                await SaveSurveyLocal(jsonRequest,SectionList.First().surveyKey.ToString());
                SecList = new SectionHelp.Rootobject();
                SecList.sections = new List<SectionHelp.Section>().ToArray();
                SecList.sections = SectionList.ToArray();

                return SecList;
            }
            try
            {
                var client = new HttpClient();
                isSaveSuccessful = false;
                HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(SurveyUrl, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    jsonString = await response.Content.ReadAsStringAsync();

                    SecList = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionHelp.Rootobject>(jsonString);
                    isSuccess = true;
                    isSaveSuccessful = true;
                    await SaveSurveyLocal(jsonRequest, SectionList.First().surveyKey.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SecList;
        }

        public async Task SaveSurveyLocal(string jsonRequest,string SurveyKey)
        {
            var usrfolder = ApplicationData.Current.LocalFolder;
           StorageFolder folder= await usrfolder.CreateFolderAsync("Surveys",
                  CreationCollisionOption.OpenIfExists);
            var path = folder.Path;
            
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string FilePath = Path.Combine(path, string.Format("{0}.json",SurveyKey));
            textFile = (IStorageFile)null;
            if (!File.Exists(FilePath))
            {
                textFile = await folder.CreateFileAsync(string.Format("{0}.json", SurveyKey));
            }
            else
            {
                textFile = await folder.GetFileAsync(string.Format("{0}.json", SurveyKey));
            }
            await FileIO.WriteTextAsync(textFile, jsonRequest);
            if (Hospitals != null && Hospitals.Count > 0)
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(Hospitals);
                SaveHospitalsLocal(jsonString);
            }
        }

        internal async Task<SectionHelp.Rootobject> CallGetSurveyService(string CurrentuserKey, string surveykey)
        {
            string jsonString = null;
            SectionHelp.Rootobject SectionList = null;
            var client = new HttpClient();
            try
            {
               
                    HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(ServeyDataUrl, CurrentuserKey, surveykey)));
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        jsonString = await response.Content.ReadAsStringAsync();

                        SectionList = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionHelp.Rootobject>(jsonString);

                    }
               
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return SectionList;

        }
        #endregion

        #region Authentication
        internal async Task<bool> CallLoginService(string UserName,string Password)
        {
           _isOffline = false;
            //_OfflineMode = false;
            var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(LoginServiceUrl, UserName, Password)));
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string jsonString = await response.Content.ReadAsStringAsync();

                    currentUser = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(jsonString);
                    //currentUser.userName = UserName;
                    currentUser.Password = Password;

                    currentUser.InsertUser();
                   
                }
                else if (response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                   _isOffline = true;
                    //_OfflineMode = true;
                    OfflineLogin(UserName,Password);
                    //return _isOffline;
                }
            }
            catch(System.Net.Http.HttpRequestException ex)
            {
                _isOffline = true;
                OfflineLogin(UserName, Password);
            }
            return _isOffline;
        }

        private void OfflineLogin(string UserName,string password)
        {
           // _isOffline = true;
           // _OfflineMode = true;
            User usr = new Models.User();
            usr.GetUser(UserName,password);
            if(usr!=null)
            currentUser = usr.CurrentUser;
        }
        #endregion

        #region User Survey
        internal async Task CallUserSurveyService()
        {
            try
            {
                var client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(UserSurveyUrl, this.currentUser.userKey)));
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string jsonString = await response.Content.ReadAsStringAsync();

                    UserSurveyList = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<UserSurvey>>(jsonString);

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        internal void CallUserSurveyServiceOffline()
        {
            SurveyHelper svHelper = new SurveyHelper();
            
            UserSurveyList = new ObservableCollection<UserSurvey>();
            foreach (var SurveyObj in SurveyHelper.SurveyList)
            {
                UserSurvey usrSurvey = new UserSurvey();
                var surObj = SurveyObj.sections.FirstOrDefault();
                if (surObj == null)
                    return;
                var stateList = surObj.surveyQuestionAnswerList.Where(e => e.questionId.Equals(100)).Select(e => e.answersList).FirstOrDefault();
                var Statecode = stateList.Where(e => e.htmlControlId.Equals(100)).Select(e => e.answer).FirstOrDefault();
                var ansList = surObj.surveyQuestionAnswerList.Where(e => e.questionId.Equals(200)).Select(e => e.answersList).FirstOrDefault();
                int provider = Convert.ToInt32(ansList.Where(e => e.htmlControlId.Equals(200)).Select(e => e.answer).FirstOrDefault());
                var hsp= GetHospitalForProviderKey(provider);
                usrSurvey.surveyProvider = hsp.facilityName;
               
                usrSurvey.surveyKey = Convert.ToString(surObj.surveyKey);
                usrSurvey.surveyType = "Infection control";//Convert.ToString(surObj.surveyName);
                usrSurvey.surveyNumber = "";
                // var _hospital = Hospitals.Where(e => e.providerKey.Equals(provider)).Select(e => e.facilityName).FirstOrDefault();
                usrSurvey.surveyProvider = Convert.ToString(usrSurvey.surveyProvider);
                usrSurvey.status = "In Progress";//Convert.ToString(surObj.status);
                usrSurvey.endDateString = "";
                UserSurveyList.Add(usrSurvey);
            }
            

        }

        internal string GetOfflineSelectedState(string stateCode)
        {
            //if (SurveyHelper.Request == null)
            //    CallUserSurveyServiceOffline();
            //var surObj = SurveyHelper.Request.sections.FirstOrDefault();
            //if (surObj == null)
            //    return null;
            //var ansList = surObj.surveyQuestionAnswerList.Where(e => e.questionId.Equals(100)).Select(e => e.answersList).FirstOrDefault();
            //string stateCode = Convert.ToString(ansList.Where(e => e.htmlControlId.Equals(100)).Select(e => e.answer).FirstOrDefault());
            var state = StateCode.Where(e => e.stateCode.ToUpper().Equals(stateCode.ToUpper())).Select(e => e.stateName).FirstOrDefault();
           
            return state;

        }
        #endregion

        #region DeleteSurvey
        internal async Task<bool> DeleteSurvey(string SurveyKey)
        {
            var Deleted = false;
            var client = new HttpClient();
            try
            {
               
                HttpResponseMessage response = await client.DeleteAsync(new Uri(string.Format(DeleteSurveyUrl, SurveyKey)));
                if (response.StatusCode == HttpStatusCode.OK)
                {

                    string jsonString = await response.Content.ReadAsStringAsync();
                    Deleted = true;

                }

            }
            catch (System.Net.Http.HttpRequestException ex)
            {

            }
            return Deleted;
        }

        #endregion

        #region Questionreading
        public async void getJsonFile()
        {
            if (await IsOffline())
                return;
            var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(HospitalJsonUrl));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var folder = ApplicationData.Current.LocalFolder;
                    string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Answers_Hospital_Infection_Control_Worksheet.json");
                    var textFile = (IStorageFile)null;
                    if (!File.Exists(FilePath))
                    {
                        textFile = await folder.CreateFileAsync("Answers_Hospital_Infection_Control_Worksheet.json");
                    }
                    else
                    {
                        textFile = await folder.GetFileAsync("Answers_Hospital_Infection_Control_Worksheet.json");
                    }
                    await FileIO.WriteTextAsync(textFile, jsonString);
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {

            }
        }

        #endregion

        #region HearbeatRegion

        internal async Task<bool> CheckHeartbeat()
        {
            _isOffline = false;
            _OfflineMode = false;
            var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(HeartbeatUrl)));
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _isOffline = false;
                    _OfflineMode = false;
                   
                }
                else if (response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    _isOffline = true;
                    _OfflineMode = true;
                   
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                _isOffline = true;
              
            }
            return _isOffline;
        }


        #endregion

        #region Users
        internal async Task<List<Models.User>> GetUsersForState(string state)
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new Uri(string.Format(UsersUrl,state)));
            List<Models.User> Users=null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(jsonString);
              
            }
            return Users;
        }
        //internal async Task UpdateAllUsers()
        //{
        //    List<User> users =await GetUsersForState("ALL");
        //    Database.users_table usrTable = new Database.users_table();
        //    usrTable.deleteAll();
        //    await usrTable.BulkInsertUsers(users);

        //}
      
        #endregion

    }
}
