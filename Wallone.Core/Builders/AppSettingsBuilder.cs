namespace Wallone.Core.Builders
{
    public class AppSettingsBuilder : IAppSettings
    {
        private IAppSettings appSettings;

        public AppSettingsBuilder Query(IAppSettings interfaces)
        {
            appSettings = interfaces;
            return this;
        }
    }
}