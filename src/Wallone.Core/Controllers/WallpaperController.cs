using System;
using System.Runtime.InteropServices;
using System.Threading;
using Wallone.Core.Helpers;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Win32;

namespace Wallone.Core.Controllers
{
    public class WallpaperInstaller
    {
        public class Window
        {
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            public static IntPtr GetWindow(string lpClassName, string lpWindowName)
            {
                return FindWindow(lpClassName, lpWindowName);
            }

            public static IntPtr GetProgman()
            {
                return FindWindow("Progman", null);
            }

        }
        public class Animation
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern int SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, uint fuFlags, uint uTimeout, out IntPtr result);

            public static void EnableTransitions(bool value)
            {
                if(value)
                {
                    IntPtr result = IntPtr.Zero;
                    var message = SendMessageTimeout(Window.GetProgman(), 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 500, out result);

                    if(message == 0)
                    {
                        _ = LoggerService.LogAsync(typeof(Animation), $"Анимация смены обоев не поддерживается в {OSHelper.IsWindows()} {OSHelper.Get().Version}");
                    }
                }
            }
        }
        public class Controller
        {
            public static void Set(string path)
            {
                ThreadStart threadStarter = () =>
                {
                    IActiveDesktop _activeDesktop = ActiveDesktopWrapper.GetActiveDesktop();
                    _activeDesktop.SetWallpaper(path, 1);
                    _activeDesktop.SetWallpaperOptions(ActiveDesktopWrapper.GetWallpaperOpt(WallPaperStyle.WPSTYLE_TILE), 0);
                    _activeDesktop.ApplyChanges(AD_Apply.ALL | AD_Apply.FORCE);

                    Marshal.ReleaseComObject(_activeDesktop);
                };
                Thread thread = new Thread(threadStarter);
                thread.SetApartmentState(ApartmentState.STA);  // Set the thread to STA (required!)
                thread.Start();
                thread.Join(1000);
            }
        }
    }
}
