using CMS_Survey.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using static CMS_Survey.Models.SectionHelp;

namespace CMS_Survey.Helpers
{
    internal class SurveyHelper
    {
        internal static SectionHelp.Rootobject Request;
        public List<SurverInsertObject> surveyInsertObjectList { get; set; }
        public static ObservableCollection<SectionHelp.Rootobject> SurveyList { get; set; }
        internal SurveyHelper(SectionHelp.Rootobject SectionHelpRoot)
        {
            Request = SectionHelpRoot;
           
        }
        internal SurveyHelper()
        {
            SurveyList = new ObservableCollection<Models.SectionHelp.Rootobject>();
            
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path,"Surveys");
            
           
            if (!Directory.Exists(FilePath))
                return;
            DirectoryInfo dInfo = new DirectoryInfo(FilePath);
            
            try
            {
                foreach (var item in dInfo.GetFiles("*.json"))
                {
                   var RootObj = new Rootobject();
                    List<Models.SectionHelp.Section> SectionList = null;
                    
                    using (StreamReader file = File.OpenText(item.FullName))
                    {
                        var json = file.ReadToEnd();
                        SectionList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.SectionHelp.Section>>(json);
                        RootObj.sections = SectionList.ToArray();
                        SurveyList.Add(RootObj);
                        // System.Diagnostics.Debug.WriteLine(result);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        private void ProcessSaveRequest()
        {
            User usr = new Models.User();
            SurveyTypeLu surveyTypeLu = new Models.SurveyTypeLu();
            surveyInsertObjectList = new List<Helpers.SurverInsertObject>();
            DateTime currentTime = DateTime.Now;
            foreach (Section section in Request.sections)
            {
                SurverInsertObject surveyInsertObject = new Helpers.SurverInsertObject();
                var survey = new Models.Survey(new Random().Next(int.MinValue, int.MaxValue), surveyTypeLu.SurveyTypeKey, currentTime, true, currentTime, usr.UserKey, currentTime, usr.UserKey);
                surveyInsertObject.Survey = survey;
                var surveyaccess = new UserSurveyAccess(new Random().Next(int.MinValue, int.MaxValue), usr.UserKey, survey.SurveyKey, "", currentTime, usr.UserKey, currentTime, usr.UserKey);
                surveyInsertObject.userSurveyAccess = surveyaccess;
                surveyInsertObject.Answers = new List<Models.Answer>();
                foreach (Surveyquestionanswerlist surveyQuestionAnswerList in section.surveyQuestionAnswerList.ToList())
                {
                    foreach (Answerslist answer in surveyQuestionAnswerList.answersList)
                    {
                        Answer ans = new Models.Answer(surveyaccess.UserSurveyKey, answer.answer.ToString(), answer.htmlControlId, currentTime, usr.UserKey, currentTime, usr.UserKey);
                        surveyInsertObject.Answers.Add(ans);
                    }
                }
                surveyInsertObjectList.Add(surveyInsertObject);
            }

        }

        internal List<JumpClass> GetJumpSections(SectionHelp.Section[] Sections)
        {

            List<JumpClass> JmpClassList = new List<Helpers.JumpClass>();
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
        internal void GetUserSurveys()
        {

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
