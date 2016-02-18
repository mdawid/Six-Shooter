using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Bing;

namespace Six_Shooter.Lib
{
    public sealed class AddressValidator : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
        }

        public async Task<string> Validate(Bing.Maps.Address address)
        {
            Bing.Maps.UserContext User = new Bing.Maps.UserContext(Utility.GetLocalIp());
            Bing.MapsClient mapclient = new MapsClient(App.BingKey,"en-US", User);
            try
            {
                address.AddressLine = address.FormattedAddress.Split(',')[0].ToString();
                address.Locality = address.FormattedAddress.Split(',')[1].ToString();
                address.AdminDistrict = address.FormattedAddress.Split(',')[2].Split(' ')[1].ToString();
                address.PostalCode = address.FormattedAddress.Split(',')[2].Split(' ')[2].ToString();
            }
            catch (Exception e)
            {
                return "BadAddress";
            }
            try
            {
                Bing.Maps.MapsResponse<Bing.Maps.Location> location = await mapclient.LocationQuery(address, 1, true);
                return location.ResourceSets[0].Resources[0].Confidence.ToString();
            }
            catch (Exception e)
            {
                return "BadAddress";
            }
        }
    }
 }
