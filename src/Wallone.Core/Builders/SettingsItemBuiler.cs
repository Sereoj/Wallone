using System;
using ModernWpf;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;

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

        public SettingsItemBuilder SetWindowTheme(ModernWpf.ElementTheme theme)
        {
            settings.General.Theme = theme;
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

        public SettingsItemBuilder SetMode(Mode mode)
        {
            settings.Advanced.Type = mode;
            return this;
        }

        public Mode GetMode()
        {
            return settings.Advanced.Type;
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

        public SettingsItemBuilder SetModelWindow(bool value)
        {
            settings.General.IsArcticModal = value;
            return this;
        }

        public string GetEmail()
        {
            return settings.User.Email;
        }

        public SettingsItemBuilder SetLatitude(double value)
        {
            settings.User.Latitude = value;
            return this;
        }


        public SettingsItemBuilder SetLongitude(double value)
        {
            settings.User.Longitude = value;
            return this;
        }

        public SettingsItemBuilder SetAnimation(bool value)
        {
            settings.General.Animation = value;
            return this;
        }

        public bool GetAutoSetImage()
        {
            return settings.General.AutoSetImage;
        }

        public double GetLatitude()
        {
            return settings.User.Latitude;
        }

        public double GetLongitude()
        {
            return settings.User.Longitude;
        }

        public SettingsItemBuilder SetCountry(string locationCountry)
        {
            settings.User.Country = locationCountry;
            return this;
        }

        public bool GetAnimation()
        {
            return settings.General.Animation;
        }

        public SettingsItemBuilder SetCity(string locationCity)
        {
            settings.User.City = locationCity;
            return this;
        }

        public SettingsItemBuilder SetAutorun(bool isRun)
        {
            settings.General.AutoRun = isRun;
            return this;
        }

        public bool GetAutorun()
        {
            return settings.General.AutoRun;
        }

        public SettingsItemBuilder SetGeolocation(bool isGeolocation)
        {
            settings.General.Geolocation = isGeolocation;
            return this;
        }

        public bool GetGeolocation()
        {
            return settings.General.Geolocation;
        }

        public ElementTheme GetWindowTheme()
        {
            return settings.General.Theme;
        }

        public bool GetModelWindow()
        {
            return settings.General.IsArcticModal;
        }

        public SettingsItemBuilder SetAutoSetImage(bool value)
        {
            settings.General.AutoSetImage = value;
            return this;
        }

        public Geolocation GetGeolocationMode()
        {
            return settings.General.GeolocationMode;
        }

        public SettingsItemBuilder SetGeolocationMode(Geolocation value)
        {
            settings.General.GeolocationMode = value;
            return this;
        }

        public bool GetUseCustomResolution()
        {
            return settings.General.UseCustomResolution;
        }

        public SettingsItemBuilder SetUseCustomResolution(bool value)
        {
            settings.General.UseCustomResolution = value;
            return this;
        }

        public SettingsItemBuilder SetResolutionMode(ResolutionMode value)
        {
            settings.General.ResolutionMode = value;
            return this;
        }

        public ResolutionMode GetResolutionMode()
        {
            return settings.General.ResolutionMode;
        }

        public SettingsItemBuilder SetResolutionTemplate(int value)
        {
            settings.General.ResolutionTemplate = value;
            return this;
        }

        public int GetResolutionTemplate()
        {
            return settings.General.ResolutionTemplate;
        }

        public int GetResolutionWidth()
        {
            return settings.General.ResolutionWidth;
        }

        public int GetResolutionHeight()
        {
            return settings.General.ResolutionHeight;
        }

        public SettingsItemBuilder SetResolutionWidth(int value)
        {
            settings.General.ResolutionWidth = value;
            return this;
        }

        public SettingsItemBuilder SetResolutionHeight(int value)
        {
            settings.General.ResolutionHeight = value;
            return this;
        }
    }
}