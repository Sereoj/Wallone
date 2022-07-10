using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ModernWpf.Controls;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services
{
    public class CategoriesService
    {
        private static Task<List<Category>> GetCategoryAsync(string fields = null)
        {
            var items = RequestRouter<List<Category>>.GetAsync("categories", fields);
            return items;
        }

        public static ObservableCollection<NavigationViewItem> LoadCategories()
        {
            var items = Task.Run(async () => await GetCategoryAsync()).Result;

            var categories = new ObservableCollection<NavigationViewItem>();
            if(items != null)
            {
                foreach (var item in items.Where(item => item.Status))
                {
                    categories.Add(new NavigationViewItem
                    {
                        Uid = item.ID,
                        Content = item.Name,
                        Name = "Categories",
                        Icon = FontIconService.SetIcon("ultimate", item.Icon),
                        Tag = "Gallery"
                    });
                }
            }

            return categories;
        }
    }
}