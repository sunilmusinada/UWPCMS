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
    public enum Result
    {
        Continue,
        Cancel
    }

    public sealed partial class ErrorsWarningsDialog : ContentDialog
    {
        public Result Result { get; set; }
        public string SurveyKey { get; set; }
        SectionHelp.Section _section;
        List<string> UnasnweredList = new List<string>();
        string WarningText = @"	Warning : You have not answered this question.";
        public event EventHandler NavigateToMainPage;
        public ErrorsWarningsDialog()
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
        public ErrorsWarningsDialog(List<string> UnAnsweredList)
        {
            UnasnweredList = UnAnsweredList;
            this.InitializeComponent();
            ShowErrorsAndWarnings();
        }
  
        public void ShowErrorsAndWarnings()
        {
            int Rindex = 1;
            foreach (var questionText in UnasnweredList)
            {
                TextBlock txtBlock = new TextBlock();
                txtBlock.Text = questionText;
               // txtBlock.Width = 16;
                txtBlock.TextWrapping = TextWrapping.Wrap;
                addUIControl(WarningGrid, txtBlock, Rindex, 1);
                TextBlock txtBlock1 = new TextBlock();
                txtBlock1.Text = WarningText;
                txtBlock1.TextWrapping = TextWrapping.Wrap;
                addUIControl(WarningGrid, txtBlock1, Rindex, 2);
                addBlankLine(WarningGrid, Rindex++);
            }
        }
        private void addBlankLine(Grid mainGrid, int rowIndex)
        {
            TextBlock blank = new TextBlock();
            blank.Text = "   ";
            blank.TextWrapping = TextWrapping.Wrap;
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            mainGrid.RowDefinitions.Add(row);
            mainGrid.Children.Add(blank);
            Grid.SetRow(blank, rowIndex);
        }
        private void addUIControl(Grid mainGrid, FrameworkElement uiComponent, int rowIndex, int column = -1)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            mainGrid.RowDefinitions.Add(row);
            mainGrid.Children.Add(uiComponent);
            Grid.SetRow(uiComponent, rowIndex);
            if (column != -1)
            {
                if (column == 1)
                {
                    uiComponent.HorizontalAlignment = HorizontalAlignment.Left;
                    uiComponent.Width = 160;
                }
                else if (column == 2)
                    uiComponent.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumnSpan(uiComponent, column);

                Grid.SetColumn(uiComponent, column);
            }
            //addBlankLine(mainGrid, rowIndex);
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

      

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            this.Result = Result.Continue;
            dialog.Hide();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result=Result.Cancel;
            dialog.Hide();
        }
    }
}
