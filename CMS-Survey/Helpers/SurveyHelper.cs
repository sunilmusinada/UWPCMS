﻿using CMS_Survey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using static CMS_Survey.Models.SectionHelp;

namespace CMS_Survey.Template
{
    internal class SurveyHelper
    {
        private static SurveyHelper _suveyHelper;
        internal static SectionHelp.Rootobject Request;
        public List<SurverInsertObject> surveyInsertObjectList { get; set; }
        public static ObservableCollection<SectionHelp.Rootobject> SurveyList { get; set; }
        public event EventHandler FinishedDownloading;
        public static List<Models.UserSurvey> SurveyJsonList { get; set; }

        public  bool DownloadFinished = false;
        internal SurveyHelper(SectionHelp.Rootobject SectionHelpRoot)
        {
            Request = SectionHelpRoot;
           
        }
        private SurveyHelper()
        {
            CreateSurveyList();
        }
        public static SurveyHelper SurveyHelperObject
        {
            get
            {
                if (_suveyHelper == null)
                    _suveyHelper = new SurveyHelper();
                return _suveyHelper;
            }
        }
        public void CreateSurveyList()
        {
            SurveyList = new ObservableCollection<Models.SectionHelp.Rootobject>();

            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, Constants.SurveyFolder,Services.ServiceHelper.ServiceHelperObject.currentUser.userKey.ToString());


            if (!Directory.Exists(FilePath))
                return;
            DirectoryInfo dInfo = new DirectoryInfo(FilePath);

            try
            {
                //foreach (var item in dInfo.GetFiles("*.json"))
                //{
                //    var RootObj = new Rootobject();
                //    List<Models.SectionHelp.Section> SectionList = null;

                    using (StreamReader file = File.OpenText(FilePath+ "\\SurveyList.json" ))
                    {
                        var json = file.ReadToEnd();
                        //SurveyJsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.SectionHelp.Section>>(json);
                        //RootObj.sections = SectionList.ToArray();
                        //SurveyList.Add(RootObj);
                    SurveyJsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserSurvey>>(json);
                    //RootObj.sections = SectionList.ToArray();
                    //SurveyList.Add(RootObj);
                    // System.Diagnostics.Debug.WriteLine(result);
                }
               // }
            }
            catch (Exception ex)
            {

            }

        }
        internal SurveyHelper (bool IsOnline)
        {

        }
        private void ProcessSaveRequest()
        {
            User usr = new Models.User();
            SurveyTypeLu surveyTypeLu = new Models.SurveyTypeLu();
            surveyInsertObjectList = new List<Template.SurverInsertObject>();
            DateTime currentTime = DateTime.Now;
            foreach (Section section in Request.sections)
            {
                SurverInsertObject surveyInsertObject = new Template.SurverInsertObject();
                var survey = new Models.Survey(new Random().Next(int.MinValue, int.MaxValue), surveyTypeLu.SurveyTypeKey, currentTime, true, currentTime, usr.userKey, currentTime, usr.userKey);
                surveyInsertObject.Survey = survey;
                var surveyaccess = new UserSurveyAccess(new Random().Next(int.MinValue, int.MaxValue), usr.userKey, survey.SurveyKey, "", currentTime, usr.userKey, currentTime, usr.userKey);
                surveyInsertObject.userSurveyAccess = surveyaccess;
                surveyInsertObject.Answers = new List<Models.Answer>();
                foreach (Surveyquestionanswerlist surveyQuestionAnswerList in section.surveyQuestionAnswerList.ToList())
                {
                    foreach (Answerslist answer in surveyQuestionAnswerList.answersList)
                    {
                        Answer ans = new Models.Answer(surveyaccess.UserSurveyKey, answer.answer.ToString(), answer.htmlControlId, currentTime, usr.userKey, currentTime, usr.userKey);
                        surveyInsertObject.Answers.Add(ans);
                    }
                }
                surveyInsertObjectList.Add(surveyInsertObject);
            }

        }

