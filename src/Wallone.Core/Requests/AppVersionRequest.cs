using System.Threading.Tasks;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Requests
{
    public class AppVersionRequest
    {
        public static Task<string> GetVersionAsync()
        {
            var items = RequestRouter<string>.GetAsync("app/version");
            return items;
        }
    }
}