using System;
using System.Collections.Generic;
using System.Text;

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
            Uri uri = new Uri(path);
            return uri.IsAbsoluteUri ? uri : new Uri("http://v3.w2me.ru" + path);
        }

        public static Uri Get(Uri path)
        {
            return path.IsAbsoluteUri ? path : new Uri("http://v3.w2me.ru" + path);
        }
    }
}
