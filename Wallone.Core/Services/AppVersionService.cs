using System.Threading.Tasks;

namespace Wallone.Core.Services
{
    public class AppVersionService
    {
        public static string ActualVersion { get; set; }
        public static string GetCurrentVersion()
        {
            return "1.0";
        }

        public static string GetActualVersion()
        {
            return ActualVersion;
        }
        public static Task<string> GetVersionAsync()
        {
            var items = RequestRouter<string>.GetAsync("app/version");
            return items;
        }

        public static void SetVersion(string version)
        {
            ActualVersion = version ?? "1.0";
        }
    }
}
