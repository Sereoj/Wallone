using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ModernWpf.Controls;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services
{
    public class BrandsService
    {
        private static Task<List<Brand>> GetBrandAsync(string page = null)
        {
            var items = RequestRouter<List<Brand>>.GetAsync("brands", page);
            return items;
        }

        public static ObservableCollection<NavigationViewItem> LoadBrands()
        {
            var items = Task.Run(async () => await GetBrandAsync()).Result;

            var categories = new ObservableCollection<NavigationViewItem>();

            foreach (var item in items.Where(item => item.Status))
            {
                categories.Add(new NavigationViewItem
                {
                    Uid = item.ID,
                    Content = item.Name,
                    Name = "Brands",
                    Icon = FontIconService.SetIcon("ultimate", item.Icon),
                    Tag = "Gallery"
                });
            }

            return categories;
        }
    }
}