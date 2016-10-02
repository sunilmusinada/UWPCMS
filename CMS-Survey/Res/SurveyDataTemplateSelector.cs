using CMS_Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CMS_Survey.Res
{
   public sealed class MyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InProgress { get; set; }
        public DataTemplate Approved { get; set; }
        protected override DataTemplate SelectTemplateCore(object item,
                                                      DependencyObject container)
        {
            UserSurvey usrSrvyy = item as UserSurvey;
            if (usrSrvyy.status=="In Progress")
                return InProgress;
            if (usrSrvyy.status == "Approved")
                return Approved;

            return base.SelectTemplateCore(item, container);
        }
    }
}
