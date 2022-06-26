using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models.Settings;
using Wallone.Core.Schedulers;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.UI.Properties;
using Wallone.UI.Views;
using Application = System.Windows.Forms.Application;

namespace Wallone.UI
{
    public class MainMenu
    {
        private static MainWindow main;
        public static ToolStripMenuItem Autorun;

        public static ContextMenuStrip GetMenu(Views.MainWindow window)
        {
            if (window != null && SettingsService.Get() != null)
            {
                main = window;

                List<ToolStripItem> menuItems = GetMenuItems();
                ContextMenuStrip menuStrip = new ContextMenuStrip();
                menuStrip.Items.AddRange(menuItems.ToArray());

                SettingsService.Get().General.PropertyChanged += SettingsChanged;

                return menuStrip;
            }

            return null;
        }

        private static void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Autorun.Checked = ((General)sender).AutoRun;
        }

        private static List<ToolStripItem> GetMenuItems()
        {

            var itemBuilder = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            List<ToolStripItem> items = new List<ToolStripItem>();
            items.Add(new ToolStripLabel(Common.Translation.Localization.AppName));
            items[0].Enabled = false;
            
            items.Add(new ToolStripSeparator());

            items.Add(new ToolStripLabel(itemBuilder.GetImage()));
            items[2].Enabled = false;

            items.AddRange(new List<ToolStripItem>()
            {
                new ToolStripMenuItem("Выбрать новые", null, OnSelect),
                new ToolStripMenuItem("Обновить", null, OnUpdateImage),
                new ToolStripSeparator()
            });

            Autorun = new ToolStripMenuItem("Автозапуск", null, OnAutorun);
            Autorun.Checked = Platformer.GetHelper().CheckAutorun();
            items.AddRange(new List<ToolStripItem>
            {
                Autorun,
                new ToolStripMenuItem("Выход", null, OnCloseApplication),
            });
            return items;
        }

        private static void OnCloseApplication(object sender, EventArgs e)
        {
            AppContext.Close();
        }
        private static void OnAutorun(object sender, EventArgs e)
        {
            Autorun.Checked = !Autorun.Checked;

            var path = AppSettingsService.GetApplicationPath();

            if (AppSettingsService.ExistsFile(path))
            {
                Platformer.GetHelper().SwitcherAutorun(path, Autorun.Checked);
            }
        }

        private static void OnUpdateImage(object sender, EventArgs e)
        {
            ThemeScheduler.Stop();
            ThemeScheduler.SetInterval(1000);
            ThemeScheduler.Run();
        }

        private static void OnSelect(object sender, EventArgs e)
        {
            main.Topmost = true;
            main.WindowState = WindowState.Normal;
            main.ShowInTaskbar = true;
            main.Topmost = false;
        }
    }
}