using Wallone.Core.Interfaces;
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
            settings.Theme = theme;
            return this;
        }

        public SettingsItemBuilder SetEmail(string email)
        {
            settings.Email = email;
            return this;
        }

        public SettingsItemBuilder SetToken(string token)
        {
            settings.Token = token;
            return this;
        }

        public SettingsItemBuilder SetLanguage(string language)
        {
            settings.Language = language;
            return this;
        }

        public SettingsItemBuilder SetWindowTheme(string theme)
        {
            settings.WindowTheme = theme;
            return this;
        }

        public SettingsItemBuilder SetHost(string host)
        {
            settings.Host = host;
            return this;
        }

        public SettingsItemBuilder SetPrefix(string prefix)
        {
            settings.Prefix = prefix;
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
    }
}