using System;
using System.Threading.Tasks;
using Wallone.Core.Models.Settings;

namespace Wallone.Core.Services
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