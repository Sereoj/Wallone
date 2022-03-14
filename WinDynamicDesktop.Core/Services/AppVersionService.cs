using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models.App;

namespace WinDynamicDesktop.Core.Services
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
            var items = RequestRouter<string>.GetAsync("version");
            return items;
        }

        public static void SetVersion(string version)
        {
            ActualVersion = version ?? "1.0";
        }
    }
}
