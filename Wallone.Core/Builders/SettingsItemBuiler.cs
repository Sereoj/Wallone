using Wallone.Core.Interfaces;

namespace Wallone.Core.Builders
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
}