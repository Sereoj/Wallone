namespace Wallone.Core.Builders
{
    public class AppConvert
    {
        public static bool StringToBool(string value)
        {
            return value == "true" ? true : false;
        }

        public static string BoolToString(bool value)
        {
            return value ? "true" : "false";
        }

        public static bool Revert(bool value)
        {
            return value != true;
        }
    }
}