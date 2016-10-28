using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CMS_Survey.Views
{
    public sealed partial class SettingsPage : Page
    {
        Template10.Services.SerializationService.ISerializationService _SerializationService;

        public SettingsPage()
        {
            InitializeComponent();
            StateCombobox.Items.Clear();
            _SerializationService = Template10.Services.SerializationService.SerializationService.Json;
            var states=Services.ServiceHelper.ServiceHelperObject.GetStates();
            states.ForEach(e => StateCombobox.Items.Add(e.stateName));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
            MyPivot.SelectedIndex = index;
        }

        private void StateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmbx = sender as ComboBox;
            string state =Convert.ToString(cmbx.SelectedValue);
            string stateName=Services.ServiceHelper.ServiceHelperObject.GetCodeforState(state);
            ViewModel.SettingsPartViewModel.SetState(stateName);
        }
    }
}
