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

        public static DateTime GetDOB()
        {
            return DateTime.Parse(user.dob);
        }

        public static string GetCountry()
        {
            return user.country;
        }

        public static string GetGithub()
        {
            return user.github;
        }

        public static string GetFacebook()
        {
            return user.facebook;
        }

        public static string GetVK()
        {
            return user.vk;
        }

        public static string GetTwitter()
        {
            return user.twitter;
        }
        public static Task<string> GetPageAsync()
        {
            var items = RequestRouter<string>.GetAsync("user/profile", null, null);
            return items;
        }

        public static Task<string> GetPageGuidsAsync()
        {
            var items = RequestRouter<string>.GetAsync("guids", null);
            return items;
        }
    }
}
