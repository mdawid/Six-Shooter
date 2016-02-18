using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Six_Shooter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SV.Content = new Pages.Default();
        }

        private void ExpandSV(object sender, TappedRoutedEventArgs e)
        {
            SV.IsPaneOpen = !SV.IsPaneOpen;
        }
        private void ExpandSV(object sender, RoutedEventArgs e)
        {
            SV.IsPaneOpen = !SV.IsPaneOpen;
        }

        private void NewAddressPush(object sender, TappedRoutedEventArgs e)
        {
            //SV.Content = new Pages.AddressPush();
            MenuFlyout.ShowAttachedFlyout((FrameworkElement)sender);
        }
        private void NewAddressPush(object sender, RoutedEventArgs e)
        {
            //SV.Content = new Pages.AddressPush();
            MenuFlyout.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void NewLinkPush(object sender, TappedRoutedEventArgs e)
        {
            SV.Content = new Pages.LinkPush();
        }

        private void NewFilePush(object sender, TappedRoutedEventArgs e)
        {
            SV.Content = new Pages.FilePush();
        }

        private void NewNotePush(object sender, TappedRoutedEventArgs e)
        {
            SV.Content = new Pages.NotePush();
        }

        private void NearestAddressTapped(object sender, TappedRoutedEventArgs e)
        {
            SV.Content = new Pages.NearestAddress();
        }
        private void NearestAddressTapped(object sender, RoutedEventArgs e)
        {
            SV.Content = new Pages.NearestAddress();
        }

        private void GeoLocationTapped(object sender, RoutedEventArgs e)
        {
            SV.Content = new Pages.GeoLocation();
        }

        private void GeoLocationTapped(object sender, TappedRoutedEventArgs e)
        {
            SV.Content = new Pages.GeoLocation();
        }

        private void SettingsTapped(object sender, TappedRoutedEventArgs e)
        {
            SV.Content = new Pages.Settings();
        }
    }
}
