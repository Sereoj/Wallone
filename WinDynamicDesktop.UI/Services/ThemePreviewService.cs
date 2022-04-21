using System.Collections.Generic;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.UI.Services
{
    public class ThemePreviewService
    {

        public static bool IsNotNull(List<Images> images)
        {
            return images != null && images.Count > 0;
        }
    }
}
