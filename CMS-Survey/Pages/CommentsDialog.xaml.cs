﻿using System;
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
        public CommentsDialog()
        {
            this.InitializeComponent();
        }
        private async void SaveAndContinue_Click(object sender, RoutedEventArgs e)
        {
            if (!await IsOnline())
                return;
           bool issuccess=await Services.ServiceHelper.ServiceHelperObject.ApproveOrReject(SurveyKey, "SAVE", CommentsTextBox.Text);
            if(issuccess)
            dialog.Hide();
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
                dialog.Hide();
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
                dialog.Hide();
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
            if(!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
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
    }
}
