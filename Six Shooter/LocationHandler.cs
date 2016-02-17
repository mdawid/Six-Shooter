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
    public sealed class LocationHandler : IBackgroundTask
    {
        public Geolocator _geolocator = null;
        public Geoposition _position = null;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            StartTracking();
        }

        /// <summary>
        /// This is the click handler for the 'StartTracking' button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void StartTracking()
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    //LocationDisabledMessage.Visibility = Visibility.Collapsed;
                    break;

                case GeolocationAccessStatus.Denied:
                    //LocationDisabledMessage.Visibility = Visibility.Visible;
                    break;

                case GeolocationAccessStatus.Unspecified:
                    //LocationDisabledMessage.Visibility = Visibility.Collapsed;
                    break;

            }
        }

        /// <summary>
        /// This is the click handler for the 'StopTracking' button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void StopTracking(FrameworkElement sender, object e)
        {
            _geolocator.PositionChanged -= OnPositionChanged;
            _geolocator.StatusChanged -= OnStatusChanged;
            _geolocator = null;
        }

        /// <summary>
        /// Event handler for PositionChanged events. It is raised when
        /// a location is available for the tracking session specified.
        /// </summary>
        /// <param name="sender">Geolocator instance</param>
        /// <param name="e">Position data</param>
        private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateLocationData(e.Position);
            });
        }

        /// <summary>
        /// Event handler for StatusChanged events. It is raised when the 
        /// location status in the system changes.
        /// </summary>
        /// <param name="sender">Geolocator instance</param>
        /// <param name="e">Status data</param>
        private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            var dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Show the location setting message only if status is disabled.
                //LocationDisabledMessage.Visibility = Visibility.Collapsed;

                switch (e.Status)
                {
                    case PositionStatus.Ready:
                        // Location platform is providing valid data.
                        break;

                    case PositionStatus.Initializing:
                        // Location platform is attempting to acquire a fix. 
                        break;

                    case PositionStatus.NoData:
                        // Location platform could not obtain location data.
                        break;

                    case PositionStatus.Disabled:
                        // The permission to access location data is denied by the user or other policies.

                        // Show message to the user to go to location settings
                        //LocationDisabledMessage.Visibility = Visibility.Visible;

                        // Clear cached location data if any
                        UpdateLocationData(null);
                        break;

                    case PositionStatus.NotInitialized:
                        // The location platform is not initialized. This indicates that the application 
                        // has not made a request for location data.
                        break;

                    case PositionStatus.NotAvailable:
                        // The location platform is not available on this version of the OS.
                        break;

                    default:
                        break;
                }
            });
        }

        /// <summary>
        /// Updates the user interface with the Geoposition data provided
        /// </summary>
        /// <param name="position">Geoposition to display its details</param>
        private void UpdateLocationData(Geoposition position)
        {
            _position = position;
        }
    }
}
