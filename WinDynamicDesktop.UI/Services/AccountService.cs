using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.Services
{
    public class AccountService
    {
        private static User user;

        public AccountService(User page)
        {
            Load(page);
        }

        public static void Load(User page)
        {
            if (page != null)
                user = page;
        }

        public static string GetUsername()
        {
            return user.name;
        }

        public static string GetDescription()
        {
            return user.description;
        }

        public static object GetAvatar()
        {
            return user.avatar;
        }

        public static Task<string> GetPageAsync()
        {
            var items = RequestRouter<string>.GetAsync("user/profile", null, null);
            return items;
        }
    }
}