        internal List<JumpClass> GetJumpSections(SectionHelp.Section[] Sections)
        {

            List<JumpClass> JmpClassList = new List<Template.JumpClass>();
            for (int i = 0; i < Sections.Count(); i++)
            {
                var secArray = Sections[i].sectionTitle.Split('.');
                var jmpCls = new JumpClass();
                jmpCls.PageIndex = i;
                if (secArray.Count() > 1)
                {
                    jmpCls.SubSection = secArray[0];
                    jmpCls.SectionTitle = Sections[i].sectionTitle;

                }
                else
                {
                    jmpCls.SubSection = secArray[0];
                }
                JmpClassList.Add(jmpCls);
            }
            return JmpClassList;
        }
        internal void GetHelpSections(List<SectionHelp.Help> HelpSections)
        {


        }
        internal async Task GetUserSurveys()
        {
            try
            {
                var svcHelper = Services.ServiceHelper.ServiceHelperObject;
                if (await svcHelper.IsOffline())
                {
                    svcHelper.CallUserSurveyServiceOffline();
                }
                else
                {
                  string jsonString=   await svcHelper.CallUserSurveyService();
                  await Services.ServiceHelper.ServiceHelperObject.SaveSurveyList(jsonString, Constants.SurveyFolder);
                }
                var surveyList = svcHelper.UserSurveyList;
                
                foreach (UserSurvey usrSurvey in surveyList)
                {
                    
                    string jsonString = await GetClickedSurvey(usrSurvey.surveyKey);
                    await WriteFilesToDirectory(jsonString, usrSurvey.surveyKey);
                 
                }
                CreateSurveyList();
                MessageDialog msgDialog = new MessageDialog("Offline Sync Complete", "Information");
                await msgDialog.ShowAsync();
                DownloadFinished = true;
                OnFinishedDownload(null);
            }
            catch(Exception ex)
            {

            }
        }
        protected virtual void OnFinishedDownload(EventArgs e)
        {
            EventHandler handler = FinishedDownloading;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private async Task<string> GetClickedSurvey(string SurveyKey)
        {
            var currentUserKey = Convert.ToString(Services.ServiceHelper.ServiceHelperObject.currentUser.userKey);
            var jsonString= await Services.ServiceHelper.ServiceHelperObject.CallGetSurveyServiceJson(currentUserKey, SurveyKey);

            return jsonString;
        }

        private async Task WriteFilesToDirectory(string JsonRequest,string SurveyKey)
        {
            if (string.IsNullOrEmpty(JsonRequest))
                return;
            var usrfolder = ApplicationData.Current.LocalFolder;
            string subFolder = "Surveys";
            subFolder= subFolder + "\\" + Services.ServiceHelper.ServiceHelperObject.currentUser.userKey.ToString();
            StorageFolder folder = await usrfolder.CreateFolderAsync(subFolder,
                   CreationCollisionOption.OpenIfExists);
            var path = folder.Path;
          
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
          var userKey= Services.ServiceHelper.ServiceHelperObject.currentUser.userKey;
            string FilePath = Path.Combine(path, string.Format("{0}.json", SurveyKey));
            var sectionHelp = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionHelp.Rootobject>(JsonRequest);
            JsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(sectionHelp.sections.ToList());
            await Services.ServiceHelper.ServiceHelperObject.WriteFile(JsonRequest, SurveyKey,userKey.ToString(), folder, FilePath);
        }

        public static SectionHelp.Rootobject GetSurveyFromLocal(string SurveyKey)
        {
            var RootObj = new Rootobject();
            List<Models.SectionHelp.Section> SectionList = null;
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, Constants.SurveyFolder, Services.ServiceHelper.ServiceHelperObject.currentUser.userKey.ToString());
            if (!Directory.Exists(FilePath))
                return RootObj;
            try
            {
                using (StreamReader file = File.OpenText(FilePath + string.Format("\\{0}.json",SurveyKey)))
                {
                    var json = file.ReadToEnd();
                    SectionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.SectionHelp.Section>>(json);
                    RootObj.sections = SectionList.ToArray();

                }

            }
            catch(Exception ex)
            {

                //MessageDialog
            }
            return RootObj;
        }
    }

    internal class SurverInsertObject
    {
        public UserSurveyAccess userSurveyAccess { get; set; }
        public Survey Survey { get; set; }

        public List<Answer> Answers { get; set; }
    }
    internal class JumpClass
    {
        public int PageIndex { get; set; }
        public string SubSection { get; set; }

        public string SectionTitle { get; set; }
    }

}
