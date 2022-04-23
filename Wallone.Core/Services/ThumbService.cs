using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Wallone.Core.Helpers;
using Wallone.Core.Models;

namespace Wallone.Core.Services
{
    public class ThumbService
    {
        public static HttpStatusCode GetStatusCode()
        {
            return AppEthernetService.GetStatus();
        }

        public static Task<List<Thumb>> GetThumbsAsync(string router, string page, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync(router, page, parameters);
            return items;
        }

        public static Task<List<Thumb>> GetThumbsFavoriteAsync(string page, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers/favorite", page, parameters);
            return items;
        }

        public static Task<List<Thumb>> GetThumbsInstallAsync(string page, List<Parameter> parameters)
        {
            var items = RequestRouter<List<Thumb>>.GetAsync("wallpapers/install", page, parameters);
            return items;
        }

        public static bool CheckItems(List<Thumb> items)
        {
            return items != null;
        }

        public static bool IsNotNull(List<Thumb> items)
        {
            return items != null;
        }

        public static bool IsIDNotNull(string ID)
        {
            return ID != null;
        }

        public static string ValidateName(string name)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
        }

        public static string ValidateViews(string views)
        {
            return views ?? "0";

        }
        public static string ValidateDownloads(string downloads)
        {
            return downloads ?? "0";
        }

        public static BitmapImage ValidatePreview(Uri uri)
        {
            return uri == null
                ? new BitmapImage(UriHelper.Get("pack://application:,,,/Wallone.Common;component/Images/Placeholder.png"))
                : new BitmapImage(uri);
        }
    }
}