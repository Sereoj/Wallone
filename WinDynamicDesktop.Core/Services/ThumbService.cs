using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ThumbService
    {
        public static async Task<List<Thumb>> GetThumbsAsync(string page = null)
        {
            var items = await RequestRouter<List<Thumb>>.GetAsync("wallpapers", page).ConfigureAwait(true);
            return items;
        }

        public static List<Thumb> GetThumbs(string page = null)
        {
            var items = RequestRouter<List<Thumb>>.Get("wallpapers", page);
            return (List<Thumb>)items;
        }
    }
}
