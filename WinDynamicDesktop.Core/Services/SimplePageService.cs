using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class SimplePageService
    {
        private static SimplePage simplePage;

        public SimplePageService(SimplePage page)
        {
            Load(page);
        }
        public static void Load(SimplePage page)
        {
            if (page != null)
                simplePage = page;
        }
        public static SimplePage GetSimplePage()
        {
            return simplePage;
        }
        public static string GetHeader()
        {
            return simplePage.name ?? "Lorem ipsum";
        }

        public static string GetDescription()
        {
            return simplePage.description ?? "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }

        public static string GetUsername()
        {
            return simplePage?.user?.name ?? "Lorem";
        }
        public static Task<string> GetPageAsync(string fields = null)
        {
            var items = RequestRouter<string>.GetAsync("wallpapers/one", fields);
            return items;
        }
    }
}
