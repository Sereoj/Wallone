using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using Wallone.Core.Builders;
using Parameter = Wallone.Core.Models.Parameter;

namespace Wallone.Core.Services
{
    public class RequestRouter<T> : Router
    {

        public static async Task<T> GetAsync(string method, string page, List<Parameter> parameters)
        {

            var token = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .GetToken();

            var client = new RestClient($"{domainApi}/{method}");
            
            if (token != null)
            {
                client.Authenticator = new JwtAuthenticator(token);
            }

            var request = new RestRequest(page, Method.GET, DataFormat.Json);
            if (parameters != null)
                foreach (var parameter in parameters)
                    request.AddParameter(parameter.Name, parameter.Value);
            var result = await client.ExecuteGetAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }

        public static async Task<T> GetAsync(string method, string page)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}", Method.GET, DataFormat.Json);
            var result = await client.ExecuteGetAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }

        public static async Task<T> GetAsync(string method)
        {
            var token = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .GetToken();

            var client = new RestClient($"{domainApi}");
            
            if (token != null)
            {
                client.Authenticator = new JwtAuthenticator(token);
            }
            
            var request = new RestRequest($"{method}", Method.GET, DataFormat.Json);

            var result = await client.ExecuteGetAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }
    }

    public class RequestRouter<T, T2> : Router
    {
        public static async Task<T> PostAsync(string method, T2 model, List<Parameter> parameters = null)
        {
            var token = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .GetToken();

            var client = new RestClient(domainApi);
            
            if (token != null)
            {
                client.Authenticator = new JwtAuthenticator(token);
            }

            var request = new RestRequest($"{method}", Method.POST, DataFormat.Json);
            request.AddBody(model);

            if (parameters != null)
                foreach (var item in parameters)
                    switch (item.Type)
                    {
                        case "file":
                            request.AddHeader("Content-Type", "multipart/form-data");
                            request.AddFile(item.Name, item.Value);
                            break;
                    }

            var result = await client.ExecutePostAsync<T>(request);
            AppEthernetService.SetStatus(result.StatusCode);
            return result.Data;
        }
    }
}