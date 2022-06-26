using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Wallone.Core.Builders;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Helpers
{

    public abstract class PlatformHelper
    {
        public abstract string GetCurrentFolder();

        public abstract void SwitcherAutorun(string path, bool autorunChecked);
        public abstract bool CheckAutorun();
        public abstract void OpenUpdateLink();
    }

    public class ClassicPlatformHelper : PlatformHelper
    {
        private string RegistryStartupLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";
        public override string GetCurrentFolder()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public override void SwitcherAutorun(string path, bool autorunChecked)
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(RegistryStartupLocation, true);

            var settings = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            if (startupKey == null) return;

            if (autorunChecked)
            {
                startupKey.SetValue(Common.Translation.Localization.AppName, path);
                settings.SetAutorun(true);
            }
            else
            {
                if (CheckAutorun())
                {
                    startupKey.DeleteValue(Common.Translation.Localization.AppName);
                    settings.SetAutorun(false);
                }
            }
            

            settings.Build();
        }

        public override bool CheckAutorun()
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(RegistryStartupLocation);
            var isChecked = startupKey.GetValue(Common.Translation.Localization.AppName) != null; // true
            startupKey.Close();
            return isChecked;
        }

        public override void OpenUpdateLink()
        {
            System.Diagnostics.Process.Start(Router.domain + "/update");
        }
    }

    public class UwpPlatformHelper : PlatformHelper
    {
        public override string GetCurrentFolder()
        {
            return Windows.Storage.ApplicationData.Current.LocalFolder.Path;
        }

        public override void SwitcherAutorun(string path, bool autorunChecked)
        {
            if (CheckAutorun())
            {

            }
        }

        public override bool CheckAutorun()
        {
            return false;
        }

        public override void OpenUpdateLink()
        {
            Task.Run(() => Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://downloadsandupdates")));
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