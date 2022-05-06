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
            return prefix.StartsWith("/") && !prefix.EndsWith("/") && prefix.Contains("api");
        }

        public bool ValidateHost()
        {
            return Uri.TryCreate(host, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public HostBuilder SetHost()
        {
            host = SettingsService.Get().Host;
            switch (host)
            {
                case null:
                    host = default_host;
                    Router.SetDomain(host);
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
                    prefix = default_prefix;
                    Router.SetDomainApi(host + prefix);
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