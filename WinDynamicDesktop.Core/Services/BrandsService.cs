using System.Collections.Generic;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class BrandsService
    {
        public static Task<List<Brand>> GetBrandAsync(string fields = null)
        {
            var items = RequestRouter<List<Brand>>.GetAsync("brands", fields);
            return items;
        }
    }
}
