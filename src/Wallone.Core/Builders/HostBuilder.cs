using System;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Builders
{
    public class HostBuilder : IAppSettings
    {
        private const string default_prefix = "/api/v1";
        private const string default_host = "https://dev.w2me.ru";

        private static string host;
        private static string prefix;

        public bool ValidatePrefix()
        {
            if(prefix != null)
                return prefix.StartsWith("/") && !prefix.EndsWith("/") && prefix.Contains("api");
            return false;
        }

        public bool ValidateHost()
        {
            if (host != null)
                return Uri.TryCreate(host, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return false;
        }

        public HostBuilder SetHost()
        {
            var valueHost = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetHost();

            switch (valueHost)
            {
                case null:
                    host = default_host;
                    Router.SetDomain(host);
                    break;
                default:
                    host = valueHost;
                    Router.SetDomain(host);
                    break;
            }
            LoggerService.Log(this, valueHost);
            return this;
        }

        public HostBuilder SetPrefix()
        {
            var valuePrefix = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .GetPrefix();

            switch (valuePrefix)
            {
                case null:
                    prefix = default_prefix;
                    Router.SetDomainApi(host + prefix);
                    break;
                default:
                    prefix = valuePrefix;
                    Router.SetDomainApi(host + prefix);
                    break;
            }
            LoggerService.Log(this, valuePrefix);
            return this;
        }

        public HostBuilder Validate()
        {
            if (!ValidatePrefix())
                prefix = default_prefix;
            if (!ValidateHost())
                host = default_host;

            LoggerService.Log(this, $"Валидация сервера");
            LoggerService.Log(this, $"Host: {ValidateHost()} Prefix: {ValidatePrefix()}");
            return this;
        }

        public HostBuilder Build()
        {
            new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder()
                .SetHost(host)
                .SetPrefix(prefix)
                .Build();

            return this;
        }
    }
}