using System;
using Wallone.Core.Services;
using Wallone.Core.Services.Loggers;

namespace Wallone.Core.Builders
{
    public class AppUpdaterBuilder : IAppSettings
    {
        public int Compare(string verionCurrent, string verionActual)
        {
            if (verionActual == null) return -1;
            LoggerService.Log(this, $"verionCurrent {verionCurrent}, verionActual {verionActual}");
            return string.Compare(verionActual, verionCurrent, StringComparison.Ordinal);
        }
    }
}