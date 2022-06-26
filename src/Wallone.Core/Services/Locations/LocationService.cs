using System.Threading.Tasks;
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
    }
}