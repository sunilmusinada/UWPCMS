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
using CMS_Survey.Helpers;
using Windows.UI.Popups;
using CMS_Survey.Models;
using System.Threading.Tasks;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CMS_Survey.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignSurvey : Page
    {
        Surveyors users = null;
        public IUICommand btnResult;
        List<Models.User> AllUsers = null;
        List<SurveyorControls> SurveyorList = new List<SurveyorControls>();
        int rowIndex;
        int totalUserCount;
        long m_SurveyKey;
        UserSurvey SelectedSurvey;
        NavigationEventArgs Eventargs;
        Surveyors DeleteUsrs;
        List<string> UsersList = new List<string>();
        public AssignSurvey()
        {
            this.InitializeComponent();
            DeleteUsrs = new Surveyors();
            DeleteUsrs.surveyerUserModels = new List<Models.SurveyerUserModel>();

        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            Submit.Visibility = Visibility.Visible;
            SurveyKeyText.Text = "";
            ProviderKeyText.Text = "";
            totalUserCount = 0;
            CMS_Survey.Template.SurveyHelper.SurveyHelperObject.CreateSurveyList();
            var surList = CMS_Survey.Template.SurveyHelper.SurveyJsonList;
            if (await Services.ServiceHelper.ServiceHelperObject.IsOffline())
            {

                if (e.Parameter != null)
                {
                    Eventargs = e;
                    AllUsers = await Services.ServiceHelper.ServiceHelperObject.GetFullUsersOffline("ALL");
                    SelectedSurvey = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Models.UserSurvey>(Convert.ToString(e.Parameter));
                    SurveyKeyText.Text = SelectedSurvey.surveyKey;
                    ProviderKeyText.Text = SelectedSurvey.surveyProvider;
                    var otherUsersList = surList.Where(t => t.surveyKey.Equals(SelectedSurvey.surveyKey)).Select(t => t.otherSurveyerKeys).FirstOrDefault();
                    if (otherUsersList == null && otherUsersList.Count <= 0)
                        return;
                    var surveyors = new Surveyors();
                    surveyors.surveyerUserModels = new List<Models.SurveyerUserModel>();
                    m_SurveyKey = Convert.ToInt64(SelectedSurvey.surveyKey);
                    surveyors.surveyKey = m_SurveyKey;
                    var surUserModel = from t in otherUsersList
                                       select new Models.SurveyerUserModel() { userKey = t, delete = false };
                    surveyors.surveyerUserModels = surUserModel.ToList();
                    users = surveyors;
                    AddExistingUsersandNew(true);
                    Submit.Visibility = Visibility.Collapsed;
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
                        Eventargs = e;
                        Submit.Visibility = Visibility.Visible;
                        AllUsers = await Services.ServiceHelper.ServiceHelperObject.GetUsersForState("ALL");
                        //var surKey = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));

                        // var res = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));
                        SelectedSurvey = Template10.Services.SerializationService.SerializationService.Json.Deserialize<Models.UserSurvey>(Convert.ToString(e.Parameter));
                        SurveyKeyText.Text = SelectedSurvey.surveyKey;
                        ProviderKeyText.Text = SelectedSurvey.surveyProvider;
                        m_SurveyKey = Convert.ToInt64(SelectedSurvey.surveyKey);
                        //var otherUsersList = surList.Where(t => t.surveyKey.Equals(SelectedSurvey.surveyKey)).Select(t => t.otherSurveyerKeys).FirstOrDefault();
                        var otherUsersList = await Services.ServiceHelper.ServiceHelperObject.GetSurveyorForSurvey(SelectedSurvey.surveyKey);

                        if (otherUsersList == null && otherUsersList.surveyerUserModels.Count <= 0)
                            return;
                        var surveyors = new Surveyors();
                        surveyors.surveyerUserModels = new List<Models.SurveyerUserModel>();
                        surveyors.surveyKey = m_SurveyKey;
                        //var surUserModel = from t in otherUsersList
                        //                   select new Models.SurveyerUserModel() { userKey = t, delete = false };
                        //surveyors.surveyerUserModels = surUserModel.ToList();
                        //surveyors.surveyerUserModels=otherUsersList.
                        users = otherUsersList;
                        //users = await Services.ServiceHelper.ServiceHelperObject.GetSurveyorForSurvey(surv.surveyKey);
                        // users.userKeys.Remove(Services.ServiceHelper.ServiceHelperObject.currentUser.userKey);
                        AddExistingUsersandNew();
                    }
                    else if (e.Parameter == null)
                    {

                    }
                }
                else
                {

                    NavigateToMainPage("Assign");
                    return;
                }
            }

        }

        private void AddExistingUsersandNew(bool isOffline = false)
        {
            rowIndex = 1;
            string fisrtName;
            string lastName;
            UserDataGrid.Children.Clear();
            DeleteUsrs.surveyKey = m_SurveyKey;
            addBlankLine(UserDataGrid, rowIndex++);
            addBlankLine(UserDataGrid, rowIndex++);
            foreach (var user in users.surveyerUserModels)
            {
                var usr = AllUsers.Where(e => e.userKey.Equals(user.userKey)).Select(e => e).FirstOrDefault();
                fisrtName = usr.FirstName;
                lastName = usr.LastName;

                TextBlock userLabel = new TextBlock();
                userLabel.Text = "User  :";
                userLabel.TextWrapping = TextWrapping.Wrap;
                AddUIControlWithAlignment(rowIndex++, HorizontalAlignment.Left, userLabel, 1);
                // addUIControl(UserDataGrid,userLabel, rowIndex);
                userLabel = new TextBlock();
                userLabel.Text = string.Format("{0} {1}", fisrtName, lastName);
                userLabel.TextWrapping = TextWrapping.Wrap;
                AddUIControlWithAlignment(rowIndex - 1, HorizontalAlignment.Center, userLabel, 1);
                if (!isOffline)
                {
                    Button btn = new Button();
                    btn.Content = "Remove";
                    btn.Name = user.userKey.ToString();
                    btn.Click += RemoveButton;
                    btn.Width = 120;
                    btn.Height = 35;
                    btn.IsEnabled = true;
                    btn.Visibility = Visibility.Visible;
                    //UserDataGrid.Children.Add(btn);
                    //if(!Services.ServiceHelper.ServiceHelperObject.currentUser.userKey.ToString().Equals(user.ToString()))
                    if (user.delete)
                        AddbuttonWithAlignment(rowIndex - 1, HorizontalAlignment.Right, btn, 3);
                }
                addBlankLine(UserDataGrid, rowIndex++);
                //addBlankLine(UserDataGrid, rowIndex++);

            }
            if (!isOffline)
                rowIndex = AddNewSurveyor(rowIndex);
            //addUIControl(UserDataGrid, cmbbox1, rowIndex++);

        }

        private async void RemoveButton(object sender, RoutedEventArgs e)
        {
            await RemoveButtonClicked(sender);
        }
        public async Task RemoveButtonClicked(object sender)
        {
            Button btn = sender as Button;
            //await RemoveButtonShowMessageDialog_Click(null, null);
            //if (Convert.ToInt32(btnResult.Id) == 0)
            //{
            if (users.surveyerUserModels.Count == 1)
            {
                ShowMessage("There's only one User is assigned to this Survey, you are not allowed to remove the user", "Warning");
                return;
            }
            if (btn != null)
            {
                if (users == null || users.surveyerUserModels == null)
                    return;
                var usr=users.surveyerUserModels.Where(e => e.userKey.Equals(Convert.ToInt64(btn.Name))).FirstOrDefault();
                if (usr == null)
                    return;
                users.surveyerUserModels.Remove(usr);
                //users.Remove(Convert.ToInt64(btn.Name));
                DeleteUsrs.surveyerUserModels.Add(new Models.SurveyerUserModel() { userKey = Convert.ToInt64(btn.Name), delete = false });
                //DeleteUsrs.userKeys.Add(Convert.ToInt64(btn.Name));
                AddExistingUsersandNew();
            }
            //}
            // throw new NotImplementedException();
        }
        private int AddNewSurveyor(int rowIndex)
        {
            SurveyorControls surV = new Helpers.SurveyorControls();
            TextBlock questionlabel = new TextBlock();
            questionlabel.Text = "State";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(UserDataGrid, questionlabel, rowIndex++);

            CMSCombobox cmbbox = new CMSCombobox();
            cmbbox.Name = "StateSelectCombobox_" + totalUserCount.ToString();
            cmbbox.Width = 75;
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
            AutoSuggestBox autSugBox = new AutoSuggestBox();
            autSugBox.Name = "UsersCombobox_" + totalUserCount.ToString();
            autSugBox.Width = 200;
            autSugBox.TextChanged += AutSugBox_TextChanged;
            autSugBox.SuggestionChosen += AutSugBox_SuggestionChosen;
            surV.UserCombobox = autSugBox;

            //CMSCombobox cmbbox1 = new CMSCombobox();
            //cmbbox1.Name = "UsersCombobox_" + totalUserCount.ToString();
            //cmbbox1.Width = 200;
            //surV.UserCombobox = cmbbox1;
            AddUIControlWithAlignment(rowIndex - 1, HorizontalAlignment.Center, autSugBox, 1);
            if (totalUserCount < 5)
            {
                Button btn = new Button();
                btn.Name = "AddSurveyorButton_" + totalUserCount.ToString();
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

            cmbbox.SelectedValue = "ALL";
            return rowIndex;
        }

        private void AutSugBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            AutoSuggestBox atBx = sender as AutoSuggestBox;
            if (atBx == null)
                return;
            if (args.SelectedItem == null)
                return;
            atBx.Text = Convert.ToString(args.SelectedItem);
        }

        private void AutSugBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            AutoSuggestBox atBx = sender as AutoSuggestBox;
            List<string> items = UsersList;
            if (items == null && items.Count == 0)
                return;
            var txt = atBx.Text;//.ToLower();
            if (args.Reason==AutoSuggestionBoxTextChangeReason.UserInput)
            {
                items = items.Where(e => e.Contains(txt)).OrderBy(e=>e).Select(e => e).ToList();
                atBx.ItemsSource = items;
            }
        }

        private void AddSurveyorClicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;


            btn.Visibility = Visibility.Collapsed;

            rowIndex = AddNewSurveyor(rowIndex);
        }

        private void AddUIControlWithAlignment(int rowIndex, HorizontalAlignment alignment, FrameworkElement uiComponent, int columnSpan)
        {

            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            UserDataGrid.RowDefinitions.Add(row);
            uiComponent.HorizontalAlignment = alignment;
            UserDataGrid.Children.Add(uiComponent);
            Grid.SetColumnSpan(uiComponent, columnSpan);
            Grid.SetRow(uiComponent, rowIndex);
            //Grid.SetColumn(uiComponent, columnSpan);
        }
        private void AddbuttonWithAlignment(int rowIndex, HorizontalAlignment alignment, Button btn, int columnSpan)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            UserDataGrid.RowDefinitions.Add(row);
            btn.HorizontalAlignment = alignment;
            UserDataGrid.Children.Add(btn);

            Grid.SetColumnSpan(btn, columnSpan);
            Grid.SetRow(btn, rowIndex);
            //Grid.SetColumn(btn, columnSpan);
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
            List<string> usrList = new List<string>();
            var selectedItem = Convert.ToString(cmb.SelectedItem);
           
            AutoSuggestBox usrCmb = SurveyorList.Where(t => t.StateComboBox.Name.Equals(cmb.Name)).Select(t => t.UserCombobox).FirstOrDefault();

            if (usrCmb != null && usrCmb.Items != null && usrCmb.Items.Count > 0)
                usrCmb.ItemsSource = usrList;
            List<string> UserList = null;
            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                UserList = await Services.ServiceHelper.ServiceHelperObject.GetUsersForStateCode(selectedItem);
            else
                UserList = await Services.ServiceHelper.ServiceHelperObject.GetUsersForStateOffline(selectedItem);
            UsersList = UserList;
            foreach (var usr in UserList.OrderBy(f => f))
            {
                string firstName = usr.Split(' ').FirstOrDefault();
                string LastName = usr.Split(' ').LastOrDefault();
                long usrKey = GetUserKeyForUser(firstName, LastName);
                if (!users.surveyerUserModels.Select(t=>t.userKey).ToList().Contains(usrKey))
                    usrList.Add(usr);
            }
            usrCmb.ItemsSource = usrList;
            //UserList.ForEach(t => usrCmb.Items.Add(t));
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
        private void NavigateToMainPage(string Source)
        {
            var param = Template10.Services.SerializationService.SerializationService.Json.Serialize(Source);
            this.Frame.Navigate(typeof(Views.GridMainPage), param);
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowProgress();
                Submit.Visibility = Visibility.Collapsed;
                ResetButton.Visibility = Visibility.Collapsed;
                Surveyors surveyors = new Surveyors();
                surveyors.surveyerUserModels = new List<Models.SurveyerUserModel>();
                surveyors.surveyKey = m_SurveyKey;
                List<long> UserKeys = new List<long>();
                foreach (SurveyorControls srvr in SurveyorList)
                {
                    string username = Convert.ToString(srvr.UserCombobox.Text);
                    if (string.IsNullOrEmpty(username))
                        continue;
                    string firstName = username.Split(' ').FirstOrDefault();
                    string LastName = username.Split(' ').LastOrDefault();
                    var usrKey = GetUserKeyForUser(firstName, LastName);
                    if (!UserKeys.Contains(usrKey))
                        UserKeys.Add(usrKey);
                }
                if (UserKeys.Count < 0)
                    return;
                UserKeys.ForEach(t => surveyors.surveyerUserModels.Add(new Models.SurveyerUserModel() { userKey = t, delete = false }));
                //surveyors.surveyerUserModels.Add( = UserKeys;
                bool success = false;
                if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                {
                    if (DeleteUsrs != null && DeleteUsrs.surveyerUserModels.Count > 0)
                        success = await Services.ServiceHelper.ServiceHelperObject.DeleteSurveyors(DeleteUsrs);
                    success = await Services.ServiceHelper.ServiceHelperObject.AddSurveyors(surveyors);
                    await Services.ServiceHelper.ServiceHelperObject.CallUserSurveyService();
                    await Services.ServiceHelper.ServiceHelperObject.CallUserSurveyService();
                }
                else
                {
                    success = await Services.ServiceHelper.ServiceHelperObject.AddSurveyorsOffline(surveyors);
                }
                if (success)
                    AddSurveyorToSurvey();
                else
                    ShowMessage("Failed to Add Surveyor's to the Survey", "Error");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                HideProgress();
                ResetButton.Visibility = Visibility.Visible;
            }
        }
        private async void ShowMessage(string message, string caption)
        {
            MessageDialog msgDialog = new MessageDialog(message, caption);
            IUICommand cmd = await msgDialog.ShowAsync();

        }
        private long GetUserKeyForUser(string FirstName, string LastName)
        {
            long key = -1;

            key = AllUsers.Where(e => e.FirstName.Equals(FirstName) && e.LastName.Equals(LastName)).Select(e => e.userKey).FirstOrDefault();

            return key;
        }

        private void AddSurveyorToSurvey()
        {
            try
            {

                var param = Template10.Services.SerializationService.SerializationService.Json.Serialize(SelectedSurvey);
                this.Frame.Navigate(typeof(Views.AssignSurvey), param);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void ResetButto_Click(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(Views.AssignSurvey), Eventargs);
            //UserDataGrid.Children.Clear();
            //OnNavigatedTo(Eventargs);
            await ButtonShowMessageDialog_Click(null, null);
            if (Convert.ToInt32(btnResult.Id) == 0)
            {
                var param = Template10.Services.SerializationService.SerializationService.Json.Serialize(SelectedSurvey);
                this.Frame.Navigate(typeof(Views.AssignSurvey), param);
            }

        }

        private async Task ButtonShowMessageDialog_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Windows.UI.Popups.MessageDialog("Refresh will remove any pending changes in this page.");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Continue") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Cancel") { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            btnResult = await dialog.ShowAsync();

            //var btn = sender as Button;
            //tn.Content = $"Result: {result.Label} ({result.Id})";
        }
        private async Task RemoveButtonShowMessageDialog_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Windows.UI.Popups.MessageDialog("Removing the user for this Survey will remove all the answers from this user.");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Continue") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Cancel") { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            btnResult = await dialog.ShowAsync();

            //var btn = sender as Button;
            //tn.Content = $"Result: {result.Label} ({result.Id})";
        }
        private void HideProgress()
        {
            progressRing.IsActive = false;
            progressRing.Visibility = Visibility.Collapsed;
        }

        private void ShowProgress()
        {
            progressRing.Visibility = Visibility.Visible;
            progressRing.IsActive = true;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            CMS_Survey.Pages.NewSurvey.isEnabled = true;
            CMS_Survey.Pages.NewSurvey.isCommentsEnabled = false;
            CMS_Survey.Pages.NewSurvey.isReview = false;
            GetClickedSurvey(SelectedSurvey.surveyKey);
        }
        private void GetClickedSurvey(string SurveyKey)
        {
            try
            {

                var param = Template10.Services.SerializationService.SerializationService.Json.Serialize(SurveyKey);
                this.Frame.Navigate(typeof(Pages.NewSurvey), param);



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
