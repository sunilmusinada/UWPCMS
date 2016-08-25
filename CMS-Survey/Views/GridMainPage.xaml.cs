using System;
using CMS_Survey.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.IO;
using Windows.ApplicationModel;
using CMS_Survey.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Template10.Common;

namespace CMS_Survey.Views
{
    public sealed partial class GridMainPage : Page, INotifyPropertyChanged
    {

        public delegate void MainPageLoadingHandler(object sender, EventArgs e);
        private string _filter;
        public event MainPageLoadingHandler PageLoadEvent;
        
        private ObservableCollection<UserSurvey> _Usersurveys;
        //ProgressRing Progress = null;
        // Property.
        bool isOffline = false;
        public UserSurvey SelectedSurvey = null;
        public ObservableCollection<UserSurvey> Usersurveys
        {
            get { return _Usersurveys; }
            set
            {
                if (value != _Usersurveys)
                {
                    _Usersurveys = value;
                    // Notify of the change.
                    NotifyPropertyChanged();
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Filter
        {
            get { return _filter; }
            set { _filter=value; }
        }


        public GridMainPage()
        {
            this.DataContext = this;
            //GetSurveys();
            Services.ServiceHelper.ServiceHelperObject.getJsonFile();
            InitializeComponent();
            this.Loading += MainPage_Loading;
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            //SurveyObject.Rootobject survObj = getJson();
        }

        private void MainPage_Loading(FrameworkElement sender, object args)
        {

            GetSurveys();
            //HideAllControls();
            //isOffline = Services.ServiceHelper.ServiceHelperObject.isOffline;
            SelectedSurvey = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void GetSurveys()
        {
            //progressRing = new ProgressRing();
            //progressRing.IsActive = true;
            var svcHelper = Services.ServiceHelper.ServiceHelperObject;
            if (await Services.ServiceHelper.ServiceHelperObject.IsOffline())
            {
                svcHelper.CallUserSurveyServiceOffline();
            }
            else
                await svcHelper.CallUserSurveyService();

            // Progress.IsActive = true;
            this.Usersurveys = Services.ServiceHelper.ServiceHelperObject.UserSurveyList;
            //progressRing.IsActive = false;
            //Progress.IsActive = false;
        }
        //private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    // this.Frame.Navigate(typeof(Pages.NewSurvey));
        //    if (e == null)
        //    {
        //        HideAllControls();
        //        return;
        //    }
        //    SelectedSurvey = (UserSurvey)e.ClickedItem;
        //    if (SelectedSurvey != null)
        //    {
        //        SelectedSurveyIdBlock.Visibility = Visibility.Visible;
        //        SelectedSurveyIdBlock.Text = "Survey #" + SelectedSurvey.surveyKey;
        //        SelectedSurveyType.Visibility = Visibility.Visible;
        //        SelectedSurveyType.Text = "Survey Type : " + SelectedSurvey.surveyType;
        //        SelectedProvider.Visibility = Visibility.Visible;
        //        SelectedProvider.Text = "Provider : " + SelectedSurvey.surveyProvider;
        //        SelectedSurveyStatus.Visibility = Visibility.Visible;
        //        SelectedSurveyStatus.Text = "Status : " + SelectedSurvey.status;
        //        SelectedSurveyEndDate.Visibility = Visibility.Visible;
        //        SelectedSurveyEndDate.Text = "Survey Start Date : " + SelectedSurvey.startDateString;
        //        SelectedSurveyActionDate.Visibility = Visibility.Visible;
        //        SelectedSurveyActionDate.Text = "Survey Action Date : " + SelectedSurvey.actionDateString;
        //        Edit.Visibility = Visibility.Visible;
        //        Delete.Visibility = Visibility.Visible;
        //        Delete.IsEnabled = false;
        //        if (await Services.ServiceHelper.ServiceHelperObject.IsOffline())
        //        Delete.IsEnabled =false;
        //        else
        //            Delete.IsEnabled = true;
        //    }
        //    else
        //    {

        //    }
        //}

        //private void HideAllControls()
        //{
        //    SelectedSurvey = null;
        //    SelectedSurveyIdBlock.Text = "Survey #";
        //    SelectedSurveyIdBlock.Visibility = Visibility.Collapsed;
        //    SelectedSurveyType.Text = "Survey Type : ";
        //    SelectedSurveyType.Visibility = Visibility.Collapsed;
        //    SelectedProvider.Text = "Provider : ";
        //    SelectedProvider.Visibility = Visibility.Collapsed;
        //    SelectedSurveyStatus.Text = "Status : ";
        //    SelectedSurveyStatus.Visibility = Visibility.Collapsed;
        //    SelectedSurveyEndDate.Text = "Survey End Date : ";
        //    SelectedSurveyEndDate.Visibility = Visibility.Collapsed;
        //    SelectedSurveyActionDate.Text = "Survey Action Date : ";
        //    SelectedSurveyActionDate.Visibility = Visibility.Collapsed;
        //    Edit.Visibility = Visibility.Collapsed;
        //    Delete.Visibility = Visibility.Collapsed;
        //}

        private void GetClickedSurvey(string SurveyKey)
        {
            try
            {
                //var currentUserKey = Services.ServiceHelper.ServiceHelperObject.currentUser.UserKey;
                //SectionHelp.Rootobject secHelp = await Services.ServiceHelper.ServiceHelperObject.CallGetSurveyService(Convert.ToString(currentUserKey), SurveyKey);
                //SectionHelp.Rootobject param = new Models.SectionHelp.Rootobject();
                //param.help = secHelp.help;
                //param.renderComments = secHelp.renderComments;
                //param.sections = secHelp.sections;
                ////var str = "Hello";
                //var param1=JsonConvert.SerializeObject(secHelp);
                //var param = Newtonsoft.Json.JsonConvert.SerializeObject<SectionHelp.Rootobject>(secHelp);
                    var param = Template10.Services.SerializationService.SerializationService.Json.Serialize(SurveyKey);
                    this.Frame.Navigate(typeof(Pages.NewSurvey), param);
                

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            GetClickedSurvey(SelectedSurvey.surveyKey);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (isOffline)
            {

            }
            else
            {
                await Services.ServiceHelper.ServiceHelperObject.DeleteSurvey(SelectedSurvey.surveyKey);
            }
            GetSurveys();
            //HideAllControls();
        }
    }
}
