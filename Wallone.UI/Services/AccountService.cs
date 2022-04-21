using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.UI.Services
{
    public class AccountService
    {
        private static User user;
        private static string cover;

        public AccountService(User page)
        {
            Load(page);
        }

        public static User getUser()
        {
            return user;
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

        public static string GetAvatar()
        {
            return user.avatar;
        }

        public static void SetCover(string value)
        {
            cover = value;
        }

        public static string GetCover()
        {
            return cover;
        }

        public static DateTime GetDOB()
        {
            return user.dob != null ? DateTime.Parse(user.dob) : DateTime.Now;
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

        public static Task<User> EditUserPageAsync(User user, List<Parameter> parameters)
        {
            var items = RequestRouter<User, User>.PostAsync("user/edit", user, parameters);
            return items;
        }

        public static Task<string> GetPageGuidsAsync()
        {
            var items = RequestRouter<string>.GetAsync("app/guids", null);
            return items;
        }
    }
}