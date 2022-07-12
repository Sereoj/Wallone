using Wallone.Core.Services;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Builders
{
    public class AppLoggerBuilder : IAppSettings
    {
        public AppLoggerBuilder SetFileName(string appLog)
        {
            LoggerService.SetFileName(appLog);
            return this;
        }

        public AppLoggerBuilder Activate()
        {
            LoggerService.Activate();
            return this;
        }

        public AppLoggerBuilder NewLine()
        {
            LoggerService.SysLog(null, null);
            return this;
        }

        public AppLoggerBuilder StartLine()
        {
            LoggerService.SysLog(null, $"Запуск приложения Wallone");
            return this;
        }
    }
}