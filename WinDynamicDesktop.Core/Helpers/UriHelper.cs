using System;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Helpers
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
            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            return uri.IsAbsoluteUri ? uri : new Uri(Router.domain + uri.OriginalString);
        }

        public static Uri Get(Uri path)
        {
            if (path != null)
                return path.IsAbsoluteUri ? path : new Uri(Router.domain + path.OriginalString);
            return new Uri(Router.domain);
        }
    }
}
