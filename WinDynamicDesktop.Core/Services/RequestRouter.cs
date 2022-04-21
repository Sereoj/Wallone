using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinDynamicDesktop.Core.Services
{
    public class RequestRouter<T> : Router
    {
        public static async Task<T> GetAsync(string method, string page, List<Models.Parameter> parameters)
        {
            RestClient client = new RestClient($"{domainApi}/{method}")
            {
                Authenticator = new JwtAuthenticator(SettingsService.Get().Token)
            };
            var request = new RestRequest(page);
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    request.AddParameter(parameter.Name, parameter.Value);
                }
            }
            var result = await client.ExecuteGetAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }

        public static async Task<T> GetAsync(string method, string page = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}", DataFormat.Json);
            var result = await client.ExecuteGetAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }
    }

    public class RequestRouter<T, T2> : Router
    {
        public static async Task<T> PostAsync(string method, T2 model, List<Models.Parameter> parameters = null)
        {

            /* Необъединенное слияние из проекта "WinDynamicDesktop.Core (net5.0-windows10.0.18362)"
            До:
                        var client = new RestClient(domainApi);

                        if(SettingsService.Get().Token != null)
            После:
                        var client = new RestClient(domainApi);

                        if(SettingsService.Get().Token != null)
            */
            var client = new RestClient(domainApi);

            if (SettingsService.Get().Token != null)
            {
                client.Authenticator = new JwtAuthenticator(SettingsService.Get().Token);
            }

            var request = new RestRequest($"{method}", DataFormat.Json);
            request.AddBody(model);

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    switch (item.Type)
                    {
                        case "file":
                            request.AddHeader("Content-Type", "multipart/form-data");
                            request.AddFile(item.Name, item.Value);
                            break;
                    }
                }
            }

            var result = await client.ExecutePostAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }
    }

}
