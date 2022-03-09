﻿using System;
using System.Collections.Generic;
using System.IO;
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

    public interface IAppSettings
    {

    }
    public class AppSettingsBuilder : IAppSettings
    {
        public AppSettingsBuilder Query(IAppSettings TInterface)
        {
            return  this;
        }

        public object Query(object p)
        {
            throw new NotImplementedException();
        }
    }

    public class SettingsBuilder : IAppSettings
    {
        public SettingsBuilder CreateFile(string path)
        {
            SettingsService.SetFile(path);
            SettingsService.Save();
            return this;
        }
        public SettingsBuilder UpdateOrCreateFile(string path)
        {
            SettingsService.SetFile(path); // Установка пути
            if (!SettingsService.Exist()) // Проверка файла
            {
                CreateFile(path); // Создание файла
            }
            SettingsService.Load(); // Загрузка данных

            var settingsPath = Path.Combine(AppSettingsService.GetAppLocation(), path);
            AppSettingsService.SetSettingsLocation(settingsPath);
            return this;
        }

        public SettingsItemBuiler Items()
        {
            return new SettingsItemBuiler();
        }

        public SettingsBuilder Build()
        {
            SettingsService.Save();
            return this;
        }
    }


    public class HostBuilder : IAppSettings
    {
        private static string host;
        private static string prefix;
        public bool ValidatePrefix()
        {
            return prefix.StartsWith("/") && !prefix.EndsWith("/") && prefix.Contains("api");
        }

        public bool ValidateHost()
        {
            return Uri.TryCreate(host, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        public HostBuilder SetHost()
        {
            host = SettingsService.Get().Host;
            switch (host)
            {
                case null:
                    host = "https://wall.w2me.ru";
                    break;
                default:
                    Router.SetDomain(host);
                    break;
            }
            return this;
        }

        public HostBuilder SetPrefix()
        {
            prefix = SettingsService.Get().Prefix;
            switch (prefix)
            {
                case null:
                    prefix = "/public/api";
                    break;
                default:
                    Router.SetDomainApi(host + prefix);
                    break;
            }
            return this;
        }

        public HostBuilder Validate()
        {
            if (!ValidatePrefix())
                prefix = "/public/api";
            if (!ValidateHost())
                host = "https://wall.w2me.ru";
            return this;
        }

        public HostBuilder Build()
        {
            
            SettingsService.Get().Host = host;
            SettingsService.Get().Prefix = prefix;

            SettingsService.Save();
            return this;
        }
    }
}
