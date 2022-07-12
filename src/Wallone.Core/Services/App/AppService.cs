using System.Threading.Tasks;
using Newtonsoft.Json;
using Wallone.Core.Builders;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Models.App;
using Wallone.Core.Schedulers;
using Wallone.Core.Services.Locations;
using Wallone.Core.Services.Loggers;

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

        public static async Task UseGeolocationAsync()
        {
            var items = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder();

            if(items.GetGeolocation())
            {
                switch (items.GetGeolocationMode())
                {
                    case Geolocation.Auto:
                        await GeoLocationRepository.Auto.InitAsync();
                        break;
                    case Geolocation.Windows:
                        if(GeoLocationRepository.Windows.Is())
                        {
                            await GeoLocationRepository.Windows.InitAsync();
                        }
                        else
                        {
                            items
                            .SetGeolocationMode(Geolocation.Auto)
                            .Build();
                            await UseGeolocationAsync();
                        }
                        break;
                    case Geolocation.Custom:
                        GeoLocationRepository.Custom.Init();
                        break;
                    default:
                        items
                            .SetGeolocationMode(Geolocation.Auto)
                            .Build();
                        await UseGeolocationAsync();
                        break;
                }
            }
        }
    }
}