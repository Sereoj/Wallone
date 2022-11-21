using System.Collections.Generic;
using System.Threading.Tasks;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Requests
{
    public class AccountRequest
    {
        public static Task<string> GetAccountInformationForEditAsync()
        {
            return RequestRouter<string>.GetWithTokenAsync("user/profile", null, null);
        }

        public static Task<string> GetGuideInformationAsync()
        {
            return RequestRouter<string>.GetAsync("app/guides");
        }

        public static Task<User> EditUserAsync(User user, List<Parameter> parameters)
        {
            return RequestRouter<User, User>.PostAsync("user/edit", user, parameters);
        }
    }
}