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
using Windows.ApplicationModel.Background;
using System.Net.Http;
using Windows.Data.Json;

namespace Six_Shooter
{
    public sealed class AddressValidator : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
        }

        HttpClient Validator = new HttpClient();
        public string _jsonString = null;

        public async Task Validate(string StreetAddress, string City, string PostalCode, string State, string CountryCode)
        {
            string UriBase = "http://dev.virtualearth.net/REST/v1/Locations/";
            UriBase += CountryCode + "/";
            UriBase += State + "/";
            UriBase += PostalCode + "/";
            UriBase += City + "/";
            UriBase += StreetAddress;
            UriBase += "?includeNeighborhood=1&maxResults=1&key=";
            UriBase += App.BingKey;
            HttpResponseMessage response = await Validator.GetAsync(UriBase);
            _jsonString = await response.Content.ReadAsStringAsync();
            Parse();
            
        }

        private void Parse()
        {
            JsonArray root = JsonValue.Parse(_jsonString).GetArray();
            int i = 1000000;
            while (i > 0)
            {
                i++;
            }
        }
    }
 }
