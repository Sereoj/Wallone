using System.Collections.ObjectModel;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class ThumbService
    {
        public static ObservableCollection<Thumb> GetThumbs(string page = null)
        {
            var items = RequestRouter<ObservableCollection<Thumb>>
                .Get("walpapers", page);
            return null;
        }
    }
}
