using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Schedulers;
using Wallone.Core.Services;
using Wallone.UI.ViewModels;
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
                return menuStrip;
            }

            return null;
        }

        private static List<ToolStripItem> GetMenuItems()
        {

            var itemBuilder = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            List<ToolStripItem> items = new List<ToolStripItem>();
            items.Add(new ToolStripLabel("Wallone"));
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
            Autorun.Checked = SettingsService.Get().General.AutoRun;
            items.AddRange(new List<ToolStripItem>
            {
                Autorun,
                new ToolStripMenuItem("Выход", null, OnCloseApplication),
            });
            return items;
        }

        private static void OnCloseApplication(object sender, EventArgs e)
        {
            Application.DoEvents();
            Application.Exit();
        }
        private static void OnAutorun(object sender, EventArgs e)
        {
            Autorun.Checked = !Autorun.Checked;
            SettingsService.Get().General.AutoRun = Autorun.Checked;
            SettingsService.Save();
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