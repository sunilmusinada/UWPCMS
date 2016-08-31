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
using MyToolkit.Controls;
using System.Linq;
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
        public static UserSurvey SelectedSurvey = null;
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
        private ObservableCollection<UserSurvey> _FilteredUsersurveys;
        //ProgressRing Progress = null;
        // Property.
      
        public ObservableCollection<UserSurvey> FilteredUsersurveys
        {
            get { return _FilteredUsersurveys; }
            set
            {
                if (value != _FilteredUsersurveys)
                {
                    _FilteredUsersurveys = value;
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
            //SelectedSurvey = null;
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
            this.FilteredUsersurveys = this.Usersurveys;
            //progressRing.IsActive = false;
            //Progress.IsActive = false;
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

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dgrid = sender as DataGrid;
            SelectedSurvey = dgrid.SelectedItem as UserSurvey;
        }

        private void Filter_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            TextBox txtBox = sender as TextBox;
            string txtBxName = txtBox.Name;
            string FilterText = txtBox.Text.ToUpper();
            if (string.IsNullOrEmpty(FilterText))
            {
                FilteredUsersurveys = Usersurveys;
                return;
            }
            switch(txtBxName)
            {
                case "IDFilter":
                    FilteredUsersurveys = new ObservableCollection<UserSurvey>((from surv in Usersurveys
                                                                                where surv.surveyKey.StartsWith(FilterText)
                                                                                select surv).ToList());
                    break;
                case "ProviderFilter":
                    FilteredUsersurveys = new ObservableCollection<UserSurvey>((from surv in Usersurveys
                                                                                where surv.surveyProvider.Contains(FilterText)
                                                                                select surv).ToList());
                    break;
                case "EndDateFilter":
                    FilteredUsersurveys = new ObservableCollection<UserSurvey>((from surv in Usersurveys
                                                                                where surv.endDateString.StartsWith(FilterText)
                                                                                select surv).ToList());
                    break;
                case "ActionDateFilter":
                    FilteredUsersurveys = new ObservableCollection<UserSurvey>((from surv in Usersurveys
                                                                                where surv.actionDateString.StartsWith(FilterText)
                                                                                select surv).ToList());
                    break;
            }
              
                
        }
    }
}
