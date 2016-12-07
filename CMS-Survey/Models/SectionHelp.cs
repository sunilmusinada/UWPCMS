using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;




namespace CMS_Survey.Models

{

    public class SectionHelp

    {




        public class Rootobject

        {

            public Section[] sections { get; set; }

            public List<Help> help { get; set; }

            public bool renderComments { get; set; }

        }




        public class Section

        {

            public string version { get; set; }

            public object userSurveyAccessKey { get; set; }

            public object status { get; set; }

            public string instructions { get; set; }

            public string surveyName { get; set; }

            public string sectionTitle { get; set; }

            public Surveyquestionanswerlist[] surveyQuestionAnswerList { get; set; }

            public object[] surveyQuestionAnswerListWithErrors { get; set; }

            public object userKey { get; set; }

            public object surveyKey { get; set; }

        }




        public class Surveyquestionanswerlist

        {

            public int questionId { get; set; }

            public string questionText { get; set; }

            public bool renderAddObservation { get; set; }

            public bool renderQuestion { get; set; }

            public bool disableAddObservation { get; set; }

            public string citableTagName { get; set; }

            public string citableTagURL { get; set; }

            public object validation { get; set; }

            public Answerslist[] answersList { get; set; }

            public bool hasErrors { get; set; }

            public object[] fileNames { get; set; }

            public int obsevationNumber { get; set; }

          //  public List<DifferentUserAnswerList> differentUserAnswerList { get; set; }

        }



        public class DifferentUserAnswerList
        {
            public int user { get; set; }
            public string answer { get; set; }

            public string answerDate { get; set; }
        }

        public class Answerslist

        {

            public string htmlControlType { get; set; }

            public string htmlControlText { get; set; }

            public object answer { get; set; }

            public object[] answersList { get; set; }

            public object answerDate { get; set; }

            public bool defaultVisible { get; set; }

            public bool defaultDisalbled { get; set; }

            public Htmloption[] htmlOptions { get; set; }

            public int htmlControlId { get; set; }

            public int parentId { get; set; }

            public Childidlist[] childIdList { get; set; }

            public bool required { get; set; }

            public object fieldType { get; set; }

            public object fieldLength { get; set; }

            public string placeHolderText { get; set; }

            public bool renderRemoveButton { get; set; }

            public int observationNumber { get; set; }

            public object message { get; set; }

            public object mesageType { get; set; }

            public object errorStyle { get; set; }

            public string observatgionNumberString { get; set; }

            public string dateString { get; set; }

            public List<DifferentUserAnswerList> differentUserAnswerList { get; set; }

        }




        public class Htmloption

        {

            public string value { get; set; }

            public string key { get; set; }

        }




        public class Childidlist

        {

            public string htmlControlType { get; set; }

            public object htmlControlText { get; set; }

            public object answer { get; set; }

            public object[] answersList { get; set; }

            public object answerDate { get; set; }

            public bool defaultVisible { get; set; }

            public bool defaultDisalbled { get; set; }

            public object[] htmlOptions { get; set; }

            public int htmlControlId { get; set; }

            public int parentId { get; set; }

            public Childidlist1[] childIdList { get; set; }

            public bool required { get; set; }

            public object fieldType { get; set; }

            public object fieldLength { get; set; }

            public string placeHolderText { get; set; }

            public bool renderRemoveButton { get; set; }

            public int observationNumber { get; set; }

            public object message { get; set; }

            public object mesageType { get; set; }

            public object errorStyle { get; set; }

            public string observatgionNumberString { get; set; }

            public string dateString { get; set; }

        }




        public class Childidlist1

        {

            public string htmlControlType { get; set; }

            public object htmlControlText { get; set; }

            public object answer { get; set; }

            public object[] answersList { get; set; }

            public object answerDate { get; set; }

            public bool defaultVisible { get; set; }

            public bool defaultDisalbled { get; set; }

            public object[] htmlOptions { get; set; }

            public int htmlControlId { get; set; }

            public int parentId { get; set; }

            public object[] childIdList { get; set; }

            public bool required { get; set; }

            public object fieldType { get; set; }

            public object fieldLength { get; set; }

            public string placeHolderText { get; set; }

            public bool renderRemoveButton { get; set; }

            public int observationNumber { get; set; }

            public object message { get; set; }

            public object mesageType { get; set; }

            public object errorStyle { get; set; }

            public string observatgionNumberString { get; set; }

            public string dateString { get; set; }

        }
        public class HelpSectionLink
        {
            public string helpLinkName { get; set; }
            public string helpLinkURL { get; set; }
        }

        public class Help
        {
            public string helpSectionName { get; set; }
            public List<HelpSectionLink> helpSectionLink { get; set; }
        }
    }

}