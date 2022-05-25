using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Services;
using Wallone.UI.ViewModels;

namespace Wallone.UI
{
    public class MainMenu
    {
        public static ContextMenuStrip GetMenu()
        {
            List<ToolStripItem> menuItems = GetMenuItems();

            ContextMenuStrip menuStrip = new ContextMenuStrip();
            menuStrip.Items.Clear();
            
            menuStrip.Items.AddRange(menuItems.ToArray());
            return menuStrip;
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


            var autorun = new ToolStripMenuItem("Автозапуск", null, OnAutorun);
            items.AddRange(new List<ToolStripItem>
            {
                autorun,
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

        }

        private static void OnEnableTheme(object sender, EventArgs e)
        {

        }

        private static void OnUpdateImage(object sender, EventArgs e)
        {

        }

        private static void OnSelect(object sender, EventArgs e)
        {

        }
    }
}