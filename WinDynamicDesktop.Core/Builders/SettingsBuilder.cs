﻿using System;
using System.Collections.Generic;
using System.Text;
using WinDynamicDesktop.Core.Interfaces;
using WinDynamicDesktop.Core.Models.App;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class SettingsItemBuiler
    {
        private ISettings settings;
        public SettingsItemBuiler LoadModel(ISettings settings)
        {
            this.settings = settings;
            return this;
        }
        public SettingsItemBuiler SetEmail(string email)
        {
            settings.Email = email;
            return this;
        }

        public SettingsItemBuiler SetToken(string token)
        {
            settings.Token = token;
            return this;
        }

        public SettingsItemBuiler SetLanguage(string language)
        {
            settings.Language = language;
            return this;
        }

        public SettingsItemBuiler SetTheme(string theme)
        {
            settings.Theme = theme;
            return this;
        }

        public SettingsItemBuiler Get()
        {
            return this;
        }
    }

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
