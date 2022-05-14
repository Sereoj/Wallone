using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Wallone.Core.Helpers
{
    public class BitmapHelper
    {
        public static BitmapImage CreateBitmapImage(Uri uri)
        {
            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = uri;
                image.EndInit();

                return image;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            return null;
        }
    }
}