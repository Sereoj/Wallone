using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinDynamicDesktop.Core.Services
{
    public class Pictore
    {
        public static BitmapImage SetPictore(Uri Uri)
        {
            BitmapImage image = new BitmapImage(Uri);
            return image.Width != 0 ? image : null;
        }
    }
}
