using CMS_Survey.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CMS_Survey.Pages
{
    public enum MyResult
    {
        Save,
        ReturnRevision,
        Approve,
        Nothing
    }

    public sealed partial class CommentsDialog : ContentDialog
    {
        public MyResult Result { get; set; }
        public string SurveyKey { get; set; }
        SectionHelp.Section _section;

        public event EventHandler NavigateToMainPage;
        public CommentsDialog()
        {
            this.InitializeComponent();
        }

        internal  void OnNavigationCalled(EventArgs e)
        {
            EventHandler handler = NavigateToMainPage;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public CommentsDialog(SectionHelp.Section Section)
        {
            this.InitializeComponent();
            _section = Section;
            if (_section != null)
            {
                var ansList = _section.surveyQuestionAnswerList.Where(e => e.questionId == -3).Select(e => e.answersList).FirstOrDefault();
                var ans = ansList.FirstOrDefault();
                if (ans != null)
                {
                    CommentsTextBox.Text = Convert.ToString(ans.answer);
                }
            }
          
        }
        private async void SaveAndContinue_Click(object sender, RoutedEventArgs e)
        {
            if (!await IsOnline())
                return;
            bool issuccess = await Services.ServiceHelper.ServiceHelperObject.ApproveOrReject(SurveyKey, "SAVE", CommentsTextBox.Text);
            if (issuccess)
            {
                bool success=await DownloadSurveyInfo();
                if(success)
                dialog.Hide();
                else
                {
                    MessageDialog md = new MessageDialog("Error occured while saving", "Error");
                    await md.ShowAsync();
                }
              
            }
            else
            {
                MessageDialog md = new MessageDialog("Error occured while saving", "Error");
                await md.ShowAsync();
            }
        }
        public void HideActionsGrid()
        {
            this.ActionsGrid.Visibility = Visibility.Collapsed;
        }
        public void MakeCloseVisible()
        {
            this.CloseGrid.Visibility = Visibility.Visible;
        }
        public void MakeCloseInVisible()
        {
            this.CloseGrid.Visibility = Visibility.Collapsed;
        }
        public void MakeCommentsReadonly()
        {
            this.CommentsTextBox.IsReadOnly = true;
        }
        public void SetComments(string Comments)
        {
            this.CommentsTextBox.Text = Comments;
        }
        private async void SaveReturnRevision_Click(object sender, RoutedEventArgs e)
        {

            if (!await IsOnline())
                return;
            bool issuccess = await Services.ServiceHelper.ServiceHelperObject.ApproveOrReject(SurveyKey, "Revision Required", CommentsTextBox.Text);
            if (issuccess)
            {
                bool success = await DownloadSurveyInfo();
                if (success)
                    dialog.Hide();
                else
                {
                    MessageDialog md = new MessageDialog("Error occured while returning for Revision", "Error");
                    await md.ShowAsync();
                }
                OnNavigationCalled(null);
            }
            else
            {
                MessageDialog md = new MessageDialog("Error occured while Returning for Revision", "Error");
                await md.ShowAsync();
            }

        }

        private async void SaveApprove_Click(object sender, RoutedEventArgs e)
        {

            if (!await IsOnline())
                return;
            bool issuccess = await Services.ServiceHelper.ServiceHelperObject.ApproveOrReject(SurveyKey, "Approved", CommentsTextBox.Text);
            if (issuccess)
            {
                bool success = await DownloadSurveyInfo();
                if (success)
                    dialog.Hide();
                else
                {
                    MessageDialog md = new MessageDialog("Error occured while approving", "Error");
                    await md.ShowAsync();
                }
                OnNavigationCalled(null);
            }
            else
            {
                MessageDialog md = new MessageDialog("Error occured while Approving", "Error");
                await md.ShowAsync();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            dialog.Hide();
        }

        public async Task<bool> IsOnline()
        {
            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
            {
                return true;
            }
            else
            {
                MessageDialog md = new MessageDialog("Application is not online, cannot perform this action", "Warning");
                await md.ShowAsync();
                return false;
            }
        }

        private void ShowProgress()
        {
            ProgressR.Visibility = Visibility.Visible;
            ProgressR.IsActive = true;
        }
        private void HideProgress()
        {
            ProgressR.Visibility = Visibility.Collapsed;
            ProgressR.IsActive = false;
        }
        public async Task<bool> DownloadSurveyInfo()
        {
            bool saveSuccessFul = false;
            ShowProgress();
            try
            {

                this.SaveAndContinue.Visibility = Visibility.Collapsed;
                this.SaveReturnRevision.Visibility = Visibility.Collapsed;
                this.SaveApprove.Visibility = Visibility.Collapsed;
                await Services.ServiceHelper.ServiceHelperObject.CallGetSurveyService(Convert.ToString(Services.ServiceHelper.ServiceHelperObject.currentUser.userKey), SurveyKey);
                await Services.ServiceHelper.ServiceHelperObject.CallUserSurveyService();
                saveSuccessFul = true;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                HideProgress();
            }
            return saveSuccessFul;
        }
    }
}
