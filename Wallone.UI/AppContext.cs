using System;
using System.Windows;
using System.Windows.Forms;
using Wallone.UI.Views;
using Application = System.Windows.Forms.Application;

namespace Wallone.UI
{
    public class AppContext : ApplicationContext
    {
        private static NotifyIcon notifyIcon;
        private static MainWindow main;

        public AppContext(MainWindow mainWindow)
        {
            if (mainWindow != null)
            {
                main = mainWindow;
            }

            InitTrayIcon();
        }

        private void InitTrayIcon()
        {
            Application.ApplicationExit += Application_ApplicationExit;

            notifyIcon = new System.Windows.Forms.NotifyIcon()
            {
                Text = "Wallone",
                Visible = true,
                Icon = Properties.Resources.favicon
            };
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                main.Topmost = true;
                main.WindowState = WindowState.Normal;
                main.ShowInTaskbar = true;
                main.Topmost = false;
            }

            if (e.Button == MouseButtons.Right)
            {
                notifyIcon.ContextMenuStrip = MainMenu.GetMenu(main);
            }
        }

        public static void ShowPopup(string message, string title = null)
        {
            notifyIcon.BalloonTipTitle = title ?? "Wallone";
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(10000);
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            Close();
        }

        public static void Show(bool b)
        {
            notifyIcon.Visible = b;
        }

        public static void Close()
        {
            notifyIcon.Dispose();
            Application.Exit();
        }
    }
}