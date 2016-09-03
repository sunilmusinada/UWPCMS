using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_Survey.Models
{
    internal class ObservationHelper
    {
        public SectionHelp.Surveyquestionanswerlist Question { get; set; }
        public string ButtonName { get; set; }
        public int rowIndex { get; set; }
        public ObservationHelper(SectionHelp.Surveyquestionanswerlist question,string buttonName,int rowInd)
        {
            Question = question;
            ButtonName = buttonName;
            rowIndex = rowInd;
        }
    }
}
