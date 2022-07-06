using System.Threading.Tasks;
using Wallone.Core.Builders;
using Wallone.Core.Models;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services.Locations
{
    public class LocationService
    {
        public static Task<string> GetLocationAsync()
        {
            var items = RequestRouter<string>.GetAsync("app/ip");
            return items;
        }
        public static void SetLocation(Mode useWebLocation, bool useGeolocation)
        {
            new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .SetGeolocation(useGeolocation)
                .SetMode(useWebLocation)
                .Build();
            LoggerService.LogAsync(typeof(LocationService), $"useGeolocation: {useGeolocation} SetMode: {useWebLocation}");
        }
    }
}