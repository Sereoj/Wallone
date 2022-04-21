using System.Collections.Generic;

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
