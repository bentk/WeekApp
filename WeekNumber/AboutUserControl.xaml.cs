using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace WeekNumber
{
    public sealed partial class AboutUserControl
    {
        public AboutUserControl()
        {
            InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            var parent = Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }
            SettingsPane.Show();
        }
    }
}
