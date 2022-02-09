using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ThumbService
    {
        public static Task<List<Thumb>> GetThumbsAsync(string page, List<Models.Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers", page, parameters);
            return items;
        }

        public static Task<List<Thumb>> GetThumbsFavoriteAsync(string page, List<Models.Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers/favorite", page, parameters);
            return items;
        }

        public static Task<List<Thumb>> GetThumbsInstallAsync(string page, List<Models.Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers/install", page, parameters);
            return items;
        }

        public static List<Thumb> GetThumbs(string page = null)
        {
            var items = RequestRouter<List<Thumb>>.Get("wallpapers", page);
            return (List<Thumb>)items;
        }

        public static bool CheckItems(List<Thumb> items)
        {
            return items != null || items.Count > 0;
        }
    }
}
