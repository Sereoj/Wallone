using System;
using System.Collections.Generic;
using System.Text;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.UI.Services
{
    public class ProfileService
    {
        private static Profile profile;

        public ProfileService(Profile page)
        {
            Load(page);
        }

        public static void Load(Profile page)
        {
            if (page != null)
                profile = page;
        }

        public static string GetUsername()
        {
            return profile?.name ?? "Lorem";
        }
        public static string GetAvatar()
        {
            return profile?.avatar;
        }
        public static string GetCover()
        {
            return profile?.cover;
        }

        public static string GetCountry()
        {
            return profile?.country;
        }
        public static string GetFriends()
        {
            return profile?.friends;
        }
        public static string GetDescription()
        {
            return profile?.description;
        }
        public static string GetPublish()
        {
            return profile?.publish ?? "0";
        }
    }
}
