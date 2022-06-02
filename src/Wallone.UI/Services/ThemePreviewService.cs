using System.Collections.Generic;
using Wallone.Core.Models;

namespace Wallone.UI.Services
{
    public class ThemePreviewService
    {
        public static bool IsNotNull(List<Images> images)
        {
            return images != null && images.Count > 0;
        }
    }
}