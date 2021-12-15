using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class CategoriesService
    {
        public static Task<List<Category>> GetCategoryAsync(string fields = null)
        {
            var items = RequestRouter<List<Category>>.GetAsync("categories", fields);
            return items;
        }
    }
}
