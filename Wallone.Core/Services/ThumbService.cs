using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Wallone.Core.Models;

namespace Wallone.Core.Services
{
    public class ThumbService
    {
        public static HttpStatusCode GetStatusCode()
        {
            return AppEthernetService.GetStatus();
        }

        public static Task<List<Thumb>> GetThumbsAsync(string router, string page, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync(router, page, parameters);
            return items;
        }

        public static Task<List<Thumb>> GetThumbsFavoriteAsync(string page, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers/favorite", page, parameters);
            return items;
        }

        public static Task<List<Thumb>> GetThumbsInstallAsync(string page, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers/install", page, parameters);
            return items;
        }

        public static bool CheckItems(List<Thumb> items)
        {
            return items != null;
        }
    }
}