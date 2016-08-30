using System;
using System.Threading.Tasks;
using Template10.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CMS_Survey.Pages
{
    public sealed partial class LoginPart : UserControl
    {

        public LoginPart()
        {

            this.InitializeComponent();
#if WINDOWS_UWP
            this.UserName.Text = "kalyan";
            this.Password.Password = "kalyancpamula";
#endif
        }

        public event EventHandler HideRequested;
        public event EventHandler LoggedIn;

        private async void LoginClicked(object sender, RoutedEventArgs e)
        {

            this.Login.IsEnabled = false;
            await CallLoginService();
            if(Services.ServiceHelper.ServiceHelperObject.currentUser==null)
            {
                this.ErrorMessage.Text = "Login failed for user";
            }
            else
            {
                
                LoggedIn?.Invoke(this, EventArgs.Empty);
            }
            this.Login.IsEnabled = true;
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            HideRequested?.Invoke(this, EventArgs.Empty);
        }

        private async Task CallLoginService()
        {
            Services.ServiceHelper srvHelper = Services.ServiceHelper.ServiceHelperObject;
           await srvHelper.CallLoginService(this.UserName.Text, this.Password.Password);
            
        }

        private void ThisPage_Loaded(object sender, RoutedEventArgs e)
        {
            UserName.Focus(FocusState.Keyboard);
        }
        //public Models.UserCredentials UserCredentials { get; set; }
    }
}
