using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Routers;
using Windows.Devices.Geolocation;

namespace Wallone.Core.Services.Locations
{
    public class GeoLocationFactory
    {
        public static Location Create()
        {
            return new Location();
        }
    }
    public class GeoLocationRepository
    {
        private static Location location;

        public static void SetGeolocation(Location value)
        {
            if(value != null)
            {
                location = value;
            }
            else
            {
                _ = LoggerService.LogAsync(typeof(GeoLocationRepository), "Не удалось задать модель геолокации", Message.Error);
            }
        }

        public static Location GetGeolocation()
        {
            return location;
        }

        public static Geolocation GetGeolocationMode()
        {
            return new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetGeolocationMode();
        }

        public static bool Validate()
        {
            if (double.IsNaN(location.latitude) && double.IsNaN(location.longitude))
            {
                _= LoggerService.LogAsync(typeof(GeoLocationRepository), "Значения latitude и longitude должны быть валидными");
                return false;
            }
            return true;
        }

        public class Auto
        {
            private static Task<string> GetFromEthernet()
            {
                var items = RequestRouter<string>.GetAsync("app/ip");
                return items;
            }
            public static Location Get()
            {
                return new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder()
                    .GetLocation();
            }

            internal static async Task InitAsync()
            {
                var items = new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder();

                var data = await GetFromEthernet();
                await Task.CompletedTask;

                try
                {
                    var location = JsonConvert.DeserializeObject<Location>(data);
                    SetGeolocation(location);

                    items.SetLatitude(location.latitude)
                        .SetCountry(location.country)
                        .SetCity(location.city)
                        .SetLongitude(location.longitude)
                        .Build();
                }
                catch (Exception ex)
                {
                    _ = LoggerService.LogAsync(typeof(Auto), $"Критическая ошибка: {ex.Message}", Message.Error);
                }
            }
        }

        public class Windows
        {
            public static bool Is()
            {
                var requiredVersion = new Version("10.0.10240.0");
                return requiredVersion < OSHelper.Get().Version;
            }
            public static Location Get()
            {
                return new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder()
                    .GetLocation();
            }

            internal static async Task InitAsync()
            {
                var items = new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder();

                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Unspecified:
                        break;
                    case GeolocationAccessStatus.Allowed:
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };
                        Geoposition pos = await geolocator.GetGeopositionAsync();

                        var location = GeoLocationFactory.Create();

                        location.latitude = pos.Coordinate.Point.Position.Latitude;
                        location.longitude = pos.Coordinate.Point.Position.Longitude;
                        SetGeolocation(location);

                        items.SetLatitude(location.latitude)
                            .SetLongitude(location.longitude)
                            .Build();
                        break;
                    case GeolocationAccessStatus.Denied:
                        _ = LoggerService.LogAsync(typeof(Windows), "В операционной системе Windows отключена геолокация, включите её");
                        break;
                    default:
                        break;
                }
            }
        }

        public class Custom
        {
            public static Location Get()
            {
                return new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder()
                    .GetLocation();
            }

            internal static void Init()
            {
                SetGeolocation(Get());
            }
        }
    }
}