using Microsoft.Advertising.WinRT.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace WeekNumber
{
    public sealed partial class AboutUserControl
    {
        private readonly AdControl _ad;
        public AboutUserControl(AdControl ad)
        {
            InitializeComponent();
            _ad = ad;
            ad.Visibility = Visibility.Collapsed;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var parent = Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }
            SettingsPane.Show();
               _ad.Visibility = Visibility.Visible;
        }
    }
}
        
