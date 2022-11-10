using System;
using Wallone.Core.Models;

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

        public static Times StringToTimes(string value)
        {
            switch (value)
            {
                case "Dawn":
                    return Times.Dawn;
                case "Day":
                    return Times.Day;
                case "GoldenHour":
                    return Times.GoldenHour;
                case "Sunset":
                    return Times.Sunset;
                case "Night":
                    return Times.Night;
                default:
                    return Times.NotFound;
            }
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