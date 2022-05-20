using System;
using Wallone.Core.Services;

namespace Wallone.Core.Builders
{
    public class HostBuilder : IAppSettings
    {
        private const string default_prefix = "/api/v1";
        private const string default_host = "https://wall.w2me.ru";

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
            var valueHost = new SettingsBuilder(SettingsService.Get())
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

            return this;
        }

        public HostBuilder SetPrefix()
        {
            var valuePrefix = new SettingsBuilder(SettingsService.Get())
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

            return this;
        }

        public HostBuilder Validate()
        {
            if (!ValidatePrefix())
                prefix = default_prefix;
            if (!ValidateHost())
                host = default_host;
            return this;
        }

        public HostBuilder Build()
        {
            new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .SetHost(host)
                .SetPrefix(prefix)
                .Build();

            return this;
        }
    }
}