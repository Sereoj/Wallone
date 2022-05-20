﻿using Wallone.Core.Interfaces;
using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    public class SettingsItemBuilder
    {
        private readonly ISettings settings;

        public SettingsItemBuilder(ISettings settings)
        {
            if (settings != null) this.settings = settings;
        }

        public SettingsItemBuilder SetTheme(string theme)
        {
            settings.General.Image = theme;
            return this;
        }

        public SettingsItemBuilder SetEmail(string email)
        {
            settings.User.Email = email;
            return this;
        }

        public SettingsItemBuilder SetToken(string token)
        {
            settings.User.Token = token;
            return this;
        }

        public SettingsItemBuilder SetLanguage(string language)
        {
            settings.User.Language = language;
            return this;
        }

        public SettingsItemBuilder SetWindowTheme(string theme)
        {
            settings.General.Image = theme;
            return this;
        }

        public SettingsItemBuilder SetHost(string host)
        {
            settings.Server.Host = host;
            return this;
        }

        public SettingsItemBuilder SetPrefix(string prefix)
        {
            settings.Server.Prefix = prefix;
            return this;
        }

        public ISettings Get()
        {
            return settings;
        }


        public void Build()
        {
            SettingsService.SetModel(settings);
            SettingsService.Save();
        }

        public string GetToken()
        {
            return settings.User.Token;
        }

        public string GetImage()
        {
            return settings.General.Image;
        }

        public string GetHost()
        {
            return settings.Server.Host;
        }

        public string GetPrefix()
        {
            return settings.Server.Prefix;
        }

        public string GetEmail()
        {
            return settings.User.Email;
        }
    }
}