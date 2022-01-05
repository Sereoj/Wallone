using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class Router
    {
        public static string domainApi = "https://wall.w2me.ru/public/api";
        public static string domain = "https://wall.w2me.ru";
        //public static string domainApi = "http://v3.w2me.ru/public/api";
        //public static string domain = "http://v3.w2me.ru";
    }

    public class RequestRouter<T> : Router
    {
        public static async Task<T> GetAsync(string method, string page, List<Models.Parameter> parametrs)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}", DataFormat.Json);
            if (parametrs != null)
            {
                foreach (var parametr in parametrs)
                {
                    request.AddParameter(parametr.Name, parametr.Value);
                }
            }
            var result = await client.GetAsync<T>(request);
            return result;
        }
        public static async Task<T> GetAsync(string method, string page = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}", DataFormat.Json);
            var result = await client.GetAsync<T>(request);
            return result;
        }
        public static IRestResponse Get(string method, string page = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}", DataFormat.Json);
            var result = client.Get(request);
            return result;
        }
    }

    public class RequestRouter<T, T2> : Router
    {
        public static async Task<T> PostAsync(string method, T2 model)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}", DataFormat.Json).AddBody(model);
            var result = await client.PostAsync<T>(request);
            return result;
        }
        public static IRestResponse Get(string method, string page, T2 fields)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}", DataFormat.Json).AddJsonBody(fields);
            var result = client.Get(request);
            return result;
        }
    }

}
