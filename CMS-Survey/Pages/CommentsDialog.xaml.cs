using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public CommentsDialog()
        {
            this.InitializeComponent();
        }
        private void SaveAndContinue_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MyResult.Save;
            // Close the dialog
            dialog.Hide();
        }

        private void SaveReturnRevision_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MyResult.ReturnRevision;
            // Close the dialog
            dialog.Hide();
        }

        private void SaveApprove_Click(object sender, RoutedEventArgs e)
        {
            this.Result = MyResult.Approve;
            // Close the dialog
            dialog.Hide();
        }
    }
}
