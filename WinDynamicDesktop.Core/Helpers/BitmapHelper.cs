using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Windows.Media.Imaging;

namespace WinDynamicDesktop.Core.Helpers
{
    public class BitmapHelper
    {
        private readonly Dictionary<Uri, BitmapImage> images = new Dictionary<Uri, BitmapImage>();

        public BitmapImage this[Uri imageUrl]
        {
            get
            {
                if (images.TryGetValue(imageUrl, out BitmapImage bitmap))
                {
                    return bitmap;
                }

                bitmap = CreateBitmapImage(imageUrl);

                images[imageUrl] = bitmap;

                return bitmap;
            }
        }

        private BitmapImage CreateBitmapImage(Uri uri)
        {
            var bitmap = new BitmapImage();

            bitmap.BeginInit();
            
            var stream = new MemoryStream(new WebClient().DownloadData(uri));
            bitmap.StreamSource = stream;
            
            bitmap.EndInit();
            bitmap.Freeze();

            return bitmap;
        }

        public void Clear()
        {
            images.Clear();
        }
    }
}
