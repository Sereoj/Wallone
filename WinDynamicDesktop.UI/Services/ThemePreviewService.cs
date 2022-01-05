using System.Collections.Generic;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.UI.Services
{
    public class ThemePreviewService
    {
        public static bool CheckItems(List<Images> images)
        {
            return images.Count > 0;
        }
    }
}
