using System;
using System.ComponentModel;
using System.Linq;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CMS_Survey.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

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

        private void LoginLoggedIn(object sender, System.EventArgs e)
        {
            //LoginButton.IsEnabled = false;
            LoginModal.IsModal = false;
        }
        public void LoginTapped(object sender, RoutedEventArgs e)
        {
            LoginModal.IsModal = true;
        }
        public Shell(INavigationService navigationService) : this()
        {
            if (Services.ServiceHelper.ServiceHelperObject.currentUser == null)
            {
                loginPart.LoggedIn += LoginPart_LoggedIn;
                LoginTapped(this, null);
            }
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
    }
}

