using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services.Pages
{
    public class AccountRepository
    {
        private static User user;

        public static void Load(User page)
        {
            if (page != null)
                user = page;
        }

        public static User GetUser()
        {
            return user;
        }

        public class AccountService
        {
            private static string cover;

            public AccountService(User page)
            {
                Load(page);
            }

            public static string GetUsername()
            {
                return user.username;
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
        }
    }
}