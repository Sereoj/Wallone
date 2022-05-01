using System;
using System.Collections.Generic;

namespace Wallone.Core.Builders
{
    public class AppSettingsBuilder : IAppSettings
    {
        public AppSettingsBuilder Query(IAppSettings interfaces)
        {
            return this;
        }
    }
}