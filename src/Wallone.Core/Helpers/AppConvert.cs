using System;

namespace Wallone.Core.Helpers
{
    public class AppConvert
    {
        public static bool StringToBool(string value)
        {
            return string.Equals(value, "true", StringComparison.Ordinal);
        }

        public static string BoolToString(bool value)
        {
            return value ? "true" : "false";
        }

        public static string BoolToStringRevert(bool value)
        {
            return value ? "false" : "true";
        }

        public static bool Revert(bool value)
        {
            return value != true;
        }
    }
}