using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Windows.UI;

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



        private async void SendPush(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
