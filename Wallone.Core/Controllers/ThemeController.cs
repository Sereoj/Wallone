using Wallone.Core.Models;
using Wallone.Core.Services;

namespace Wallone.Core.Controllers
{
    public class ThemeController
    {
        public void Set(Theme theme)
        {
            if (theme != null)
            {
                ThemeService.Set(theme);
                ThemeService.Save();
            }
        }
    }
}