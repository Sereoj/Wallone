using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

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
            return profile.avatar;
        }
        public static string GetCover()
        {
            return profile.cover;
        }
        public static string GetFriend()
        {
            return profile.friend;
        }
        public static string GetCountry()
        {
            return profile?.country ?? "Unknown";
        }
        public static string GetDescription()
        {
            return profile?.description ?? "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }
        public static string GetFriends()
        {
            return profile?.friends_count ?? "0";
        }
        public static string GetLikes()
        {
            return profile?.users_likes ?? "0";
        }
        public static string GetPublish()
        {
            return profile?.publish_count ?? "0";
        }

        public static List<Thumb> GetPosts()
        {
            return profile.posts;
        }

        public static Task<string> GetPageAsync(string page_id)
        {
            var items = RequestRouter<string>.GetAsync($"user/{page_id}/info", null, null);
            return items;
        }
        public static Task SetAppendFriendAsync()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("friend_id", profile.id));
            var items = RequestRouter<string>.GetAsync($"user/add", null, parameters);
            return items;
        }
        public static Task SetRemoveFriendAsync()
        {
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("friend_id", profile.id));
            var items = RequestRouter<string>.GetAsync($"user/remove", null, parameters);
            return items;
        }
    }
}
