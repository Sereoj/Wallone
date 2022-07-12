using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Builders
{
    public class AppPathBuilder : IAppSettings
    {
        public AppPathBuilder AppLocation(string path)
        {
            AppSettingsRepository.AppSettingsService.SetAppDirectoryLocation(path);

            LoggerService.SysLog(this, path);
            return this;
        }

        public AppPathBuilder ApplicationPath(string path)
        {
            AppSettingsRepository.AppSettingsService.SetApplicationPath(path);

            LoggerService.SysLog(this, path);
            return this;
        }

        public AppPathBuilder Build()
        {
            return this;
        }
    }
}