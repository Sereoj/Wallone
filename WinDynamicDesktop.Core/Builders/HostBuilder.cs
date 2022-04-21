using System;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class HostBuilder : IAppSettings
    {
        const string default_prefix = "/api/v1";
        const string default_host = "https://wall.w2me.ru";

        private static string host;
        private static string prefix;
        public bool ValidatePrefix() => prefix.StartsWith("/") && !prefix.EndsWith("/") && prefix.Contains("api");

        public bool ValidateHost() => Uri.TryCreate(host, UriKind.Absolute, out Uri uriResult)
                                      && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
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

            SettingsService.Get().Host = host;
            SettingsService.Get().Prefix = prefix;

            SettingsService.Save();
            return this;
        }
    }
}