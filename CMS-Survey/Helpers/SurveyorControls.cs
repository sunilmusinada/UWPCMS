using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
namespace CMS_Survey.Helpers
{
    public class SurveyorControls
    {
        public CMSCombobox StateComboBox { get; set; }
        public AutoSuggestBox UserCombobox { get; set; }

        public Button AddSurveyorButton { get; set; }
    }
}
