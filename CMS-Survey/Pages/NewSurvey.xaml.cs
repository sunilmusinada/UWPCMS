using CMS_Survey.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows;
using Windows.UI.Popups;
using CMS_Survey.Template;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using CMS_Survey.Helpers;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Win32;
using Windows.Storage.Pickers;
using System.Collections.ObjectModel;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CMS_Survey.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// Not an empty page, this will either load an new survey or an existing survey.
    /// </summary>
    public sealed partial class NewSurvey : Page
    {
        bool isUserOwner = true;
        List<string> UnAnsweredList = null;
        private int sectionIndex = 0;
        public static bool isInCheckBoxState = false;
        SectionHelp.Rootobject result = new SectionHelp.Rootobject();
        List<UserObservation> UserObservationList;
        private int groupIndex = 0;
        private string HospitalControlName;
        private List<string> _Hospitals;
        private string CcnControlName;
        private TextBox ccnTextBox;
        private static List<Hospital> SelectedHospitals;
        public static bool isEnabled = true;
        public static bool isCommentsEnabled = false;
        private string FetchedHospitalCcn;
        private bool fromSavedObject = false;
        List<string> Maillist;
        private string SelectedState = null;
        private bool LoadedOffline = false;
        private CMSCombobox stateControl = null;
        private CMSCombobox HospitalControl = null;
        private AutoSuggestBox MailIdCombobox = null;
        private TextBox Hospitalcn = null;
        private TextBlock StateErrorBlock, HospitalErrorBlock, HospitalCnBlock, FromErrorBlock;
        List<JumpClass> jmpClass;
        string tempObservvationQuestionId = string.Empty;
        string tempCitiationId = string.Empty;
        List<ObservationHelper> ObservationsList = null;
        List<Tuple<string, string>> CitiableItems = new List<Tuple<string, string>>();
        Dictionary<int, int> CantObserveKey;
        Dictionary<int, int> QuestionObservationDictionary;
        public IUICommand btnResult;
        List<Models.User> AllUsers = null;
        DatePicker fromTimePicker, toTimePicker;
        public string SurveyKey;
        List<CheckBox> UpdatedCheckBoxes;
        public static bool isReview = false;
        public NewSurvey()
        {

            this.InitializeComponent();
            SurveyHelper.SurveyHelperObject.FinishedDownloading += SurveyHelperObject_FinishedDownloading;
            if (SurveyHelper.SurveyHelperObject.DownloadFinished)
            {
                ChangeIndicator();
            }


        }

        private void SurveyHelperObject_FinishedDownloading(object sender, EventArgs e)
        {
            ChangeIndicator();
        }


        private void ChangeIndicator()
        {
            string url = "ms-appx:///Assets/green-glossy-ball.jpg";
            image.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (e.Parameter == null)
                {
                    isEnabled = true;
                    isCommentsEnabled = false;

                }
                CommentsButton.Visibility = Visibility.Collapsed;
                ObservationsList = new List<Models.ObservationHelper>();
                SectionHelp.Rootobject survObj = null;
                // base.OnNavigatedTo(e);
                ShowProgress();
                if (await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                {
                    LoadedOffline = true;
                    AllUsers = await Services.ServiceHelper.ServiceHelperObject.GetFullUsersOffline("ALL");
                    if (e.Parameter != null)
                    {
                        var surKey = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));
                        SurveyKey = Convert.ToString(surKey);
                        if (CMS_Survey.Template.SurveyHelper.SurveyJsonList == null || CMS_Survey.Template.SurveyHelper.SurveyJsonList.Count == 0)
                        {
                            ShowMessage("The selected survey is not available now because application is Offline.You will be re-directed to the Main page", "Error");
                            NavigateToMainPage("Error");
                            return;
                        }
                        else
                        {
                            result = CMS_Survey.Template.SurveyHelper.GetSurveyFromLocal(Convert.ToString(surKey));//CMS_Survey.Template.SurveyHelper.SurveyList.Where(t => t.sections.First().surveyKey.ToString().Equals(surKey)).FirstOrDefault();
                                                                                                                   //result = Helpers.SurveyHelper.Request;
                        }
                    }
                    else if (e.Parameter == null)
                    {
                        ReadJsonFie();
                        UpdateKeys();

                    }
                }
                else
                {
                    LoadedOffline = false;
                    if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                    {
                        AllUsers = await Services.ServiceHelper.ServiceHelperObject.GetUsersForState("ALL");
                        if (e.Parameter != null)
                        {
                            //var surKey = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));

                            var res = Template10.Services.SerializationService.SerializationService.Json.Deserialize(Convert.ToString(e.Parameter));
                            SurveyKey = Convert.ToString(res);
                            if (SurveyHelper.SurveyHelperObject.DownloadFinished)
                            {
                                ChangeIndicator();
                            }
                            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                                result = await GetClickedSurvey(Convert.ToString(res));
                            isUserOwner = await Services.ServiceHelper.ServiceHelperObject.IsUserOwner(result.sections[0].surveyKey.ToString());

                        }
                        else if (e.Parameter == null)
                        {
                            ReadJsonFie();
                            UpdateKeys();
                        }
                    }
                    else
                    {
                        HideProgress();
                        ShowMessage("Application went offline. You will be re-directed to the Main page", "Error");
                        NavigateToMainPage("Error");
                        return;
                    }
                }

                survObj = getJson(mainGrid);
                if (survObj == null)
                {
                    HideProgress();
                    ShowMessage("Selected Survey is not available right now. You will be re-directed to the Main page", "Error");
                    NavigateToMainPage("Error");
                    return;
                }
                GetHelpDocuments();
                HideProgress();
                if (isCommentsEnabled)
                {
                    CommentsButton.Visibility = Visibility.Visible;
                }
                else
                {
                    CommentsButton.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                NavigateToMainPage("Error");

                ShowMessage("Selected Survey is not available right now. You will be re-directed to the Main page", "Error");

                return;
            }
            // parameters.Name
            // parameters.Text
            // ...
        }

        private async Task<SectionHelp.Rootobject> GetClickedSurvey(string SurveyKey)
        {
            var currentUserKey = Services.ServiceHelper.ServiceHelperObject.currentUser.userKey;
            SectionHelp.Rootobject secHelp = await Services.ServiceHelper.ServiceHelperObject.CallGetSurveyService(Convert.ToString(currentUserKey), SurveyKey);
            return secHelp;
        }
        private void UpdateKeys()
        {
            Services.ServiceHelper svcHelper = Services.ServiceHelper.ServiceHelperObject;
            // await svcHelper.CallLoginService("kalyan", "kalyancpamula");
            long surveyKey;
            if (result != null)
            {
                Windows.Storage.ApplicationDataContainer localSettings =
       Windows.Storage.ApplicationData.Current.LocalSettings;
                if (localSettings.Values.Keys.Contains("OfflineSurveyKey"))
                {
                    surveyKey = Convert.ToInt64(localSettings.Values["OfflineSurveyKey"]) - 1;
                }
                else
                {
                    localSettings.Values.Add("OfflineSurveyKey", -1);
                    surveyKey = -1;
                }
                foreach (var section in result.sections)
                {
                    section.status = 1;
                    section.surveyKey = surveyKey;
                    section.userKey = svcHelper.currentUser.userKey;
                }
            }
        }

        private void ReadJsonFie()
        {
            string FilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Answers_Hospital_Infection_Control_Worksheet.json");
            using (StreamReader file = File.OpenText(FilePath))
            {
                var json = file.ReadToEnd();
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<SectionHelp.Rootobject>(json);
                //System.Diagnostics.Debug.WriteLine(result);
            }
            GetHelpDocuments();
        }

        private void GetHelpDocuments()
        {
            CitiableItems = new List<Tuple<string, string>>();
            foreach (var section in result.sections)
            {

                foreach (SectionHelp.Surveyquestionanswerlist question in section.surveyQuestionAnswerList)
                {
                    CitiableItems.Add(new Tuple<string, string>(question.citableTagName, question.citableTagURL));
                }
            }
        }


        private SectionHelp.Rootobject getJson(Grid grid, bool scrollToTop = false, bool issetChckBx = false)
        {
            //
            //SurveyHelper svHelper = new SurveyHelper();
            //List<JumpClass> jmpClass= svHelper.GetJumpSections(result.sections);
            if (scrollToTop)
            {
                ScrollViewer scrollViewer = this.ScrollBar;
                if (scrollViewer != null)
                {
                    //var val = ScrollViewer.VerticalOffsetProperty;
                    scrollViewer.ScrollToVerticalOffset(0);
                }
            }

            QuestionObservationDictionary = new Dictionary<int, int>();
            CantObserveKey = new Dictionary<int, int>();
            SurveyHelper svHelper = SurveyHelper.SurveyHelperObject;
            if (result == null)
            {
                //
                //NavigateToMainPage();
                return null;
            }
            jmpClass = svHelper.GetJumpSections(result.sections);
            JumpButtonLoad();
            HelpButtonLoad();
            ShowProgress();
            mainGrid.Children.Clear();

            int rowIndex = 0;

            SectionHelp.Section section = result.sections[sectionIndex];

            TextBlock sectionLabel = new TextBlock();
            sectionLabel.Text = section.sectionTitle;
            sectionLabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(grid, sectionLabel, rowIndex++);
            addBlankLine(grid, rowIndex++);
            UpdatedCheckBoxes = new List<CheckBox>();
            foreach (SectionHelp.Surveyquestionanswerlist question in section.surveyQuestionAnswerList)
            {
                if (!question.renderQuestion)
                    continue;
                UserObservationList = new List<Helpers.UserObservation>();
                TextBlock questionlabel = new TextBlock();
                questionlabel.IsTextSelectionEnabled = true;
                questionlabel.Text = question.questionText.Replace("<br>", Environment.NewLine).Replace("<li>", Environment.NewLine)
                                     .Replace("<ul>", "").Replace("<\\li>", "").Replace("<\\ul>", "")
                                     .Replace("</li>", "").Replace("</ul>", "").Replace("<br/>", "").Replace("<br/>>", "").Replace("<div>", "").Replace("<div class=\"h4\"", "");
                questionlabel.TextWrapping = TextWrapping.Wrap;
                //bool renderObservation = question.renderAddObservation;
                addUIControl(grid, questionlabel, rowIndex++);
                if (!QuestionObservationDictionary.Keys.Contains(question.questionId))
                    QuestionObservationDictionary.Add(question.questionId, 1);
                //else
                //    QuestionObservationDictionary[question.questionId] = QuestionObservationDictionary[question.questionId] + 1;
                if (question.answersList != null && question.answersList.Length > 0)
                {
                    int ansIndex = 1;
                    int i = 1;
                    int j = 1;
                    foreach (SectionHelp.Answerslist answer in question.answersList)
                    {
                        j++;
                        AddControlByType(answer, grid, ref rowIndex, question, ansIndex);
                        if (answer.differentUserAnswerList != null && answer.differentUserAnswerList.Count > 0)
                        {
                            if (result.sections.Count() - 1 == sectionIndex)
                            {
                                if (question.questionId == -3)
                                {
                                    continue;
                                }
                            }
                            //AddOtherAnswers(answer, question, ref rowIndex);
                            AddUserObservation(answer, question.questionId, i);
                        }
                        if (j == 3)
                        {
                            i++;
                            j = 1;
                        }
                        if (!isReview)
                            addBlankLine(grid, rowIndex++);
                        ansIndex++;

                    }

                    if (question.renderAddObservation)

                    {

                        tempObservvationQuestionId = question.questionId.ToString();

                        //addBlankLine(grid, rowIndex++);
                        //addBlankLine(grid, rowIndex++);
                        if (question.obsevationNumber <= 5)
                            AddObservationButton(grid, rowIndex, question);
                        //AddRemoveObservationButton(grid, rowIndex, question);
                        ObservationsList.Add(new ObservationHelper(question, question.questionId.ToString(), rowIndex));

                    }
                    AddOtherAnswersPerUser(UserObservationList, question, ref rowIndex);
                }
                // }
            }
            if (!issetChckBx && UpdatedCheckBoxes.Count > 0)
                Setcheckboxes();
            if (result.sections.Count() - 1 == sectionIndex)
            {
                rowIndex = AddSurveyNotesSection(grid, rowIndex);
            }

            if (sectionIndex == result.sections.Count() - 1)
            {
                this.NextButton.Visibility = Visibility.Collapsed;
            }
            else
                this.NextButton.Visibility = Visibility.Visible;
            HideProgress();
            return result;
            //}
        }

        private void Setcheckboxes()
        {
            isInCheckBoxState = true;
            foreach (CheckBox chkBx in UpdatedCheckBoxes)
            {
                CheckBoxStateChanged(chkBx, !chkBx.IsChecked, true);
            }
            isInCheckBoxState = false;
            getJson(mainGrid, false, true);
        }
        public void AddUserObservation(SectionHelp.Answerslist answer, int QuestionId, int obsNumber)
        {
            UserObservation usrObs;
            int i = answer.observationNumber;

            foreach (SectionHelp.DifferentUserAnswerList Otheranswer in answer.differentUserAnswerList)
            {

                usrObs = new UserObservation();
                usrObs.UserKey = Otheranswer.user;
                usrObs.Answer = Otheranswer.answer;
                usrObs.AnswerDate = Convert.ToDateTime(Otheranswer.answerDate);
                usrObs.questionID = QuestionId;
                usrObs.ObservationNumber = obsNumber; //Convert.ToInt32(answer.observatgionNumberString);
                UserObservationList.Add(usrObs);
            }
        }
        private int AddSurveyNotesSection(Grid grid, int rowIndex)
        {
            SectionHelp.Surveyquestionanswerlist question = new Models.SectionHelp.Surveyquestionanswerlist();
            TextBlock questionlabel = new TextBlock();
            questionlabel.Text = "Survey Approver Details";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            //bool renderObservation = question.renderAddObservation;
            questionlabel.FontSize = 15;

            addUIControl(grid, questionlabel, rowIndex++);
            addBlankLine(grid, rowIndex++);
            questionlabel = new TextBlock();
            questionlabel.Text = "• Enter Email Address of the Approver in this section.";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(grid, questionlabel, rowIndex++);
            questionlabel = new TextBlock();
            questionlabel.Text = "• Upon submission of the survey, a notification will be sent to the Approver.";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(grid, questionlabel, rowIndex++);
            questionlabel = new TextBlock();
            questionlabel.Text = "• If Approver’s State is different from State of facility surveyed, use the drop - down to select the desired state.";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(grid, questionlabel, rowIndex++);
            addBlankLine(grid, rowIndex++);
            addBlankLine(grid, rowIndex++);
            addBlankLine(grid, rowIndex++);
            questionlabel = new TextBlock();
            questionlabel.Text = "State";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(grid, questionlabel, rowIndex++);
            CMSCombobox cmbbox = new CMSCombobox();
            cmbbox.Name = "StateSelectCombobox";
            cmbbox.Width = 66;
            cmbbox.Items.Add("ALL");
            Services.ServiceHelper.ServiceHelperObject.StateCode.Select(e => e.stateCode).ToList().ForEach(t => cmbbox.Items.Add(t));
            cmbbox.SelectionChanged += StateSelectionComboboxChanged;
            addUIControl(grid, cmbbox, rowIndex++);
            questionlabel = new TextBlock();
            questionlabel.Text = "Email address";
            questionlabel.TextWrapping = TextWrapping.Wrap;
            addUIControl(grid, questionlabel, rowIndex++);
            AutoSuggestBox autSugBox = new AutoSuggestBox();
            MailIdCombobox = autSugBox;
            autSugBox.TextChanged += AutoSuggestionTextChanged;
            autSugBox.SuggestionChosen += AutSugBox_SuggestionChosen;
            autSugBox.Width = 400;
            autSugBox.Name = "EmailIDCombobox";
            addUIControl(grid, autSugBox, rowIndex++,1);
            //CMSCombobox cmbbox1 = new CMSCombobox();
            //cmbbox1.Name = "EmailIDCombobox";
            //MailIdCombobox = cmbbox1;
            //cmbbox1.Width = 400;
            //addUIControl(grid, cmbbox1, rowIndex++);
            addBlankLine(grid, rowIndex++);
            addBlankLine(grid, rowIndex++);
            Button btn = new Button();
            btn.Content = "Submit";
            btn.Width = 200;
            btn.Click += SubmitButtonClicked;
            addUIControl(grid, btn, rowIndex++);
            cmbbox.SelectedItem = "ALL";
            return rowIndex;
        }

        private void AutSugBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args != null && args.SelectedItem != null)
            {
                var val = Convert.ToString(args.SelectedItem);
                MailIdCombobox.Text = val;
            }
        }

        private void AutoSuggestionTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            var suggestions = new List<string>();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                AutoSuggestBox atBx = sender as AutoSuggestBox;
                if (atBx == null)
                    return;
                if (string.IsNullOrEmpty(atBx.Text))
                {
                    return;
                }
                var txt = atBx.Text;//.ToLower();
                if (Maillist != null && Maillist.Count > 0)
                {
                    suggestions = Maillist.Where(e => e.Contains(txt)).OrderBy(e=>e).Select(e => e).ToList();
                    atBx.ItemsSource = suggestions;
                }
            }
        }

        private async void SubmitButtonClicked(object sender, RoutedEventArgs e)
        {
            string mailId = Convert.ToString(MailIdCombobox.Text);
            string SurveyKey = Convert.ToString(result.sections.First().surveyKey);
            if (string.IsNullOrEmpty(mailId))
                return;
            mailId = GetMaild(mailId);
            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
            {
                await SaveSurvey();
                bool success = await Services.ServiceHelper.ServiceHelperObject.CallAssignSurvey(mailId, SurveyKey);
                if (success)
                    this.NavigateToMainPage("Submit");
                else
                {
                    MessageDialog msgDialog = new MessageDialog("Submit Survey failed", "Error");
                    await msgDialog.ShowAsync();
                }
            }
            else
            {
                await SaveSurvey();
                Assignment assignment = new Assignment();
                assignment.Assignment_ID = -1;
                assignment.User_Key = Services.ServiceHelper.ServiceHelperObject.currentUser.userKey;
                assignment.Survey_Key = Convert.ToInt64(SurveyKey);
                assignment.EmailID = mailId;
                assignment.InsertAssignment(assignment);
            }

        }
        private string GetMaild(string userandMailId)
        {
            string mailid;
            mailid = userandMailId.Substring(userandMailId.IndexOf('(') + 1).Replace(")", "");
            return mailid;
        }
        private async void StateSelectionComboboxChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            var selectedItem = Convert.ToString(cmb.SelectedItem);
            MailIdCombobox.Items.Clear();
            //List<string> Maillist = null;
            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                Maillist = await Services.ServiceHelper.ServiceHelperObject.GetMaildsForState(selectedItem);
            else
                Maillist = await Services.ServiceHelper.ServiceHelperObject.GetMaildsForStateOffline(selectedItem);
            if (Maillist != null && Maillist.Count > 0)
            {
                Maillist = Maillist.OrderBy(f => f).ToList();
            }
            foreach (var mail in Maillist)
            {

                if (!GetMaild(mail).Equals(GetMailIdForUser(Services.ServiceHelper.ServiceHelperObject.currentUser.userKey)))
                    MailIdCombobox.Items.Add(mail);
            }
            // Maillist.ForEach(t => MailIdCombobox.Items.Add(t));
        }
        private string GetMailIdForUser(long userkey)
        {
            string key;

            key = AllUsers.Where(e => e.userKey.Equals(userkey)).Select(e => e.Email).FirstOrDefault();

            return key;
        }
        private void AddObservationButton(Grid grid, int rowIndex, SectionHelp.Surveyquestionanswerlist question)
        {
            Button Btn = new Button();
            //Btn.Name = question.questionId.ToString();
            Btn.Height = 40;
            Btn.Width = 150;
            Btn.IsEnabled = true;
            Btn.Click += AddObservationButtonClick;
            Btn.ClickMode = ClickMode.Press;
            int cntObservation = question.obsevationNumber;
            Btn.Name = question.questionId.ToString() + "_Add Observation_" + cntObservation.ToString();
            Btn.IsEnabled = IsEnabled;
            Btn.Content = "Add Observation " + cntObservation.ToString();
            if (!isReview)
                addUIControl(grid, Btn, rowIndex - 2);
        }
        private void AddRemoveObservationButton(Grid grid, int rowIndex, SectionHelp.Surveyquestionanswerlist question, int ObservationIndex)
        {
            if (isReview)
                return;
            if (result.sections.Count() - 1 == sectionIndex)
                return;
            int cntObservation = question.obsevationNumber - 1;
            if (cntObservation < 1)
                return;
            Button Btn = new Button();
            //Btn.Name = question.questionId.ToString();
            Btn.Height = 40;
            Btn.Width = 175;
            Btn.IsEnabled = true;
            Btn.Click += RemoveObservationButtonClick; ;
            Btn.ClickMode = ClickMode.Press;
            Btn.IsEnabled = isEnabled;
            Btn.HorizontalAlignment = HorizontalAlignment.Right;
            Btn.Content = "Remove Observation " + ObservationIndex.ToString();
            Btn.Name = question.questionId.ToString() + "_Remove Observation_" + ObservationIndex.ToString();
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            mainGrid.RowDefinitions.Add(row);
            mainGrid.Children.Add(Btn);
            Grid.SetRow(Btn, rowIndex - 1);

        }

        private void RemoveObservationButtonClick(object sender, RoutedEventArgs e)
        {
            Button bn = sender as Button;
            var name = bn.Name.Split('_').First();
            var index = Convert.ToInt32(bn.Name.Split('_').Last());
            var answerList = result.sections[sectionIndex].surveyQuestionAnswerList.Where(t => t.questionId.ToString().Equals(name)).Select(t => t).FirstOrDefault();
            if (answerList.obsevationNumber == 2)
            {
                answerList.answersList[0].answer = null;

                if (answerList.answersList[0].childIdList != null && answerList.answersList[0].childIdList.Count() > 0)
                    answerList.answersList[0].childIdList = null;
                answerList.answersList[1].answer = null;
                getJson(mainGrid);
                return;
            }
            int startIndex = (index * 2) - 2;
            //var ans = answerList.answersList.ToList();
            if (startIndex < 8)
            {
                answerList.answersList[startIndex].answer = null;
                answerList.answersList[startIndex].renderRemoveButton = false;
                answerList.answersList[startIndex].defaultVisible = false;
                if (answerList.answersList[startIndex].childIdList != null && answerList.answersList[startIndex].childIdList.Count() > 0)
                    answerList.answersList[startIndex].childIdList = null;
                answerList.answersList[startIndex + 1].answer = null;
                answerList.answersList[startIndex + 1].renderRemoveButton = false;
                answerList.answersList[startIndex + 1].defaultVisible = false;
                if (answerList.answersList[startIndex + 1].childIdList != null && answerList.answersList[startIndex + 1].childIdList.Count() > 0)
                    answerList.answersList[startIndex + 1].childIdList = null;
                answerList.obsevationNumber = answerList.obsevationNumber - 1;
                for (int i = index; i < 5; i++)
                {
                    var sInd = (i * 2) - 2;
                    answerList.answersList[sInd].answer = answerList.answersList[sInd + 2].answer;

                    answerList.answersList[sInd + 1].answer = answerList.answersList[sInd + 3].answer;
                    if (answerList.answersList[sInd + 2].renderRemoveButton || answerList.answersList[sInd + 3].renderRemoveButton)
                    {
                        answerList.answersList[sInd].renderRemoveButton = true;
                        answerList.answersList[sInd].defaultVisible = true;
                        answerList.answersList[sInd + 1].renderRemoveButton = true;
                        answerList.answersList[sInd + 1].defaultVisible = true;
                    }
                    answerList.answersList[sInd + 2].answer = null;
                    answerList.answersList[sInd + 3].answer = null;
                    answerList.answersList[sInd + 2].renderRemoveButton = false;
                    answerList.answersList[sInd + 2].defaultVisible = false;
                    answerList.answersList[sInd + 3].renderRemoveButton = false;
                    answerList.answersList[sInd + 3].defaultVisible = false;
                }
            }
            else if (startIndex == 8)
            {
                answerList.answersList[startIndex].answer = null;
                answerList.answersList[startIndex].renderRemoveButton = false;
                answerList.answersList[startIndex].defaultVisible = false;
                answerList.answersList[startIndex + 1].answer = null;
                answerList.answersList[startIndex + 1].renderRemoveButton = false;
                answerList.answersList[startIndex + 1].defaultVisible = false;
                answerList.obsevationNumber = answerList.obsevationNumber - 1;
            }

            //ans.RemoveAt(startIndex);
            //ans.RemoveAt(startIndex+1);
            //ans.Add(new SectionHelp.Answerslist());
            //ans.Add(new SectionHelp.Answerslist());
            //answerList.answersList = null;

            //answerList.answersList = ans.ToArray();
            //
            // RemoveBlankObservations();
            //AdjustObservations(answerList, index * 2);
            getJson(mainGrid);
        }
        #region ControlMethods
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBx = sender as TextBox;
            SetControlValue(txtBx.Text, txtBx.Name, "Text");
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton RbBtn = sender as RadioButton;
            SetControlValue(RbBtn.Content, RbBtn.Name, "Radio");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //throw new NotImplementedException();
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
                    uiComponent.HorizontalAlignment = HorizontalAlignment.Left;
                else if (column == 2)
                    uiComponent.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetColumnSpan(uiComponent, column);

                Grid.SetColumn(uiComponent, column);
            }
            //addBlankLine(mainGrid, rowIndex);
        }

        private void addErrorLabelControl(Grid mainGrid, string Message, int rowIndex, string Question)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            TextBlock ErrorTextBlock = new TextBlock();
            ErrorTextBlock.Text = Message;
            ErrorTextBlock.TextWrapping = TextWrapping.Wrap;
            ErrorTextBlock.Foreground = new SolidColorBrush(Colors.Red);
            ErrorTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            if (Question.Equals("State"))
                StateErrorBlock = ErrorTextBlock;
            if (Question.Equals("Hospital"))
                HospitalErrorBlock = ErrorTextBlock;
            if (Question.Equals("CCN"))
                HospitalCnBlock = ErrorTextBlock;
            if (Question.Equals("From"))
                FromErrorBlock = ErrorTextBlock;
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            mainGrid.RowDefinitions.Add(row);
            mainGrid.Children.Add(ErrorTextBlock);
            Grid.SetColumnSpan(ErrorTextBlock, 1);
            Grid.SetRow(ErrorTextBlock, rowIndex);
            Grid.SetColumn(ErrorTextBlock, 1);

        }
        private void addCitationLinkControl(Grid mainGrid, FrameworkElement uiComponent, int rowIndex)
        {
            if (isReview)
                return;
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(0, GridUnitType.Auto);
            mainGrid.RowDefinitions.Add(row);
            uiComponent.HorizontalAlignment = HorizontalAlignment.Center;
            mainGrid.Children.Add(uiComponent);
            Grid.SetColumnSpan(uiComponent, 1);
            Grid.SetRow(uiComponent, rowIndex);
            Grid.SetColumn(uiComponent, 1);
            //addBlankLine(mainGrid, rowIndex);
        }
        #endregion

        #region SaveAndProcess
        private async void processNext(object sender, RoutedEventArgs e)
        {
            if (!isEnabled)
            {
                if (sectionIndex < result.sections.Length - 1)
                {
                    sectionIndex++;
                    getJson(mainGrid);
                }
                return;

            }
            if (sectionIndex == 0)
            {
                if (!Validate())
                {
                    // ShowMessage("Name,Hospital Name and CMS certification number are mandatory.", "Error");
                    return;
                }
            }

            if (sectionIndex < result.sections.Length - 1)
            {
                if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
                {
                    await SaveAndMoveForward();
                }
                else //if (LoadedOffline)
                {
                    await SaveAndMoveForward();

                }
                //else
                //{
                //    ShowMessage("Application is not online to proceed further. You will be redirected to the Mainpage", "Error");
                //    NavigateToMainPage();
                //}
            }

        }

        private async Task SaveAndMoveForward()
        {
            CheckAllAnswers();
            if (UnAnsweredList.Count > 0)
            {
                Pages.ErrorsWarningsDialog dialog = new Pages.ErrorsWarningsDialog(UnAnsweredList);
                dialog.SurveyKey = SurveyKey;
                dialog.NavigateToMainPage += Dialog_NavigatetoMainPage;
                var res = await dialog.ShowAsync();
                if (dialog.Result == Result.Cancel)
                    return;
            }
            await SaveSurvey();
            sectionIndex++;
            getJson(mainGrid, true);
        }
        private void CheckAllAnswers()
        {
            UnAnsweredList = new List<string>();
            if (sectionIndex == 0)
                return;
            foreach (var SurQuestion in result.sections[sectionIndex].surveyQuestionAnswerList)
            {

                if (SurQuestion.answersList != null && SurQuestion.answersList.Count() > 0)
                {
                    if (SurQuestion.answersList[0].answer == null && SurQuestion.renderQuestion)
                    {
                        if (SurQuestion.answersList[0].answersList != null && SurQuestion.answersList[0].answersList.Count() > 0)
                        {
                            if (SurQuestion.answersList[0].answersList[0] != null)
                                continue;
                        }
                        int num;
                        if (Int32.TryParse(SurQuestion.questionText.Substring(0, 1), out num))
                            UnAnsweredList.Add(SurQuestion.questionText.Substring(0, SurQuestion.questionText.IndexOf(" ") + 1));
                        else
                            UnAnsweredList.Add(SurQuestion.questionText);
                    }
                }
                //    if (SurQuestion.answer==null)
                //{
                //    UnAnsweredList.Add(SurQuestion.questionText);
                //}


            }
        }
        private void processPrevious(object sender, RoutedEventArgs e)
        {
            if (sectionIndex > 0)
            {
                this.NextButton.Content = "Next";
                sectionIndex--;
                getJson(mainGrid);

            }
        }

        #endregion
        private void SetControlValue(object ControlValue, string ControlID, string ControlType)
        {
            SectionHelp.Section section = result.sections[sectionIndex];
            bool found = false;
            DateTime dt = DateTime.MinValue;
            foreach (SectionHelp.Surveyquestionanswerlist question in section.surveyQuestionAnswerList)
            {
                if (found) break;
                if (question.answersList != null && question.answersList.Length > 0)
                {
                    foreach (SectionHelp.Answerslist answer in question.answersList)
                    {
                        if (answer.htmlControlId.ToString().Equals(ControlID))
                        {
                            found = true;
                            var val = Convert.ToString(ControlValue);
                            switch (ControlType)
                            {
                                case "Date":
                                    if (DateTime.TryParse(val, out dt))
                                        answer.answerDate = dt;
                                    answer.dateString = val;
                                    break;
                                case "Radio":
                                    answer.answer = (val);
                                    break;
                                case "Text":

                                    var stateVal = GetStateCode(val);
                                    if (!string.IsNullOrEmpty(stateVal))
                                    {
                                        SelectedState = val;
                                        val = stateVal;
                                    }

                                    answer.answer = val;
                                    break;
                                case "checkbox":
                                    var ansList = new List<Object>();
                                    if (Constants.CheckBoxAnswerDictionary.Keys.Contains(answer.htmlControlId))
                                    {
                                        var chStat = Convert.ToBoolean(ControlValue);
                                        if (chStat)
                                        {
                                            string ctrlVal = Constants.CheckBoxAnswerDictionary[answer.htmlControlId];
                                            ansList.Add(ctrlVal);
                                            // ansList.Add(ans);
                                            answer.answersList = ansList.ToArray();
                                        }
                                        else
                                        {
                                            answer.answersList = ansList.ToArray();
                                        }
                                    }
                                    break;
                            }

                            //if (DateTime.TryParse(val, out dt))
                            //    answer.answerDate = dt;
                            //else
                            //answer.answer = (!string.IsNullOrEmpty(GetStateCode(val))) ? (GetStateCode(val)) : val;

                            //break;
                            break;

                        }
                    }
                }

            }
        }

        private void SetRadioState(string RadioText, string HTMLControlValue, RadioButton RdButton)
        {
            if (String.IsNullOrEmpty(HTMLControlValue))
                return;
            if (HTMLControlValue.Equals("Yes"))
            {
                if (RadioText.Equals("Yes"))
                {
                    RdButton.IsChecked = true;
                }

            }
            else if (HTMLControlValue.Equals("No"))
            {
                if (RadioText.Equals("No"))
                    RdButton.IsChecked = true;
            }
            else if (HTMLControlValue.Equals("Unable to observe"))
            {
                if (RadioText.Equals("Unable to observe"))
                    RdButton.IsChecked = true;
            }
            else if (HTMLControlValue.Equals("N/A"))
            {
                if (RadioText.Equals("N/A"))
                    RdButton.IsChecked = true;
            }
        }

        private List<string> GetStates()
        {
            Services.ServiceHelper serviceHelper = Services.ServiceHelper.ServiceHelperObject;
            List<State> States = serviceHelper.StateCode;
            if (States == null || States.Count == 0)
            {
                serviceHelper.GetStates();
                States = serviceHelper.StateCode;
            }
            return States.Select(e => e.stateName).ToList();
        }
        private string GetStateFromCode(string Code)
        {
            Services.ServiceHelper serviceHelper = Services.ServiceHelper.ServiceHelperObject;
            List<State> States = serviceHelper.StateCode;
            if (States == null || States.Count == 0)
                return null;
            var state = States.Where(e => e.stateCode.Equals(Code)).Select(e => e.stateName).FirstOrDefault();
            return state;
        }
        private string GetStateCode(string StateName)
        {
            Services.ServiceHelper serviceHelper = Services.ServiceHelper.ServiceHelperObject;
            List<State> States = serviceHelper.StateCode;
            if (States == null || States.Count == 0)
                return null;
            return States.Where(e => e.stateName.Equals(StateName)).Select(e => e.stateCode).FirstOrDefault();
        }
        private async Task<List<string>> GetHospitals(string StateName)
        {
            Services.ServiceHelper serviceHelper = Services.ServiceHelper.ServiceHelperObject;
            var off = await serviceHelper.IsOffline();
            if (!off && !LoadedOffline)
            {
                string StateCode = serviceHelper.GetCodeforState(StateName);
                //SelectedHospitals = await serviceHelper.GetHospitalsForState(StateCode);//Modified by Sunil since DB has all the hospitals
                SelectedHospitals = serviceHelper.GetHospitalForStateOffline(StateCode);
                if (SelectedHospitals == null || SelectedHospitals.Count == 0)
                    return null;
                _Hospitals = SelectedHospitals.Select(e => e.facilityName).ToList();
                return _Hospitals;
            }
            else if (LoadedOffline && off)
            {
                string StateCode = serviceHelper.GetCodeforState(StateName);
                SelectedHospitals = serviceHelper.GetHospitalForStateOffline(StateCode);
                if (SelectedHospitals == null || SelectedHospitals.Count == 0)
                    return null;
                _Hospitals = SelectedHospitals.Select(e => e.facilityName).ToList();
                return _Hospitals;
            }
            else
            {
                ShowMessage("Application went offline. You will be redirected to the Main page", "Error");
                NavigateToMainPage("Error");
                return null;

            }
        }
        private void AddControlByType(SectionHelp.Answerslist answer, Grid grid, ref int rowIndex, SectionHelp.Surveyquestionanswerlist Question, int ansIndex)
        {
            string QuestionText = Question.questionText;
            if (!answer.defaultVisible)
                return;
            #region Control Switch
            switch (answer.htmlControlType)
            {
                case "textarea":
                    if (Question.renderAddObservation)
                    {

                        TextBlock txBlock = new TextBlock();

                        txBlock.Text = "Observation " + QuestionObservationDictionary[Question.questionId].ToString();
                        if (!isReview)
                            addUIControl(mainGrid, txBlock, rowIndex - 1);

                    }
                    TextBox textBox = new TextBox();
                    textBox.Name = answer.htmlControlId.ToString();
                    textBox.Height = 150;
                    textBox.TextWrapping = TextWrapping.Wrap;
                    textBox.AcceptsReturn = true;
                    if (sectionIndex == 0 && string.IsNullOrEmpty(Convert.ToString(answer.answer)))
                    {
                        if (answer.differentUserAnswerList == null || answer.differentUserAnswerList.Count == 0)
                        {
                            textBox.Text = "";
                        }
                        else
                        {
                            var ans = answer.differentUserAnswerList.OrderByDescending(e => e.answerDate).FirstOrDefault();
                            textBox.Text = ans.answer;
                        }
                    }
                    else
                        textBox.Text = string.IsNullOrEmpty(Convert.ToString(answer.answer)) ? "" : (Convert.ToString(answer.answer));

                    if (result.sections.Count() - 1 == sectionIndex)
                    {
                        if (Question.questionId == -3)
                        {
                            textBox.IsReadOnly = true;
                        }
                    }
                    textBox.TextChanged += TextBox_TextChanged1;
                    if (Question.renderAddObservation && answer.renderRemoveButton)
                    {
                        int ind = QuestionObservationDictionary[Question.questionId];
                        AddRemoveObservationButton(mainGrid, rowIndex - 1, Question, ind);
                        QuestionObservationDictionary[Question.questionId] = ind + 1;
                    }
                    textBox.IsEnabled = isEnabled;
                    if (!isReview)
                        addUIControl(grid, textBox, rowIndex++);
                    break;
                case "radio":
                    foreach (SectionHelp.Htmloption option in answer.htmlOptions)
                    {
                        RadioButton radio = new RadioButton();
                        radio.Name = answer.htmlControlId.ToString();
                        radio.Content = option.value;
                        if (radio.Content.ToString().Equals("Unable to observe"))
                        {
                            CantObserveKey.Add(Question.questionId, answer.htmlControlId);
                        }
                        SetRadioState(radio.Content.ToString(), (Convert.ToString(answer.answer)), radio);
                        //radio.IsChecked = Convert.ToBoolean(answer.htmlControlText);
                        //radio.Checked += Radio_Checked;
                        radio.IsEnabled = isEnabled;
                        radio.GroupName = groupIndex.ToString();
                        radio.Checked += RadioControlChecked;
                        if (!isReview)
                            addUIControl(grid, radio, rowIndex++);

                    }

                    groupIndex++;
                    break;
                case "checkbox":
                    isInCheckBoxState = true;
                    CheckBox chkBox = new CheckBox();
                    chkBox.Name = answer.htmlControlId.ToString();
                    string label = string.Empty;
                    if (answer.htmlOptions.Count() > 0)
                    {
                        label = answer.htmlOptions.First().value;
                    }
                    Boolean chkVal = false;
                    if (answer.answersList != null && answer.answersList.Count() > 0)
                    {
                        var an = answer.answersList[0];
                        if (Constants.CheckBoxAnswerDictionary.Keys.Contains(answer.htmlControlId))
                        {
                            if (Constants.CheckBoxAnswerDictionary[answer.htmlControlId].Equals(an))
                            {
                                chkVal = true;//Convert.ToBoolean(an);
                            }
                        }
                    }

                    chkBox.IsChecked = chkVal;
                    if (!isReview && chkVal)
                        UpdatedCheckBoxes.Add(chkBox);
                    chkBox.Content = label;
                    chkBox.Unchecked += ChkBox_Unchecked;
                    chkBox.Checked += ChkBox_Checked;
                    chkBox.IsEnabled = isEnabled;
                    if (!isReview)
                        addUIControl(grid, chkBox, rowIndex++);
                    isInCheckBoxState = false;
                    break;
                case "select":
                    CMS_Survey.Helpers.CMSCombobox cmbbox = new Helpers.CMSCombobox();

                    cmbbox.Name = answer.htmlControlId.ToString();
                    cmbbox.Width = 200;
                    var val = Convert.ToString(answer.answer);
                    if (QuestionText.Equals("State"))
                    {
                        stateControl = cmbbox;
                        cmbbox.SelectionChanged += CmbBxStateValueChanged;
                        GetStates().ForEach(e => cmbbox.Items.Add(e));
                        if (LoadedOffline)
                        {
                            SelectedState = Services.ServiceHelper.ServiceHelperObject.GetOfflineSelectedState(val);  //Sunil
                            if (sectionIndex == 0 && string.IsNullOrEmpty(val))
                            {
                                if (answer.differentUserAnswerList == null || answer.differentUserAnswerList.Count == 0)
                                {

                                }
                                else
                                {
                                    var ans = answer.differentUserAnswerList.OrderByDescending(e => e.answerDate).FirstOrDefault();
                                    SelectedState = Services.ServiceHelper.ServiceHelperObject.GetOfflineSelectedState(ans.answer);
                                    cmbbox.IsEnabled = false;
                                }
                            }
                            cmbbox.SelectedValue = SelectedState;
                            //cmbbox.IsEnabled = false;
                        }
                        else
                        {
                            cmbbox.IsEnabled = true;
                            SelectedState = GetStateFromCode(val);
                            if (sectionIndex == 0 && string.IsNullOrEmpty(val))
                            {
                                if (answer.differentUserAnswerList == null || answer.differentUserAnswerList.Count == 0)
                                {

                                }
                                else
                                {
                                    var ans = answer.differentUserAnswerList.OrderByDescending(e => e.answerDate).FirstOrDefault();
                                    SelectedState = Services.ServiceHelper.ServiceHelperObject.GetOfflineSelectedState(ans.answer);
                                    cmbbox.IsEnabled = false;
                                }
                            }
                            cmbbox.SelectedValue = SelectedState;
                        }
                        addErrorLabelControl(grid, "Please select a state", rowIndex, "State");
                    }
                    else if (QuestionText.Equals("Hospital Name"))
                    {
                        HospitalControl = cmbbox;
                        HospitalControlName = cmbbox.Name;
                        cmbbox.DropDownOpened += Hospital_comboBoxOpened;
                        cmbbox.SelectionChanged += CmbBxHospitalSelected;
                        if (!string.IsNullOrEmpty(val))
                        {

                            SetHospital(cmbbox, val);
                        }
                        else if (sectionIndex == 0)
                        {
                            if (answer.differentUserAnswerList == null || answer.differentUserAnswerList.Count == 0)
                            {

                            }
                            else
                            {
                                var ans = answer.differentUserAnswerList.OrderByDescending(e => e.answerDate).FirstOrDefault();
                                if (!string.IsNullOrEmpty(ans.answer))
                                    SetHospital(cmbbox, ans.answer);
                                cmbbox.IsEnabled = false;
                            }
                        }
                        addErrorLabelControl(grid, "Please select a Hospital", rowIndex, "Hospital");
                    }
                    cmbbox.IsEnabled = isEnabled;
                    if (QuestionText == "Hospital Name")
                    {
                        if (isEnabled)
                            cmbbox.IsEnabled = isUserOwner;
                    }
                    if (answer.differentUserAnswerList != null && answer.differentUserAnswerList.Count > 0)
                        cmbbox.IsEnabled = false;
                    //cmbbox.SelectedValue = string.IsNullOrEmpty((Convert.ToString(answer.answer))) ? "" : (Convert.ToString(answer.answer));
                    if (!isReview)
                        addUIControl(grid, cmbbox, rowIndex++);
                    break;

                case "number":
                    TextBox txtBx = new TextBox();
                    txtBx.Name = answer.htmlControlId.ToString();
                    //txtBx.Height = 150;
                    txtBx.HorizontalAlignment = HorizontalAlignment.Left;
                    if (QuestionText.Equals("CMS Certification Number"))
                    {
                        CcnControlName = txtBx.Name;
                        Hospitalcn = txtBx;
                        if (!string.IsNullOrEmpty(FetchedHospitalCcn))
                        {
                            txtBx.IsReadOnly = false;
                            txtBx.Text = FetchedHospitalCcn;
                            txtBx.IsReadOnly = true;
                        }
                        //txtBx.IsReadOnly = true;

                    }
                    txtBx.Width = 200;
                    txtBx.Text = string.IsNullOrEmpty(Convert.ToString(answer.answer)) ? "" : (Convert.ToString(answer.answer));
                    txtBx.LostFocus += TextBox_LostFocus;
                    txtBx.IsEnabled = isEnabled;
                    addErrorLabelControl(grid, "Please select a Certificate number", rowIndex, "CCN");
                    if (!isReview)
                        addUIControl(grid, txtBx, rowIndex++);
                    break;
                case "date":
                    DatePicker dtPicker = new DatePicker();
                    dtPicker.Name = answer.htmlControlId.ToString();
                    dtPicker.DateChanged += DtPicker_DateChanged;
                    dtPicker.Width = 150;
                    DateTime dttime = DateTime.Now;
                    if (sectionIndex == 0 && string.IsNullOrEmpty(Convert.ToString(answer.dateString)))
                    {
                        if (answer.differentUserAnswerList == null || answer.differentUserAnswerList.Count == 0)
                        {
                            dtPicker.Date = DateTime.Now;
                            DtPicker_DateChanged(dtPicker, null);
                        }
                        else
                        {
                            var ans = answer.differentUserAnswerList.OrderByDescending(e => e.answerDate).FirstOrDefault();
                            if (!string.IsNullOrEmpty(answer.dateString))
                            {
                                dtPicker.Date = (DateTime.TryParse(Convert.ToString(answer.dateString), out dttime)) ? dttime : DateTime.Now;
                                DtPicker_DateChanged(dtPicker, null);
                            }
                        }
                    }
                    else
                    {
                        dtPicker.Date = (DateTime.TryParse(Convert.ToString(answer.dateString), out dttime)) ? dttime : DateTime.Now;
                        DtPicker_DateChanged(dtPicker, null);
                    }
                    TextBlock txtBlock = new TextBlock();
                    if (answer.htmlControlText.Equals("From"))
                    {
                        fromTimePicker = dtPicker;
                        txtBlock.Text = "From";
                        addErrorLabelControl(grid, "From date cannot be greater than To date", rowIndex, "From");
                    }
                    else if (answer.htmlControlText.Equals("To"))
                    {
                        toTimePicker = dtPicker;
                        txtBlock.Text = "To";
                    }
                    dtPicker.IsEnabled = isEnabled;
                    txtBlock.TextWrapping = TextWrapping.Wrap;
                    if (!isReview)
                    {
                        addUIControl(grid, txtBlock, rowIndex++);
                        addUIControl(grid, dtPicker, rowIndex++);
                    }
                    break;

                    //case "file":
                    //    Button selectBtn = new Button();
                    //    selectBtn.Name = "SelectButton";
                    //    selectBtn.Content = "Choose";
                    //    selectBtn.Click += SelectBtn_Click;
                    //    selectBtn.Width = 150;
                    //    addUIControl(grid, selectBtn, rowIndex,1);
                    //    Button uploadBtn = new Button();
                    //    uploadBtn.Name = "UploadBtn";
                    //    uploadBtn.Content = "Upload";
                    //    uploadBtn.Click += UploadBtn_Click;
                    //    uploadBtn.Width = 150;
                    //    addUIControl(grid, uploadBtn, rowIndex++,2);

                    //    break;  
            }
            #endregion
            if (!string.IsNullOrEmpty(Question.citableTagURL))
            {
                if (tempCitiationId.Equals(Question.questionId.ToString()))
                    return;
                else
                    tempCitiationId = Question.questionId.ToString();
                TextBlock tb = new TextBlock();
                Hyperlink hyperlink = new Hyperlink();
                Run run = new Run();
                string linkText = linkText = Question.citableTagName;
                if (!string.IsNullOrEmpty(linkText))
                {
                    if (linkText.Length > 75)
                        linkText = Question.citableTagName.Substring(0, 75);

                }
                run.Text = linkText;
                hyperlink.NavigateUri = new Uri(Services.ServiceHelper.CitationUrl + Question.citableTagURL);
                hyperlink.Inlines.Add(run);
                tb.Inlines.Add(hyperlink);
                // stpnl.Children.Add(tb);
                addCitationLinkControl(mainGrid, tb, rowIndex - 1);
            }
            //if(answer.renderRemoveButton)
            //{
            //    AddRemoveObservationButton(grid, rowIndex, Question);
            //}
        }

        private async void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fileOpenpicker = new FileOpenPicker();
            fileOpenpicker.ViewMode = PickerViewMode.List;
            fileOpenpicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            //fileOpenpicker.FileTypeFilter.Add("*.*");
            fileOpenpicker.FileTypeFilter.Add("*");
            StorageFile file = await fileOpenpicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            }
            else
            {

            }
        }

        private void UploadBtn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TextBox_TextChanged1(object sender, TextChangedEventArgs e)
        {
            TextBox_LostFocus(sender, e);
        }

        private void AddOtherAnswers(SectionHelp.Answerslist answer, SectionHelp.Surveyquestionanswerlist question, ref int rowIndex)

        {
            List<SectionHelp.DifferentUserAnswerList> DifferentAnswers = answer.differentUserAnswerList;

            StackPanel panel = new StackPanel();
            Border myBorder1 = new Border();
            SolidColorBrush myBrush = new SolidColorBrush(Colors.Black);
            myBorder1.BorderBrush = myBrush;
            myBorder1.BorderThickness = new Thickness(6);
            panel.BorderBrush = myBrush;
            foreach (SectionHelp.DifferentUserAnswerList Otheranswer in DifferentAnswers.OrderByDescending(f => f.answerDate))
            {
                TextBlock blnk = new TextBlock();
                blnk.Text = " ";
                if (!isReview)
                {
                    TextBlock txBlock = new TextBlock();

                    txBlock.Text = "User :" + Services.ServiceHelper.ServiceHelperObject.UserKeyDictionary.Where(k => k.Key.Equals(Otheranswer.user)).Select(k => k.Value).FirstOrDefault();
                    //txBlock.Text = "Observation " +(ansIndex-1).ToString();
                    panel.Children.Add(txBlock);

                    //addUIControl(mainGrid, txBlock, rowIndex++);
                    //addBlankLine(mainGrid, rowIndex);
                }
                TextBlock Otherans = new TextBlock();

                Otherans.Text = Otheranswer.answer;
                if (sectionIndex == 0)
                {
                    if (question.questionText == "State")
                    {
                        Otherans.Text = Services.ServiceHelper.ServiceHelperObject.GetStateNameforStateCode(Otheranswer.answer);
                    }
                    if (question.questionText == "Hospital Name")
                    {
                        var hsp = Services.ServiceHelper.ServiceHelperObject.GetHospitalForProviderKey(Convert.ToInt32(Otheranswer.answer));
                        Otherans.Text = hsp.facilityName;


                    }
                }
                panel.Children.Add(Otherans);
                TextBlock blnk2 = new TextBlock();
                blnk.Text = " ";
                panel.Children.Add(blnk2);
                //addUIControl(mainGrid, answer, rowIndex++);
                //addBlankLine(mainGrid, rowIndex);
                if (isReview)
                {
                    break;
                }
            }
            addUIControl(mainGrid, panel, rowIndex++);
        }


        private void AddOtherAnswersPerUser(List<UserObservation> UserObservationList, SectionHelp.Surveyquestionanswerlist question, ref int rowIndex)

        {
            //List<SectionHelp.DifferentUserAnswerList> DifferentAnswers = answer.differentUserAnswerList;
            if (sectionIndex == 0 && !isReview)
                return;
            StackPanel panel = new StackPanel();
            Border myBorder1 = new Border();
            SolidColorBrush myBrush = new SolidColorBrush(Colors.Black);
            myBorder1.BorderBrush = myBrush;
            myBorder1.BorderThickness = new Thickness(6);
            panel.BorderBrush = myBrush;
            UserObservationList = UserObservationList.OrderByDescending(e => e.AnswerDate).ToList();
            var usrObsList = UserObservationList.GroupBy(e => e.UserKey);
            int i = 1;

            foreach (var usrOb in usrObsList)
            {
                if (!isReview)
                {
                    TextBlock txBlock = new TextBlock();

                    txBlock.Text = "User :" + Services.ServiceHelper.ServiceHelperObject.UserKeyDictionary.Where(k => k.Key.Equals(usrOb.Key)).Select(k => k.Value).FirstOrDefault();
                    //txBlock.Text = "Observation " +(ansIndex-1).ToString();
                    panel.Children.Add(txBlock);
                }
                int olObdNumber = -1;
                int ans = 1;
                if (ans == 3)
                    ans = 1;
                foreach (UserObservation Otheranswer in usrOb.OrderBy(e => e.ObservationNumber))
                {

                    if (olObdNumber != Otheranswer.ObservationNumber && sectionIndex != 0 && question.renderAddObservation)
                    {

                        olObdNumber = Otheranswer.ObservationNumber;
                        TextBlock blnk = new TextBlock();
                        var obsnumber = olObdNumber + 1;
                        blnk.Text = "Observation : " + olObdNumber;
                        panel.Children.Add(blnk);
                    }



                    //addUIControl(mainGrid, txBlock, rowIndex++);
                    //addBlankLine(mainGrid, rowIndex);

                    TextBlock Otherans = new TextBlock();

                    Otherans.Text = Otheranswer.Answer;
                    Otherans.IsTextSelectionEnabled = true;
                    if (sectionIndex == 0)
                    {
                        if (question.questionText == "State")
                        {
                            Otherans.Text = Services.ServiceHelper.ServiceHelperObject.GetStateNameforStateCode(Otheranswer.Answer);
                        }
                        if (question.questionText == "Hospital Name")
                        {
                            var hsp = Services.ServiceHelper.ServiceHelperObject.GetHospitalForProviderKey(Convert.ToInt32(Otheranswer.Answer));
                            Otherans.Text = hsp.facilityName;


                        }
                        if (question.questionText == "Date of site visit")
                        {

                            if (isReview)
                            {
                                TextBlock txtBlock = new TextBlock();
                                if (ans == 1)
                                    txtBlock.Text = "From";
                                if (ans == 2)
                                    txtBlock.Text = "To";
                                panel.Children.Add(txtBlock);
                            }
                            ans++;
                        }
                    }
                    panel.Children.Add(Otherans);

                    //addUIControl(mainGrid, answer, rowIndex++);
                    //addBlankLine(mainGrid, rowIndex);

                }
                if (isReview)
                    break;
                i++;
            }
            TextBlock blnk2 = new TextBlock();
            blnk2.Text = " ";
            panel.Children.Add(blnk2);
            addUIControl(mainGrid, panel, rowIndex++);
        }

        private async void RadioControlChecked(object sender, RoutedEventArgs e)
        {
            RadioButton radio = sender as RadioButton;
            SetControlValue(radio.Content, radio.Name, "Radio");
            if (sectionIndex <= 4)
                return;
            MessageDialog msgDialog;
            if (radio == null)
                return;
            var controlId = Convert.ToInt32(radio.Name);
            //var Questiion = result.sections[sectionIndex].surveyQuestionAnswerList.Select(t => t.answersList.Where(z => z.htmlControlId.Equals(controlId)));
            bool found = false;
            int questionID = -1;
            foreach (var item in result.sections[sectionIndex].surveyQuestionAnswerList)
            {
                if (found)
                    break;
                foreach (var ans in item.answersList)
                {
                    if (ans.htmlControlId == controlId)
                    {
                        questionID = item.questionId;
                        found = true;
                        break;
                    }

                }
            }


            if (CantObserveKey != null && CantObserveKey.Count > 0)
                questionID = CantObserveKey.Where(t => t.Value.Equals(controlId)).Select(t => t.Key).FirstOrDefault();

            var Questiion = result.sections[sectionIndex].surveyQuestionAnswerList.Where(t => t.questionId.Equals(questionID)).Select(t => t).FirstOrDefault();
            if (Questiion == null)
                return;
            var firstRadioAnswer = Questiion.answersList[0].answer;
            if (radio.Content.ToString() == "Unable to observe")
            {
                // if (Questiion.obsevationNumber > 2)
                //{
                await ButtonShowMessageDialog_Click(null, null);
                if (Convert.ToInt32(btnResult.Id) == 0)
                {
                    Questiion.answersList[0].answer = "Unable to observe";
                    for (int i = 2; i < 10; i++)
                    {
                        Questiion.answersList[i].answer = null;
                        Questiion.answersList[i].defaultVisible = false;

                    }

                    Questiion.obsevationNumber = 2;

                }

                else
                {
                    radio.IsChecked = false;
                    return;
                }
                Questiion.disableAddObservation = true;
                Questiion.renderAddObservation = true;
                getJson(mainGrid);
                //}
            }
            //if(firstRadioAnswer.Equals("Unable to observe"))
            else
            {
                radio.IsChecked = true;
                Questiion.disableAddObservation = false;
                Questiion.renderAddObservation = true;
                Questiion.answersList[0].renderRemoveButton = true;
                Questiion.answersList[0].defaultVisible = true;
                Questiion.answersList[1].renderRemoveButton = true;
                Questiion.answersList[1].defaultVisible = true;
                getJson(mainGrid);

            }

        }
        private async Task ButtonShowMessageDialog_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Windows.UI.Popups.MessageDialog("Selecting Unable to observe will remove any existing observation 2-5 ");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Continue") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Cancel") { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            btnResult = await dialog.ShowAsync();

            //var btn = sender as Button;
            //tn.Content = $"Result: {result.Label} ({result.Id})";
        }
        private async Task ButtonCheckBoxShowMessageDialog_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new Windows.UI.Popups.MessageDialog("Caution: clicking yes for skip logic after inputting data in subsequent fields initially will now wipe all subsequent data.");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Continue") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Cancel") { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            btnResult = await dialog.ShowAsync();

            //var btn = sender as Button;
            //tn.Content = $"Result: {result.Label} ({result.Id})";
        }
        private void ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBoxStateChanged(sender, false);

        }

        private void ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBoxStateChanged(sender, true);
        }

        private async void CheckBoxStateChanged(object sender, bool? checkedState, bool isSetCheckbox = false)
        {


            CheckBox chkB = sender as CheckBox;
            int ChckBoxID = Convert.ToInt32(chkB.Name);
            int QuestionID = -1;
            bool found = false;
            bool chkState = (checkedState == null) ? false : Convert.ToBoolean(checkedState);

            foreach (var SurQuestionList in result.sections[sectionIndex].surveyQuestionAnswerList)
            {
                int i = 0;
                if (found)
                    break;

                foreach (var item in SurQuestionList.answersList)
                {
                    i++;
                    if (item.htmlControlId.Equals(ChckBoxID))
                    {
                        // item.answer = !checkedState;
                        //result.sections[sectionIndex].surveyQuestionAnswerList[i].answersList.Where(f => f.htmlControlId.Equals(ChckBoxID)).FirstOrDefault().answer = !checkedState;
                        if (!isSetCheckbox)
                        {
                            SetControlValue(!checkedState, Convert.ToString(item.htmlControlId), item.htmlControlType);
                        }
                        QuestionID = SurQuestionList.questionId;
                        found = true;
                        break;
                    }
                }

            }
            if (!isInCheckBoxState)
            {
                if (!chkState)
                {
                    await ButtonCheckBoxShowMessageDialog_Click(null, null);
                    if (Convert.ToInt32(btnResult.Id) == 1)
                    {

                        chkB.IsChecked = false;
                        isInCheckBoxState = false;
                        return;
                    }
                }
            }
            isInCheckBoxState = true;
            if (!Constants.QuestionSectionDictionary.ContainsKey(QuestionID))
                return;

            List<int> ControlID = Constants.QuestionSectionDictionary[QuestionID];
            foreach (var SurQuestionList in result.sections[sectionIndex].surveyQuestionAnswerList)
            {

                if (ControlID.Contains(SurQuestionList.questionId))
                {
                    int i = 1;
                    //if (SurQuestionList.answersList != null && SurQuestionList.htmlControlType != "checkbox")
                    //{
                    foreach (var item in SurQuestionList.answersList)
                    {
                        if (item.htmlControlType == "checkbox")
                            break;
                        if (i > 2 && chkState)
                            break;
                        item.answersList = null;
                        item.answer = null;
                        item.defaultVisible = chkState;
                        item.renderRemoveButton = chkState;
                        i++;
                    }
                    if (SurQuestionList.answersList != null && SurQuestionList.answersList.Count() > 0 && SurQuestionList.answersList[0].htmlControlType != "checkbox")
                    {
                        SurQuestionList.renderAddObservation = chkState;
                        SurQuestionList.obsevationNumber = 2;
                    }
                    SurQuestionList.renderQuestion = chkState;
                }

                //}

            }
            isInCheckBoxState = false;
            if (!isSetCheckbox)
                getJson(mainGrid);
        }

        private void AddObservationButtonClick(object sender, RoutedEventArgs e)
        {
            Button bn = sender as Button;
            var name = bn.Name.Split('_').First();
            var answerList = result.sections[sectionIndex].surveyQuestionAnswerList.Where(t => t.questionId.ToString().Equals(name)).Select(t => t).FirstOrDefault();

            if (answerList == null && answerList.obsevationNumber > 5)
                return;
            int startIndex = (answerList.obsevationNumber * 2) - 2;
            answerList.answersList[startIndex].defaultVisible = true;
            answerList.answersList[startIndex].renderRemoveButton = true;
            answerList.answersList[startIndex + 1].renderRemoveButton = true;
            answerList.answersList[startIndex + 1].defaultVisible = true;
            answerList.obsevationNumber = answerList.obsevationNumber + 1;
            getJson(mainGrid);

            //bn.Visibility = Visibility.Collapsed;
        }

        private void AddObservation(Grid grid, ref int rowIndex, SectionHelp.Surveyquestionanswerlist question)
        {
            if (question.renderAddObservation)
            {

            }
        }
        private void Hospital_comboBoxOpened(object sender, object e)
        {
            ComboBox provCmbbx = sender as ComboBox;
            if (Services.ServiceHelper._OfflineMode)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(provCmbbx.SelectedValue)))
                    return;
                provCmbbx.Items.Clear();
                var provs = Services.ServiceHelper.ServiceHelperObject.GetHospitalForStateOffline("");
                provs.ForEach(pr => provCmbbx.Items.Add(pr.facilityName));
            }
        }

        private async void SetHospital(ComboBox cmbx, string hospitalCode)
        {
            cmbx.Items.Clear();
            if (_Hospitals == null)
                await GetHospitals(SelectedState);
            if (_Hospitals == null)
                return;
            _Hospitals.ForEach(e => cmbx.Items.Add(e));
            var hostpitalName = SelectedHospitals.Where(hsp => hsp.providerKey.ToString().Equals(hospitalCode)).Select(hsp => hsp.facilityName).FirstOrDefault();
            if (_Hospitals.Contains(hostpitalName))
                cmbx.SelectedValue = hostpitalName;
        }
        private void DtPicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DatePicker DtPicker = sender as DatePicker;
            if (DtPicker == null)
                return;
            SetControlValue(DtPicker.Date.ToString(), DtPicker.Name, "Date");

        }

        private void CmbBxHospitalSelected(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ComboboxHospital" + DateTime.Now.ToString("hh.mm.ss.ffffff"));
            ComboBox cmbBx = sender as ComboBox;
            if (cmbBx.SelectedValue == null)
                return;
            string SelectedHospital = cmbBx.SelectedValue.ToString();
            Services.ServiceHelper serviceHelper = Services.ServiceHelper.ServiceHelperObject;
            FetchedHospitalCcn = SelectedHospitals.Where(hsp => hsp.facilityName.Equals(SelectedHospital)).Select(hsp => hsp.ccn).FirstOrDefault();
            var HospitalCode = SelectedHospitals.Where(hsp => hsp.facilityName.Equals(SelectedHospital)).Select(hsp => hsp.providerKey).FirstOrDefault();
            ccnTextBox = FindVisualChildren<TextBox>(this.mainGrid).ToList().Where(tb => tb.Name.Equals(CcnControlName)).FirstOrDefault();
            SetControlValue(HospitalCode, cmbBx.Name, "Text");
            if (ccnTextBox != null)
            {
                ccnTextBox.IsReadOnly = false;
                ccnTextBox.Text = (string.IsNullOrEmpty(FetchedHospitalCcn)) ? "" : FetchedHospitalCcn;
                ccnTextBox.IsReadOnly = true;
                SetControlValue(ccnTextBox.Text, ccnTextBox.Name, "Text");
            }

            System.Diagnostics.Debug.WriteLine("ComboBox Hospital" + DateTime.Now.ToString("hh.mm.ss.ffffff"));
        }

        private async void CmbBxStateValueChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Combobox state" + DateTime.Now.ToString("hh.mm.ss.ffffff"));
            ComboBox cmbBx = sender as ComboBox;
            if (cmbBx.SelectedValue == null)
                return;
            SelectedState = cmbBx.SelectedValue.ToString();
            SetControlValue(SelectedState, cmbBx.Name, "Text");
            _Hospitals = await GetHospitals(SelectedState);
            var Hospitalcmbx = FindVisualChildren<ComboBox>(this.mainGrid).ToList().Where(cmb => cmb.Name.Equals(HospitalControlName)).FirstOrDefault();
            if (Hospitalcmbx == null)
                return;
            //cmbx.Items.Clear();
            var val = Hospitalcmbx.SelectedValue;
            //if (ccnTextBox != null)
            //    ccnTextBox.Text = "";

            if (_Hospitals != null && _Hospitals.Count > 0)
            {
                Hospitalcmbx.Items.Clear();
                _Hospitals.Sort();
                _Hospitals.ForEach(hsp => Hospitalcmbx.Items.Add(hsp));
                if (_Hospitals.Contains(val))
                {
                    Hospitalcmbx.SelectedValue = val;
                }
                else
                    Hospitalcmbx.SelectedValue = "";
            }
            else
                Hospitalcmbx.SelectedValue = "";

            System.Diagnostics.Debug.WriteLine("Combobox state" + DateTime.Now.ToString("hh.mm.ss.ffffff"));

        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        private async void ShowMessage(string message, string caption)
        {
            MessageDialog msgDialog = new MessageDialog(message, caption);
            IUICommand cmd = await msgDialog.ShowAsync();
        }

        private void NavigateToMainPage(string Source)
        {
            var param = Template10.Services.SerializationService.SerializationService.Json.Serialize(Source);
            this.Frame.Navigate(typeof(Views.GridMainPage), param);
        }
        public async Task SaveSurvey()
        {
            try
            {
                //CheckAllAnswers();


                ShowProgress();
                RemoveBlankObservations();
                Services.ServiceHelper serviceHelper = Services.ServiceHelper.ServiceHelperObject;
                if (await serviceHelper.IsOffline())
                {
                    var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(result.sections.ToList());
                    await serviceHelper.SaveSurveyLocal(jsonRequest, result.sections.First().surveyKey.ToString(), Constants.SurveyFolder);
                    await serviceHelper.SaveSurveyLocal(jsonRequest, result.sections.First().surveyKey.ToString(), Constants.TempSurveyFolder);
                    await serviceHelper.AddSurveyToList(result.sections.ToList());
                    HideProgress();
                }
                else if (!await serviceHelper.IsOffline())
                {
                    result = await serviceHelper.CallSurveyService(result.sections.ToList());
                    HideProgress();
                }
                else
                {
                    var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(result.sections.ToList());
                    await serviceHelper.SaveSurveyLocal(jsonRequest, result.sections.First().surveyKey.ToString(), Constants.SurveyFolder);
                    HideProgress();
                    ShowMessage("Application went offline. You will be redirected to the Mainpage", "Error");
                    NavigateToMainPage("Error");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Something went wrong. You will be redirected to the Mainpage", "Error");
                NavigateToMainPage("Error");

            }

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

        private async void ProcessSave(object sender, RoutedEventArgs e)
        {
            if (!isEnabled)
                return;
            if (sectionIndex == 0)
            {
                if (!Validate())
                {
                    //ShowMessage("Name,Hospital Name and CMS certification number are mandatory.", "Error");
                    return;
                }
            }
            NextButton.Visibility = Visibility.Collapsed;
            Previous.Visibility = Visibility.Collapsed;
            await SaveSurvey();
            NextButton.Visibility = Visibility.Visible;
            Previous.Visibility = Visibility.Visible;
            getJson(mainGrid);
        }

        private async void CreateJumpSection(List<JumpClass> jumpClass)
        {

            foreach (var Sec in jumpClass)
            {
                StackPanel stkPnl = new StackPanel();
                stkPnl.Orientation = Orientation.Horizontal;
                Button btn = new Button();
                btn.Content = Sec.SubSection;
                if (string.IsNullOrEmpty(Sec.SectionTitle))
                {

                }
                else
                {
                    Flyout fly = new Flyout();
                    StackPanel stp = new StackPanel();
                    Button bn = new Button();
                    bn.Content = Sec.SectionTitle;
                    stp.Children.Add(bn);
                    fly.Content = stp;

                }
            }
            //for(int i=0;i<result.sections.Count();i++)
            //{
            //    StackPanel stkPnl = new StackPanel();
            //    stkPnl.Orientation = Orientation.Horizontal;
            //    Button btn = new Button();
            //    btn.Content = result.sections[i].sectionTitle;
            //    stkPnl.Children.Add(btn);
            //    JumpSectionPanel.Children.Add(stkPnl);
            //}
        }

        private bool Validate()
        {
            bool isvalid = true;
            if (string.IsNullOrEmpty(Convert.ToString(stateControl.SelectedValue)))
            {
                isvalid = false;
                StateErrorBlock.Visibility = Visibility.Visible;
            }
            else
                StateErrorBlock.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(Convert.ToString(HospitalControl.SelectedValue)))
            {
                isvalid = false;
                HospitalErrorBlock.Visibility = Visibility.Visible;
            }
            else
                HospitalErrorBlock.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(Convert.ToString(Hospitalcn.Text)))
            {
                isvalid = false;
                HospitalCnBlock.Visibility = Visibility.Visible;
            }
            else
                HospitalCnBlock.Visibility = Visibility.Collapsed;

            if (fromTimePicker.Date > toTimePicker.Date)
            {
                isvalid = false;
                FromErrorBlock.Visibility = Visibility.Visible;
            }
            else
                FromErrorBlock.Visibility = Visibility.Collapsed;
            return isvalid;

        }
        #region MenuFlyouts

        private void JumpButton_Loading(FrameworkElement sender, object args)
        {

        }

        private void JumpButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void JumpButtonLoad()
        {

            MenuFlyout m = new MenuFlyout();

            foreach (JumpClass Sec in jmpClass)
            {

                if (string.IsNullOrEmpty(Sec.SectionTitle))
                {
                    MenuFlyoutItem mFlyItem = new MenuFlyoutItem();
                    mFlyItem.Background = new SolidColorBrush(Colors.SteelBlue);
                    mFlyItem.Foreground = new SolidColorBrush(Colors.White);
                    mFlyItem.Text = Sec.SubSection;
                    mFlyItem.Name = Sec.SubSection;
                    mFlyItem.Click += MFlyItem_Click;
                    m.Items.Add(mFlyItem);
                }
                else
                {
                    MenuFlyoutSubItem subMItem = (MenuFlyoutSubItem)m.Items.Where(e => e.Name.Equals(Sec.SubSection)).FirstOrDefault();
                    if (subMItem != null)
                    {
                        MenuFlyoutItem mFlyItem = new MenuFlyoutItem();
                        mFlyItem.Background = new SolidColorBrush(Colors.SteelBlue);
                        mFlyItem.Foreground = new SolidColorBrush(Colors.White);
                        mFlyItem.Text = Sec.SectionTitle;
                        mFlyItem.Name = Sec.SectionTitle;
                        mFlyItem.Click += MFlyItem_Click;
                        subMItem.Items.Add(mFlyItem);
                    }
                    else
                    {
                        MenuFlyoutSubItem mnSub = new MenuFlyoutSubItem();
                        mnSub.Background = new SolidColorBrush(Colors.SteelBlue);
                        mnSub.Foreground = new SolidColorBrush(Colors.White);
                        mnSub.Text = Sec.SubSection;
                        mnSub.Name = Sec.SubSection;
                        m.Items.Add(mnSub);
                        MenuFlyoutItem mFlyItem = new MenuFlyoutItem();
                        mFlyItem.Text = Sec.SectionTitle;
                        mFlyItem.Name = Sec.SectionTitle;
                        mFlyItem.Background = new SolidColorBrush(Colors.SteelBlue);
                        mFlyItem.Foreground = new SolidColorBrush(Colors.White);
                        mFlyItem.Click += MFlyItem_Click;
                        mnSub.Items.Add(mFlyItem);
                    }
                }
                //else if (string.IsNullOrEmpty(Sec.SectionTitle))
                //{
                //    MenuFlyoutSubItem mnSub = new MenuFlyoutSubItem();

                //}
                //else
                //{
                //    mn.Text = Sec.SectionTitle;
                //}
                //m.Items.Add(mn);
            }
            //m.ShowAt((FrameworkElement)sender);
            JumpButton.Flyout = m;

        }
        private void HelpButtonLoad()
        {
            MenuFlyout m = new MenuFlyout();
            if (result.help == null)
                return;
            foreach (SectionHelp.Help _help in result.help)
            {
                MenuFlyoutSubItem mFlySub = new MenuFlyoutSubItem();
                mFlySub.Text = _help.helpSectionName;
                foreach (SectionHelp.HelpSectionLink _HelpSectionLink in _help.helpSectionLink)
                {
                    MenuFlyoutItem mFlyItem = new MenuFlyoutItem();
                    mFlyItem.Text = _HelpSectionLink.helpLinkName;
                    mFlyItem.Click += MyFlyItemClicked;
                    mFlySub.Items.Add(mFlyItem);
                }
                m.Items.Add(mFlySub);
            }
            HelpButton.Flyout = m;
        }

        private void MyFlyItemClicked(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem mItem = e.OriginalSource as MenuFlyoutItem;
            var ItemName = mItem.Text;
            bool found = false; ;
            SectionHelp.HelpSectionLink selectedHelp = null;
            foreach (SectionHelp.Help _help in result.help)
            {
                if (found)
                    break;
                foreach (SectionHelp.HelpSectionLink _helpSectionLink in _help.helpSectionLink)
                {

                    if (_helpSectionLink.helpLinkName.Equals(ItemName))
                    {
                        selectedHelp = _helpSectionLink;
                        found = true;
                    }
                }
            }
            Uri url = null;
            if (!Uri.TryCreate(selectedHelp.helpLinkURL, UriKind.Absolute, out url))
            {
                //url =new Uri( Services.ServiceHelper.HostUrl + selectedHelp.helpLinkURL);
            }
            if (url == null)
                return;
            //string url=result.help.Where()
            Windows.System.Launcher.LaunchUriAsync(url);
            //link.NavigateUri = new Uri();

        }

        private async void MFlyItem_Click(object sender, RoutedEventArgs e)
        {
            int indx;
            //if(!isEnabled)
            //{
            //    sectionIndex++;
            //    getJson(mainGrid);
            //}
            MenuFlyoutItem mFlyItem = (MenuFlyoutItem)e.OriginalSource;
            var ClickedName = mFlyItem.Name;

            if (sectionIndex == 0 && !isReview)
            {
                if (!isEnabled)
                {
                    if (!Validate())
                        return;
                }
            }
            var item = jmpClass.Where(t => t.SectionTitle != null).Where(t => t.SectionTitle.Equals(ClickedName)).Select(t => t).FirstOrDefault();
            if (item == null)
            {
                item = jmpClass.Where(t => t.SubSection.Equals(ClickedName)).Select(t => t).FirstOrDefault();
            }

            indx = item.PageIndex;
            if (indx.Equals(sectionIndex))
                return;
            else
            {
                CheckAllAnswers();

                if (isEnabled)
                {
                    if (UnAnsweredList.Count > 0)
                    {
                        Pages.ErrorsWarningsDialog dialog = new Pages.ErrorsWarningsDialog(UnAnsweredList);
                        dialog.SurveyKey = SurveyKey;
                        dialog.NavigateToMainPage += Dialog_NavigatetoMainPage;
                        var res = await dialog.ShowAsync();
                        if (dialog.Result == Result.Cancel)
                            return;
                    }
                    await SaveSurvey();
                }
                sectionIndex = indx;
                getJson(mainGrid, true);
            }

            //e.OriginalSource

            //throw new NotImplementedException();
        }
        #endregion



        private async void CommentsButton_Click(object sender, RoutedEventArgs e)
        {
            Pages.CommentsDialog dialog = new Pages.CommentsDialog(result.sections.LastOrDefault());
            dialog.SurveyKey = SurveyKey;
            dialog.MakeCloseInVisible();
            dialog.NavigateToMainPage += Dialog_NavigatetoMainPage;
            await dialog.ShowAsync();
        }

        private void Dialog_NavigatetoMainPage(object sender, EventArgs e)
        {
            this.NavigateToMainPage("Comments");
        }

        private void RemoveBlankObservations()
        {
            foreach (SectionHelp.Section section in result.sections)
            {
                foreach (SectionHelp.Surveyquestionanswerlist secQA in section.surveyQuestionAnswerList)
                {
                    if (secQA.renderAddObservation)
                    {
                        int obsNumber = secQA.obsevationNumber;
                        if (obsNumber == 1)
                            continue;
                        if (secQA.answersList.Count() < 9)
                            continue;
                        if (obsNumber > 5)
                            continue;
                        for (int i = 2; i <= 5; i++)
                        {
                            int n = i * 2;
                            if (secQA.answersList[n - 2].defaultVisible && secQA.answersList[n - 1].defaultVisible)
                            {
                                if (secQA.answersList[n - 2].answer == null && secQA.answersList[n - 1].answer == null)
                                {
                                    secQA.answersList[n - 2].defaultVisible = false;
                                    secQA.answersList[n - 1].defaultVisible = false;
                                    secQA.obsevationNumber = secQA.obsevationNumber - 1;
                                }
                                else
                                {
                                    AdjustObservations(secQA, n);
                                }
                            }


                        }
                    }
                }
            }
        }

        private void AdjustObservations(SectionHelp.Surveyquestionanswerlist secQA, int n)
        {
            int emptySlot;
            if (FindFirstEmptyObservation(secQA, out emptySlot))
            {
                int sl = emptySlot * 2;
                secQA.answersList[sl - 2].answer = secQA.answersList[n - 2].answer;
                secQA.answersList[sl - 2].defaultVisible = true;
                //secQA.answersList[sl - 2].observatgionNumberString = secQA.answersList[n - 2].observatgionNumberString;
                secQA.answersList[sl - 1].answer = secQA.answersList[n - 1].answer;
                secQA.answersList[sl - 1].defaultVisible = true;
                //secQA.answersList[sl - 1].observatgionNumberString = secQA.answersList[n - 1].observatgionNumberString;
                secQA.answersList[n - 2].answer = null;
                secQA.answersList[n - 2].defaultVisible = false;
                secQA.answersList[n - 1].answer = null;
                secQA.answersList[n - 1].defaultVisible = false;
            }
        }

        private bool FindFirstEmptyObservation(SectionHelp.Surveyquestionanswerlist seCQA, out int foundblank)
        {
            bool found = false;
            foundblank = -1;
            for (int j = 2; j <= seCQA.obsevationNumber; j++)
            {
                int n = j * 2;
                if (seCQA.answersList[n - 2].answer == null && seCQA.answersList[n - 1].answer == null)
                {
                    foundblank = j;
                    found = true;
                    break;
                }
            }
            return found;
        }

        private void MoveObservation()
        {
            foreach (SectionHelp.Section section in result.sections)
            {
                foreach (SectionHelp.Surveyquestionanswerlist secQA in section.surveyQuestionAnswerList)
                {

                    if (secQA.renderAddObservation)
                    {
                        int obsNumber = secQA.obsevationNumber;
                        if (obsNumber == 1)
                            continue;
                        var ansList = secQA.answersList.ToList();
                        for (int i = 1; i <= obsNumber; i++)
                        {
                            int n = i * 2;
                            //var AnsList = secQA.answersList.ToList();
                            if (ansList[n - 2].answer == null && ansList[n - 1].answer == null)
                            {
                                secQA.answersList[n - 2].defaultDisalbled = true;
                                secQA.answersList[n - 1].defaultDisalbled = true;
                                secQA.obsevationNumber = secQA.obsevationNumber - 1;

                            }
                        }
                    }
                }
            }
        }
    }
}
