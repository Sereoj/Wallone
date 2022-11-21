using System.Threading.Tasks;
using Wallone.Core.Models;
using Wallone.Core.Services.Pages;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Requests
{
    public class UserProfileRequest
    {
        public static Task<string> GetUserProfileAsync(string pageId)
        {
            return RequestRouter<string>.GetAsync($"user/{pageId}/info", null, null);
        }

        public static Task<string> GetUserProfileWithTokenAsync(string pageId)
        {
            return RequestRouter<string>.GetWithTokenAsync($"user/{pageId}/info", null, null);
        }

        public static Task<Profile> SetAppendFriendAsync()
        {
            return RequestRouter<Profile, Subscription>.PostAsync("user/add",
                new Subscription { friend_id = ProfileService.GetId() });
        }

        public static Task<Profile> SetRemoveFriendAsync()
        {
            return RequestRouter<Profile, Subscription>.PostAsync("user/remove",
                new Subscription { friend_id = ProfileService.GetId() });
        }
    }
}