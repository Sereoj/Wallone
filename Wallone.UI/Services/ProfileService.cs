using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.UI.Services
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

        public static string GetId()
        {
            return profile.id;
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

        public static string GetCountry()
        {
            return profile?.country ?? "Unknown";
        }

        public static string GetDescription()
        {
            return profile?.description ??
                   "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }

        public static string GetSubscriber()
        {
            return profile.subscriber;
        }

        public static string GetSubscriptions()
        {
            return profile.subscriptions_count ?? "0";
        }

        public static string GetSubscribers()
        {
            return profile?.subscribers_count ?? "0";
        }

        public static string GetLikes()
        {
            return profile?.users_like_count ?? "0";
        }

        public static string GetPublish()
        {
            return profile?.posts_count ?? "0";
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

        public static Task<Profile> SetAppendFriendAsync()
        {
            var items = RequestRouter<Profile, Subscription>.PostAsync("user/add",
                new Subscription { friend_id = GetId() });
            return items;
        }

        public static Task<Profile> SetRemoveFriendAsync()
        {
            var items = RequestRouter<Profile, Subscription>.PostAsync("user/remove",
                new Subscription { friend_id = GetId() });
            return items;
        }

        public static string GetFacebook()
        {
            return profile?.facebook;
        }

        public static string GetTwitter()
        {
            return profile?.twitter;
        }

        public static string GetGithub()
        {
            return profile?.github;
        }

        public static string GetVK()
        {
            return profile?.vk;
        }
    }
}