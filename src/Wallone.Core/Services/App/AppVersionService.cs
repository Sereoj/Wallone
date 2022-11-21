using System.Threading.Tasks;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services.App
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

        public static void SetVersion(string version)
        {
            ActualVersion = version ?? "1.0";
        }
    }
}