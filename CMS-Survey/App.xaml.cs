using Windows.UI.Xaml;
using System.Threading.Tasks;
using CMS_Survey.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Template10.Controls;
using Template10.Common;
using System;
using System.Linq;
using Windows.UI.Xaml.Data;
using SQLitePCL;
using CMS_Survey.Database;
using System.Net.Http;
using Windows.Storage;
using System.IO;
using System.Net;
using Windows.UI.Xaml.Controls;

namespace CMS_Survey
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : Template10.Common.BootStrapper
    {   
        public static SQLiteConnection conn;
        public App()
        {
            InitializeComponent();
           
            SplashFactory = (e) => new Views.Splash(e);

            #region App settings

            var _settings = SettingsService.Instance;
            RequestedTheme = _settings.AppTheme;
            CacheMaxDuration = _settings.CacheMaxDuration;
            ShowShellBackButton = _settings.UseShellBackButton;

            #endregion
        }
       
        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (Window.Current.Content as ModalDialog == null)
            {
                // create a new frame 
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);

                // create modal root
                Window.Current.Content = new ModalDialog
                {
                    DisableBackButtonWhenModal = true,
                    Content = new Views.Shell(nav),
                    ModalContent = new Pages.LoginPart()//.Busy(),
                };
            }
            conn = new SQLiteConnection("Surveydb.sqlite");
            Services.ServiceHelper.ServiceHelperObject.GetStates();
            CreateDatabase.LoadDatabse(conn);
           // getJsonFile();
            

           
            await Task.CompletedTask;
        }

      
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // long-running startup tasks go here
            //await Task.Delay(5000);

            //NavigationService.Navigate(typeof(Views.MainPage));
            await Task.CompletedTask;
        }
    }
}

