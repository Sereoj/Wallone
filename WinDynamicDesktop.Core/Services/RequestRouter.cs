using RestSharp;
using System.Threading.Tasks;

namespace WinDynamicDesktop.Core.Services
{
    public class RequestRouter<T>
    {
        private static string domain = "https://wall.w2me.ru/api/";
        public static async Task<T> Get(string method, string fields = null)
        {
            var client = new RestClient(domain);
            var request = new RestRequest($"{method}/{fields}", DataFormat.Json);
            var result = await client.GetAsync<T>(request);
            return result;
        }
    }
}
