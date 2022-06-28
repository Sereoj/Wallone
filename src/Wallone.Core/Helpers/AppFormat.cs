using System;

namespace Wallone.Core.Helpers
{
    public class AppFormat
    {
        public static string Format(string current)
        {
            return current;
        }

        public static bool Compare(string settingThemeName, string singleThemeName)
        {
            return string.Equals(Format(settingThemeName), Format(singleThemeName),
                StringComparison.CurrentCultureIgnoreCase);
        }
    }
}