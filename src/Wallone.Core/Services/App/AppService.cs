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
            var themeName = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .GetImage();

            var theme = new ThemeCreatedBuilder()
                .SetName(themeName)
                .HasDownloaded()
                .GetThemeModelFromFile();

            var controller = new ThemeController();
            controller.Load(theme);

            var themeScheduler = new ThemeScheduler(controller);
            themeScheduler.Start();
        }

        public static async Task LoadGeolocationAsync()
        {
            var useGeolocation = new SettingsBuilder(SettingsService.Get())
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
            var settings = new SettingsBuilder(SettingsService.Get())
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