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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Six_Shooter.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NearestAddress : Page
    {
        public NearestAddress()
        {
            this.InitializeComponent();
            MC.MapServiceToken = "KPY1TTaOsy2ltJ5g2R6X~YG-IPaG00h_BxqJv9bfy2Q~AqdYFGH6Aj1YKP1kLcF9aHvhmuug6K9HFWBHKhcbfFP7X54w9nGFUP-xgL765jpX";
            GetLocation();       
        }

        async private void GetLocation()
        {
            LocationHandler LocHandler = new LocationHandler();
            LocHandler._geolocator = new Geolocator { ReportInterval = 20000 };
            LocHandler._geolocator.DesiredAccuracy = PositionAccuracy.High;

            Geoposition position = await LocHandler._geolocator.GetGeopositionAsync();
            while (position == null)
            {
            }
            MapLocationFinderResult addresses = await MapLocationFinder.FindLocationsAtAsync(position.Coordinate.Point);
            await MC.TrySetViewAsync(position.Coordinate.Point, 16D);
            DropPin(position, addresses);

        }

        private void DropPin(Geoposition position, MapLocationFinderResult addresses)
        {
            MapIcon MyLocation = new MapIcon();
            MyLocation.Location = position.Coordinate.Point;
            MyLocation.NormalizedAnchorPoint = new Point(1.0, 0.5);
            MyLocation.Title = addresses.Locations.First().Address.FormattedAddress;
            MyLocation.Visible = true;
            MC.MapElements.Add(MyLocation);
        }
    }
}
