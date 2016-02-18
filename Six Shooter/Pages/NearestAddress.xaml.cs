using System;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Popups;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Six_Shooter.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NearestAddress : Page
    {
        private LocationHandler LocHandler = new LocationHandler();
        private MapLocationFinderResult _addresses = null;
        private Brush _AddressBorderBrush = null;
        private string _MatchConfidence = null;

        public NearestAddress()
        {
            this.InitializeComponent();
            _AddressBorderBrush = AddressToSendText.BorderBrush;
            //Bing map service token
            MC.MapServiceToken = App.BingKey;
            //Set the LocationHandler to 20 seconds
            LocHandler._geolocator = new Geolocator { ReportInterval = 20000 };
            //We want the best location we can get, so High.
            LocHandler._geolocator.DesiredAccuracy = PositionAccuracy.High;
            //Kick it
            GetLocation();
        }



        async private void GetLocation()
        {
            LocHandler._position = await LocHandler._geolocator.GetGeopositionAsync();
            _addresses = await MapLocationFinder.FindLocationsAtAsync(LocHandler._position.Coordinate.Point);
            await MC.TrySetViewAsync(LocHandler._position.Coordinate.Point, 16D);
            DropPin();
            AddressToSendText.Text = _addresses.Locations.First().Address.FormattedAddress.ToString();
        }

        private void DropPin()
        {
            MapIcon MyLocation = new MapIcon();
            MyLocation.Location = LocHandler._position.Coordinate.Point;
            MyLocation.NormalizedAnchorPoint = new Point(1.0, 0.5);
            MyLocation.Title = _addresses.Locations.First().Address.FormattedAddress;
            MyLocation.Visible = true;
            MC.MapElements.Add(MyLocation);
        }

        private async void PreventBadAddress(object sender, TextChangedEventArgs e)
        {
           Bing.Maps.Address address = new Bing.Maps.Address();

            address.FormattedAddress = AddressToSendText.Text;

            Lib.AddressValidator validator = new Lib.AddressValidator();

            _MatchConfidence = await validator.Validate(address);

            if (_MatchConfidence == "High")
            {
                AddressToSendText.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else if (_MatchConfidence == "Medium")
            {
                AddressToSendText.BorderBrush = new SolidColorBrush(Colors.Orange);
            }
            else if (_MatchConfidence == "BadAddress" || _MatchConfidence == "Low")
            {
                AddressToSendText.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                AddressToSendText.BorderBrush = _AddressBorderBrush;
            }
        }

        private async void GetSelectedDestination(object sender, SelectionChangedEventArgs e)
        {
            //Contact selected but not device
            if (ContactsComboBox.SelectedItem != null && DevicesComboBox.SelectedItem == null)
            {
                CreateSendDialog();
            }
            //Device selected but not contact
            else if (DevicesComboBox.SelectedItem != null && ContactsComboBox.SelectedItem == null)
            {
                CreateSendDialog();
            }
            //Both must be selected as it wasn't one or the other.
            else
            {

            }
        }

        private async void CreateSendDialog(string Content)
        {
            var dialog = new MessageDialog("Address: goood.\nDestination: good.", "Ready to send.");
            dialog.Commands.Add(new UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new UICommand("No") { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            dialog.Commands[0].Invoked += delegate { SendClicked(); };
            dialog.Commands[1].Invoked += delegate { DontSendClicked(); };

            IUICommand result = await dialog.ShowAsync();
            //return result;
        }

        private void SendClicked()
        {

        }

        private void DontSendClicked()
        {

        }
    }
}
