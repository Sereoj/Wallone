using Wallone.Core.Services;

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
            LoggerService.Log(null, null);
            return this;
        }
    }
}