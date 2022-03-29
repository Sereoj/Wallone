using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class AppPathBulder : IAppSettings
    {
        public AppPathBulder AppLocation(string path)
        {
            AppSettingsService.SetAppLocation(path);
            return this;
        }

        public AppPathBulder Build()
        {
            return this;
        }
    }
}
