using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    public class SurveyerUserModel
    {
        public long userKey { get; set; }
        public bool delete { get; set; }
    }

    public class Surveyors
    {
        public long surveyKey { get; set; }
        public List<SurveyerUserModel> surveyerUserModels { get; set; }
    }

}
