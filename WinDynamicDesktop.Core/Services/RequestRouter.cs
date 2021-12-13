using RestSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.Core.Services
{
    public class RequestRouter
    {
        private static string domain = "https://wall.w2me.ru/api/";
        public static IRestResponse Get(string method, string fields = null)
        {
            var client = new RestClient(domain);
            var request = new RestRequest($"{method}/{fields}", DataFormat.Json);
            return client.Get(request);
        }
    }
}
