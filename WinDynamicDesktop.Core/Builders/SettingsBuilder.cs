using System;
using System.Collections.Generic;
using System.Text;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class SettingsBuilder
    {
        public SettingsBuilder CreateFile(string path)
        {
            SettingsService.CreateFile(path);
            return this;
        }
        public SettingsBuilder UpdateOrCreateFile(string path)
        {
            if(!SettingsService.CheckFirstLaunch())
            {
                CreateFile(path);
            }
            SettingsService.Save();
            return this;
        }

        public SettingsBuilder Build()
        {
            SettingsService.Save();
            return this;
        }

        public Core.Models.App.Settings Get()
        {
            return SettingsService.Get();
        }
    }
}
