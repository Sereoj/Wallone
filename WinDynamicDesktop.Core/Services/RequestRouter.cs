using RestSharp;
using System.Threading.Tasks;

namespace WinDynamicDesktop.Core.Services
{
    public class Router
    {
        //public static string domainApi = "https://wall.w2me.ru/public/api"; 
        public static string domainApi = "http://v3.w2me.ru/public/api";
        public static string domain = "http://v3.w2me.ru";
    }

    public class RequestRouter<T> : Router
    {
        public static async Task<T> GetAsync(string method, string fields = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{fields}", DataFormat.Json);
            var result = await client.GetAsync<T>(request);
            return result;
        }
        public static async Task<T> GetAsync(string method, string page, string fields = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}/{fields}", DataFormat.Json);
            var result = await client.GetAsync<T>(request);
            return result;
        }
        public static IRestResponse Get(string method, string fields = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{fields}", DataFormat.Json);
            var result = client.Get(request);
            return result;
        }

        public static IRestResponse Get(string method, string page, string fields = null)
        {
            var client = new RestClient(domainApi);
            var request = new RestRequest($"{method}/{page}/{fields}", DataFormat.Json);
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
    }

}
