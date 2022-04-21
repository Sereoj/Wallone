using System;

namespace Wallone.Core.Builders
{
    public class AppSettingsBuilder : IAppSettings
    {
        public AppSettingsBuilder Query(IAppSettings TInterface)
        {
            return this;
        }

        public object Query(object p)
        {
            throw new NotImplementedException();
        }
    }
}