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
using CMS_Survey.Template;
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
            //
            CreateDatabase.CopyDataBase();
            conn = new SQLiteConnection("Surveydb.sqlite");
            //var usrfolder = ApplicationData.Current.LocalFolder;
            //if (!Directory.Exists(usrfolder.Path + @"\Surveys"))
            //{
            //    StorageFolder folder = await usrfolder.CreateFolderAsync("Surveys",
            //           CreationCollisionOption.OpenIfExists);
            //}
            Services.ServiceHelper.ServiceHelperObject.GetStates();
            //CreateDatabase.LoadDatabse(conn);
            // getJsonFile();
            var usrfolder = ApplicationData.Current.LocalFolder;
            if (Directory.Exists(usrfolder.Path + @"\Surveys"))
            {

                Directory.CreateDirectory(usrfolder.Path + @"\Surveys");
               
            }
            if (!await Services.ServiceHelper.ServiceHelperObject.IsOffline())
            {
                await Services.ServiceHelper.ServiceHelperObject.AddAllUsers();
            }
            BackGroundTaskHelper bgtHelper = new Template.BackGroundTaskHelper();
            if (!bgtHelper.TaskRegistered)
            {
                bgtHelper.RegisterTask();
            }
            //bgtHelper.Unregister();


            await Task.CompletedTask;
        }

      
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
           
            await Task.CompletedTask;
        }
    }
}

