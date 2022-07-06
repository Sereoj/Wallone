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
            _ = LoggerService.LogAsync(null, null);
            return this;
        }

        public AppLoggerBuilder StartLine()
        {
            _ = LoggerService.LogAsync(null, $"Запуск приложения Wallone");
            return this;
        }
    }
}