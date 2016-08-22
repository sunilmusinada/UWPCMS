using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    public class SurveyGridObject
    {
        public int SurveyId  
        {  
            get;  
            set;  
        }
        public string SurveyType
        {  
            get;  
            set;  
        }  
        public string Provider
        {  
            get;  
            set;  
        }  
        public string Status
        {  
            get;  
            set;  
        }

        public DateTime SurveyEndDate
        {
            get;
            set;
        }

        public DateTime SurveyActionDate
        {
            get;
            set;
        }
    }
    public class SurveyManager
        {  
        public static List<SurveyGridObject> GetSurveys()
        {  
            var surveys = new List<SurveyGridObject>();
            surveys.Add(new SurveyGridObject
            {
                SurveyId = 1,
                SurveyType = "Infection control",
                Provider = "Provider 1",
                Status = "In progress",
                SurveyEndDate = DateTime.Today,
                SurveyActionDate = DateTime.UtcNow
            });
            surveys.Add(new SurveyGridObject
            {
                SurveyId = 2,
                SurveyType = "Infection control",
                Provider = "Provider 2",
                Status = "Revision required",
                SurveyEndDate = DateTime.Today,
                SurveyActionDate = DateTime.UtcNow
            });
            surveys.Add(new SurveyGridObject
            {
                SurveyId = 3,
                SurveyType = "Infection control",
                Provider = "Provider 3",
                Status = "Revision required",
                SurveyEndDate = DateTime.Today,
                SurveyActionDate = DateTime.UtcNow
            });
            surveys.Add(new SurveyGridObject
            {
                SurveyId = 4,
                SurveyType = "Infection control",
                Provider = "Provider 4",
                Status = "In progress",
                SurveyEndDate = DateTime.Today,
                SurveyActionDate = DateTime.UtcNow
            });
            surveys.Add(new SurveyGridObject
            {
                SurveyId = 5,
                SurveyType = "Infection control",
                Provider = "Provider 5",
                Status = "Revision required",
                SurveyEndDate = DateTime.Today,
                SurveyActionDate = DateTime.UtcNow
            });  
            return surveys;  
        }  
    }
}
