using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;

namespace Wallone.Core.Services
{
    public class SinglePageService
    {
        private static SinglePage simplePage;

        public SinglePageService(SinglePage page)
        {
            Load(page);
        }

        public static void Load(SinglePage page)
        {
            if (page != null)
                simplePage = page;
        }

        public static string GetID()
        {
            return simplePage.id;
        }

        public static SinglePage GetSimplePage()
        {
            return simplePage;
        }

        public static string GetHeader()
        {
            return simplePage?.name ?? "Lorem ipsum";
        }

        public static string GetDescription()
        {
            return simplePage?.description ??
                   "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }

        public static string GetUsername()
        {
            return simplePage?.user?.username ?? "Lorem";
        }

        public static string GetPreview()
        {
            return simplePage.preview;
        }

        public static string GetAvatar()
        {
            return simplePage?.user?.avatar;
        }

        public static string GetData()
        {
            return simplePage?.created_at != null
                ? "Дата публикации: " + DateTime.Parse(simplePage.created_at).ToShortDateString()
                : "Дата публикации: 01/01/2021";
        }

        public static UserShort GetUser()
        {
            return simplePage.user;
        }

        public static string GetDate()
        {
            return DateTime.Parse(simplePage.created_at).ToShortDateString();
        }

        public static BrandShort GetBrand()
        {
            return simplePage?.brand;
        }

        public static List<CategoryShort> GetCategories()
        {
            return simplePage?.categories;
        }

        public static List<Tag> GetTags()
        {
            return simplePage?.tags;
        }

        public static string GetViews()
        {
            return simplePage?.views ?? "0";
        }

        public static string GetDownloads()
        {
            return simplePage?.downloads ?? "0";
        }

        public static string GetLikes()
        {
            return simplePage?.likes ?? "0";
        }

        public static bool HasReaction()
        {
            return simplePage.hasLike switch
            {
                "true" => true,
                "false" => true,
                null => false,
                _ => false
            };
        }

        public static bool HasFavorite()
        {
            return simplePage.hasFavorite switch
            {
                "true" => true,
                "false" => true,
                null => false,
                _ => false
            };
        }

        public static bool GetReaction()
        {
            return simplePage?.hasLike == "true";
        }

        public static bool GetFavorite()
        {
            return simplePage?.hasFavorite == "true";
        }

        public static List<Thumb> GetPosts()
        {
            return simplePage?.posts;
        }

        public static Task<string> GetPageAsync(string fields = null)
        {
            var items = RequestRouter<string>.GetAsync("wallpapers/one/" + fields + "/show", null, null);
            return items;
        }

        public static Task<string> GetPageAdsAsync()
        {
            var items = RequestRouter<string>.GetAsync("app/ads");
            return items;
        }

        public static Task<SinglePage> SetDownloadAsync(string value)
        {
            var items = RequestRouter<SinglePage, SinglePageUpdate>.PostAsync("wallpapers/one/" + simplePage.id,
                new SinglePageUpdate {hasDownload = value});
            return items;
        }

        public static Task<SinglePage> SetFavoriteAsync(string value)
        {
            var items = RequestRouter<SinglePage, SinglePageUpdate>.PostAsync("wallpapers/one/" + simplePage.id,
                new SinglePageUpdate {hasFavorite = value});
            return items;
        }

        public static Task<SinglePage> SetReactionAsync(string value)
        {
            var items = RequestRouter<SinglePage, SinglePageUpdate>.PostAsync("wallpapers/one/" + simplePage.id,
                new SinglePageUpdate {hasLike = value});
            return items;
        }
    }
}