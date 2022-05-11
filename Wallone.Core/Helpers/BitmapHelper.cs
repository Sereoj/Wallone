using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Media.Imaging;

namespace Wallone.Core.Helpers
{
    public class BitmapHelper
    {
        private readonly Dictionary<Uri, BitmapImage> images = new Dictionary<Uri, BitmapImage>();

        public BitmapImage this[Uri imageUrl]
        {
            get
            {
                if (images.TryGetValue(imageUrl, out var bitmap)) return bitmap;

                bitmap = CreateBitmapImage(imageUrl);

                images[imageUrl] = bitmap;

                return bitmap;
            }
        }

        private BitmapImage CreateBitmapImage(Uri uri)
        {
            try
            {
                return new BitmapImage(uri);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                throw;
            }
        }

        public void Clear()
        {
            images.Clear();
        }
    }
}