using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    public class UserSurvey
    {
        public string surveyKey { get; set; }
        public string surveyNumber { get; set; }
        public string surveyType { get; set; }
        public string surveyProvider { get; set; }
        public string status { get; set; }
        public object endDate { get; set; }
        public object actionDate { get; set; }
        public object supervisorComments { get; set; }
        public object startDate { get; set; }
        public bool active { get; set; }
        public string endDateString { get; set; }
        public string actionDateString { get; set; }
        public string startDateString { get; set; }
    }
}
