using System;
using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    public class AppUpdaterBuilder : IAppSettings
    {
        public int Compare(string verionCurrent, string verionActual)
        {
            if (verionActual != null)
                return string.Compare(verionActual, verionCurrent, StringComparison.Ordinal);
            else
                return -1;
        }
    }
}