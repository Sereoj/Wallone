using System.IO;

namespace Wallone.Core.Helpers
{

    public abstract class PlatformHelper
    {
        public abstract string GetCurrentFolder();
    }

    public class ClassicPlatformHelper : PlatformHelper
    {
        public override string GetCurrentFolder()
        {
            return Directory.GetCurrentDirectory();
        }
    }

    public class UwpPlatformHelper : PlatformHelper
    {
        public override string GetCurrentFolder()
        {
            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        }
    }
    public class Platformer
    {
        private static PlatformHelper helper;
        private static bool? isRunningAsUwp;

        public static bool IsSupportUwp()
        {
            if (!isRunningAsUwp.HasValue)
            {
                DesktopBridge.Helpers helpers = new DesktopBridge.Helpers();
                isRunningAsUwp = helpers.IsRunningAsUwp();
            }

            return isRunningAsUwp.Value;
        }

        public static PlatformHelper GetHelper()
        {
            if (IsSupportUwp())
            {
                helper = new UwpPlatformHelper();
                return helper;
            }
            helper = new ClassicPlatformHelper();
            return helper;
        }
    }
}