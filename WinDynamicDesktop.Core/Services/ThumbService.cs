using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ThumbService
    {
        public static Task<List<Thumb>> GetThumbsAsync(string fields = null)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers", fields);
            return items;
        }
        public static List<Thumb> GetThumbs(string fields = null)
        {
            var items = RequestRouter<List<Thumb>>.Get("wallpapers", fields);
            return (List<Thumb>)items;
        }
    }
}
