using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class Router
    {
        public static string domainExample { get; set; } = "https://example.com";
        public static string domainApi { get; set; }
        public static string domain { get; set; }
        //public static string domainApi = "http://v3.w2me.ru/public/api";
        //public static string domain = "http://v3.w2me.ru";

        public static void SetDomainApi(string value)
        {
            domainApi = value;
        }

        public static void SetDomain(string value)
        {
            domain = value;
        }
        public static string OnlyNameDomainApi()
        {
            return domainApi.Replace("https://", null).Replace("http://", null);
        }

        public static string OnlyNameDomain()
        {
            return domain.Replace("https://", null).Replace("http://", null);
        }
    }

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
            var client = new RestClient(domainApi);
            
            if(SettingsService.Get().Token != null)
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
