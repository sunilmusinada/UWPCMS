using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace CMS_Survey.Helpers
{
    public class CMSCombobox:Windows.UI.Xaml.Controls.ComboBox
    {
        private ScrollViewer _scrollViewer;

        public CMSCombobox()
        {
            DefaultStyleKey = typeof(ComboBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (_scrollViewer != null)
            {
                _scrollViewer.Loaded += OnScrollViewerLoaded;
            }
        }

        private void OnScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            _scrollViewer.Loaded -= OnScrollViewerLoaded;
            _scrollViewer.ChangeView(null, 0, null);
        }
    }
}
