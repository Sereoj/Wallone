using System;

namespace Wallone.Core.Helpers
{
    public class OS
    {
        public PlatformID PlatformID { get; set; }
        public Version Version { get; set; }
    }
    public static class OSHelper
    {
        public static string IsWindows()
        {
            return Get().PlatformID == PlatformID.Win32NT ? "Windows" : null;
        }
        public static OS Get()
        {
           return Build(Environment.OSVersion.Platform, Environment.OSVersion.Version);
        }

        private static OS Build(PlatformID pID, Version ver)
        {
            OperatingSystem opSys = new OperatingSystem(pID, ver);
            PlatformID platform = opSys.Platform;
            Version version = opSys.Version;

            return new OS()
            {
                PlatformID = platform,
                Version = version,
            };
        }
    }


}