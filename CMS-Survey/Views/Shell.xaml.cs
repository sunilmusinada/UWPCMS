using System;
using System.ComponentModel;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CMS_Survey.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        internal CMS_Survey.App _App;
        public INavigationService _navigationService;
        public event EventHandler NavigatingToOtherPage;
        public Shell()
        {
            Instance = this;
            InitializeComponent();
        }
        private void LoginHide(object sender, System.EventArgs e)
        {
            //LoginButton.IsEnabled = true;
            LoginModal.IsModal = false;
        }
        protected void OnNavigatingToOtherPage(EventArgs e)
        {
            EventHandler handler = NavigatingToOtherPage;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void LoginLoggedIn(object sender, System.EventArgs e)
        {
            //LoginButton.IsEnabled = false;
            LoginModal.IsModal = false;
        }
        public void LoginTapped(object sender, RoutedEventArgs e)
        {
            LoginModal.IsModal = true;
        }
        internal Shell(INavigationService navigationService,CMS_Survey.App App) : this()
        {
            if (Services.ServiceHelper.ServiceHelperObject.currentUser == null)
            {
                loginPart.LoggedIn += LoginPart_LoggedIn;
                LoginTapped(this, null);
            }
            _App = App;
            _navigationService = navigationService;
            SetNavigationService(navigationService);
        }

        private void LoginPart_LoggedIn(object sender, EventArgs e)
        {
            
            MyHamburgerMenu.NavigationService.Navigate(typeof(Views.GridMainPage));
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
        }

        public static implicit operator Shell(ContentControl v)
        {
            throw new NotImplementedException();
        }

        private void HamburgerButtonInfo_Selected(object sender, RoutedEventArgs e)
        {
            OnNavigatingToOtherPage(null);
        }

        private async void LogOutButton_Selected(object sender, RoutedEventArgs e)
        {
            Template10.Controls.HamburgerButtonInfo binfo = sender as Template10.Controls.HamburgerButtonInfo;
            binfo.IsChecked = false;
            
            ContentDialog LogoutDialog = new ContentDialog()
            {
                Title = "Logout",
                Content = "Do you want to Log out?",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Cancel"
            };

            ContentDialogResult result = await LogoutDialog.ShowAsync();
            if (result.Equals(ContentDialogResult.Primary))
            {
                Services.ServiceHelper.ServiceHelperObject.currentUser = null;
                _App.LaunchLoginPage();
                
            }
            else if(result.Equals(ContentDialogResult.Secondary))
            {
                
            }
                
        }
    }
}

