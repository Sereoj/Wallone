using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    public class AppPathBuilder : IAppSettings
    {
        public AppPathBuilder AppLocation(string path)
        {
            AppSettingsService.SetAppLocation(path);
            return this;
        }

        public AppPathBuilder Build()
        {
            return this;
        }
    }
}