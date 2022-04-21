using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;

namespace Wallone.Core.Services
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
