using System.Threading.Tasks;
using Newtonsoft.Json;
using Wallone.Core.Builders;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Models.App;
using Wallone.Core.Schedulers;
using Wallone.Core.Services.Locations;

namespace Wallone.Core.Services.App
{
    public class AppService
    {
        public static void UseTheme()
        {
            var settingsItems = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder();

            var themeName = settingsItems.GetImage();

            var theme = new ThemeCreatedBuilder()
                .SetName(themeName)
                .HasDownloaded()
                .GetThemeModelFromFile();

            var location = settingsItems.GetLocation();

            var themeController = new ThemeController<Theme>(theme, new GeolocationController<Location>(location));
            if (themeController.ValidateFields())
            {
                var themeScheduler = new ThemeScheduler(themeController);
                ThemeScheduler.Start();
            }
        }
        public static async Task LoadGeolocationAsync()
        {
            var useGeolocation = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetGeolocation();

            if (useGeolocation)
            {
                await UseGeolocation();
                await Task.CompletedTask;
            }
        }

        public static async Task LoadVersionAsync()
        {
            var data = await AppVersionService.GetVersionAsync();
            await Task.CompletedTask;
            if (!string.IsNullOrEmpty(data))
            {
                var appVersion = JsonConvert.DeserializeObject<AppVersion>(data);
                AppVersionService.SetVersion(appVersion?.Version);
            }
            else
            {
                AppVersionService.SetVersion(null);
            }
        }

        public static async Task UseGeolocation()
        {
            var settings = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder();

            if (settings.GetGeolocation())
            {
                var dataLocation = await LocationService.GetLocationAsync();
                await Task.CompletedTask;
                if (!string.IsNullOrEmpty(dataLocation))
                {
                    var location = JsonConvert.DeserializeObject<Location>(dataLocation);
                    if (location != null)
                        settings
                            .SetLatitude(location.latitude)
                            .SetLongitude(location.longitude)
                            .SetCountry(location.country)
                            .SetCity(location.city)
                            .SetMode(Mode.UseWebLocation)
                            .Build();
                }
            }
        }
    }
}