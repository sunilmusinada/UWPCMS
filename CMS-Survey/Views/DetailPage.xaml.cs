using CMS_Survey.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using CMS_Survey.Database;

namespace CMS_Survey.Views
{
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Disabled;
            users_table user = new users_table();
            //user.insertUser();
        }
    }
}

