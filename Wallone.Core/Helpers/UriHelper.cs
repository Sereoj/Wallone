using System;
using System.IO;
using System.Linq;
using Wallone.Core.Services;

namespace Wallone.Core.Helpers
{
    public class UriHelper
    {
        /**
         * /public/catalog/hello
         * to 
         * http://v3.w2me.ru/public/catalog/hello
         */
        public static Uri Get(string path)
        {
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            return uri.IsAbsoluteUri ? uri : new Uri(Router.domain + uri.OriginalString);
        }

        public static Uri Get(Uri path)
        {
            if (path != null)
                return path.IsAbsoluteUri ? path : new Uri(Router.domain + path.OriginalString);
            return new Uri(Router.domain);
        }

        public static string GetUri(string uri, string path, string separator)
        {
            if (uri == null || path == null || separator == null) return null;

            var item = uri.Split(separator).FirstOrDefault();

            if (string.IsNullOrEmpty(item)) return null;
            var filename = Path.GetFileName(item);

            return Path.Combine(path, filename);
        }

        public static string GetFirstValue(string uri, string separator)
        {
            if (uri == null || separator == null) return null;
            var item = uri.Split(separator).FirstOrDefault();

            return item;
        }
    }
}