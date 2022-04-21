using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;

namespace Wallone.Core.Services
{
    public class BrandsService
    {
        public static Task<List<Brand>> GetBrandAsync(string page = null)
        {
            var items = RequestRouter<List<Brand>>.GetAsync("brands", page);
            return items;
        }
    }
}