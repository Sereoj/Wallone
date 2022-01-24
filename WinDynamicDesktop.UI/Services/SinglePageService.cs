using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.Services
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
            return simplePage?.description ?? "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";
        }

        public static string GetUsername()
        {
            return simplePage?.user?.name ?? "Lorem";
        }

        public static string GetData()
        {
            if (simplePage?.created_at != null)
            {
                return "Дата публикации: " + DateTime.Parse(simplePage?.created_at).ToShortDateString();
            }
            return "Дата публикации: 01/01/2021";
        }
        public static Brand GetBrand()
        {
            return simplePage?.brand;
        }

        public static List<Category> GetCategories()
        {
            return simplePage?.category;
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
        public static string GetReaction()
        {
            return simplePage?.reaction ?? "false";
        }
        public static string GetFavorite()
        {
            return simplePage?.favorite ?? "false";
        }
        public static string GetInstall()
        {
            return simplePage?.install ?? "false";
        }

        public static List<Thumb> GetPosts()
        {
            return simplePage?.posts;
        }
        public static Task<string> GetPageAsync(string fields = null)
        {
            var items = RequestRouter<string>.GetAsync("wallpapers/one", fields);
            return items;
        }

        public static Task<SinglePage> SetDownloadAsync(string install)
        {
            var items = RequestRouter<SinglePage, SinglePageUpdate>.PostAsync("wallpapers/one/" + simplePage.id, new SinglePageUpdate() { download = install});
            return items;
        }

        public static Task<SinglePage> SetFavoriteAsync(string favorite)
        {
            var items = RequestRouter<SinglePage, SinglePageUpdate>.PostAsync("wallpapers/one/" + simplePage.id, new SinglePageUpdate() { favorite = favorite });
            return items;
        }

        public static Task<SinglePage> SetReactionAsync(string reaction)
        {
            var items = RequestRouter<SinglePage, SinglePageUpdate>.PostAsync("wallpapers/one/" + simplePage.id, new SinglePageUpdate() { reaction = reaction });
            return items;
        }
    }
}
