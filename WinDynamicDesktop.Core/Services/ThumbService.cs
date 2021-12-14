using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ThumbService
    {
        public static Task<List<Thumb>> GetThumbsAsync(string page = null)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers", page);
            return items;
        }

        public static List<Thumb> GetThumbs(string page = null)
        {
            var items = RequestRouter<List<Thumb>>.Get("wallpapers", page);
            return (List<Thumb>)items;
        }
    }
}
