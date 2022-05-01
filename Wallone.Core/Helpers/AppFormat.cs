using System;

namespace Wallone.Core.Helpers
{
    public class AppFormat
    {
        public static string Format(string current)
        {
            return current ?? "default1";
        }

        public static bool Compare(string settingThemeName, string singleThemeName)
        {
            return string.Equals(Format(settingThemeName), Format(singleThemeName),
                StringComparison.CurrentCultureIgnoreCase);
        }
    }
}