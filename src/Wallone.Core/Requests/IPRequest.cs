using System.Threading.Tasks;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Requests
{
    public class IPRequest
    {
        public static Task<string> GetFromEthernet()
        {
            var items = RequestRouter<string>.GetAsync("app/ip");
            return items;
        }
    }
}