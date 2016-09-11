using CMS_Survey.Template;
using CMS_Survey.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace CMS_Survey.Template
{
    public class SurveyDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InprogressTemplate { get; set; }
        public DataTemplate PendingReviewTemplate { get; set; }

        public DataTemplate ApprovedTemplate { get; set; }
        public DataTemplate SubmittedTemplate { get; set; }
        public DataTemplate RejectedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item,
                                                           DependencyObject container)
        {
            UserSurvey surVey = item as UserSurvey;

            if (surVey.status == Constants.InProgressStatus)
                return InprogressTemplate;
            if (surVey.status == Constants.PendingReview)
                return PendingReviewTemplate;
            if (surVey.status == Constants.Approved)
                return ApprovedTemplate;
            if (surVey.status == Constants.Submitted)
                return SubmittedTemplate;
            if (surVey.status == Constants.Rejected)
                return RejectedTemplate;


            return base.SelectTemplateCore(item, container);
        }
    }
}