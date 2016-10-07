﻿using System;
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
using CMS_Survey.Helpers;
using Windows.UI.Popups;
using CMS_Survey.Models;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CMS_Survey.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignSurvey : Page
    {
        Surveyors users = null;
        List<Models.User> AllUsers = null;
        List<SurveyorControls> SurveyorList = new List<SurveyorControls>();
        int rowIndex;
        int totalUserCount;
        long m_SurveyKey;
        public AssignSurvey()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
          
            base.OnNavigatedTo(e);
            SurveyKeyText.Text = "";
            ProviderKeyText.Text = "";
            totalUserCount = 0;
            if (await Services.ServiceHelper.ServiceHelperObject.IsOffline())
            {
              
                if (e.Parameter != null)
                {
                    var surv = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Models.UserSurvey>(Convert.ToString(e.Parameter));
                    SurveyKeyText.Text = surv.surveyKey;
                    ProviderKeyText.Text = surv.surveyProvider;
                    users = await Services.ServiceHelper.ServiceHelperObject.GetSurveyorForSurvey(surv.surveyKey);
                    AddExistingUsersandNew();
                }
                else if (e.Parameter == null)
                {
                   

                }
            }
            else
            {
               
                if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                {
                    if (e.Parameter != null)
                    {
                        AllUsers = await Services.ServiceHelper.ServiceHelperObject.GetUsersForState("ALL");
                        //var surKey = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));

                        // var res = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));
                        var surv = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Models.UserSurvey>(Convert.ToString(e.Parameter));
                        SurveyKeyText.Text = surv.surveyKey;
                        ProviderKeyText.Text = surv.surveyProvider;
                        m_SurveyKey = Convert.ToInt64(surv.surveyKey);
                        users = await Services.ServiceHelper.ServiceHelperObject.GetSurveyorForSurvey(surv.surveyKey);
                        users.userKeys.Remove(Services.ServiceHelper.ServiceHelperObject.currentUser.userKey);
                        AddExistingUsersandNew();
                    }
                    else if (e.Parameter == null)
                    {
                        
                    }
                }
                else
                {
                  
                    NavigateToMainPage();
                    return;
                }
            }
          
        }

        private void AddExistingUsersandNew()
        {
            rowIndex = 1;
            string fisrtName;
            string lastName;
           
                foreach (long user in users.userKeys)
                {
                    var usr = AllUsers.Where(e => e.userKey.Equals(user)).Select(e => e).FirstOrDefault();
                    fisrtName = usr.FirstName;
                    lastName = usr.LastName;

                    TextBlock userLabel = new TextBlock();
                    userLabel.Text = "User  :";
                    userLabel.TextWrapping = TextWrapping.Wrap;
                    AddUIControlWithAlignment(rowIndex , HorizontalAlignment.Left, userLabel, 1);
                    userLabel = new TextBlock();
                    userLabel.Text =string.Format("{0},{1}",fisrtName,lastName);
                    userLabel.TextWrapping = TextWrapping.Wrap;
                   
                    AddUIControlWithAlignment(rowIndex, HorizontalAlignment.Center, userLabel, 1);
                    addBlankLine(UserDataGrid, rowIndex++);
                    addBlankLine(UserDataGrid, rowIndex++);
                
                }
          
                rowIndex = AddNewSurveyor(rowIndex);
                //addUIControl(UserDataGrid, cmbbox1, rowIndex++);
            
        }

        private int AddNewSurveyor(int rowIndex)
        {
            SurveyorControls surV = new Helpers.SurveyorControls();
            TextBlock questionlabel = new TextBlock();
            questionlabel.Text = "State";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(UserDataGrid, questionlabel, rowIndex++);

            ComboBox cmbbox = new ComboBox();
            cmbbox.Name = "StateSelectCombobox_"+totalUserCount.ToString();
            cmbbox.Width = 66;
            cmbbox.Items.Add("ALL");
            Services.ServiceHelper.ServiceHelperObject.StateCode.Select(e => e.stateCode).ToList().ForEach(t => cmbbox.Items.Add(t));
            surV.StateComboBox = cmbbox;
            cmbbox.SelectionChanged += StateSelectionComboboxChanged;
            addUIControl(UserDataGrid, cmbbox, rowIndex++);
            questionlabel = new TextBlock();
            questionlabel.Text = "User";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            AddUIControlWithAlignment(rowIndex - 2, HorizontalAlignment.Center, questionlabel, 1);
            // addUIControl(UserDataGrid, questionlabel, rowIndex++);
            ComboBox cmbbox1 = new ComboBox();
            cmbbox1.Name = "UsersCombobox_"+totalUserCount.ToString();
            cmbbox1.Width = 200;
            surV.UserCombobox = cmbbox1;
            AddUIControlWithAlignment(rowIndex - 1, HorizontalAlignment.Center, cmbbox1, 1);
            if (totalUserCount < 5)
            {
                Button btn = new Button();
                btn.Name = "AddSurveyorButton_"+totalUserCount.ToString();
                btn.Width = 120;
                btn.Height = 35;
                btn.Content = "Add Surveyor";
                btn.Click += AddSurveyorClicked;
                addBlankLine(UserDataGrid, rowIndex++);
                //addUIControl(UserDataGrid, btn, rowIndex++);
                surV.AddSurveyorButton = btn;
                AddUIControlWithAlignment(rowIndex - 2, HorizontalAlignment.Right, btn, 2);
                SurveyorList.Add(surV);
                rowIndex = rowIndex + 3;
                totalUserCount = totalUserCount + 1;
            }
            
           
            return rowIndex;
        }

        private void AddSurveyorClicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
           
           
            btn.Visibility = Visibility.Collapsed;
            if (totalUserCount == 5)
            {
                return;
            }
            rowIndex = AddNewSurveyor(rowIndex);
        }

        private void AddUIControlWithAlignment(int rowIndex,HorizontalAlignment alignment, FrameworkElement uiComponent,int columnSpan)
        {
           
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            UserDataGrid.RowDefinitions.Add(row);
            uiComponent.HorizontalAlignment = alignment;
            UserDataGrid.Children.Add(uiComponent);
            Grid.SetColumnSpan(uiComponent, columnSpan);
            Grid.SetRow(uiComponent, rowIndex);
            Grid.SetColumn(uiComponent, columnSpan);
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
        private async void StateSelectionComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
        
            var selectedItem = Convert.ToString(cmb.SelectedItem);
           
            ComboBox usrCmb = SurveyorList.Where(t => t.StateComboBox.Name.Equals(cmb.Name)).Select(t => t.UserCombobox).FirstOrDefault();
            List<string> UserList = null;
            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                UserList = await Services.ServiceHelper.ServiceHelperObject.GetUsersForStateCode(selectedItem);
            else
                UserList = await Services.ServiceHelper.ServiceHelperObject.GetUsersForStateOffline(selectedItem);
            UserList.ForEach(t => usrCmb.Items.Add(t));
        }

        private void addUIControl(Grid mainGrid, FrameworkElement uiComponent, int rowIndex)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            mainGrid.RowDefinitions.Add(row);
            mainGrid.Children.Add(uiComponent);
            Grid.SetRow(uiComponent, rowIndex);
            //addBlankLine(mainGrid, rowIndex);
        }
        private void NavigateToMainPage()
        {
            this.Frame.Navigate(typeof(Views.GridMainPage));
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {

            Surveyors surveyors = new Surveyors();
            surveyors.surveyKey = m_SurveyKey;
            List<long> UserKeys = new List<long>();
            foreach (SurveyorControls srvr in SurveyorList)
            {
                string username = Convert.ToString(srvr.UserCombobox.SelectedItem);
                if (string.IsNullOrEmpty(username))
                    continue;
                string firstName = username.Split(',').FirstOrDefault();
                string LastName = username.Split(',').LastOrDefault();
                UserKeys.Add(GetUserKeyForUser(firstName, LastName));
            }
            if (UserKeys.Count < 0)
                return;
            surveyors.userKeys = UserKeys;
         bool success=   await Services.ServiceHelper.ServiceHelperObject.AddSurveyors(surveyors);
            if (success)
                NavigateToMainPage();
            else
               ShowMessage("Failed to Add Surveyor's to the Survey","Error");
        }
        private async void ShowMessage(string message, string caption)
        {
            MessageDialog msgDialog = new MessageDialog(message, caption);
            IUICommand cmd = await msgDialog.ShowAsync();
      
        }
        private long GetUserKeyForUser(string FirstName,string LastName)
        {
            long key=-1;

          key=  AllUsers.Where(e => e.FirstName.Equals(FirstName) && e.LastName.Equals(LastName)).Select(e => e.userKey).FirstOrDefault();

            return key;
        }
    }
}
