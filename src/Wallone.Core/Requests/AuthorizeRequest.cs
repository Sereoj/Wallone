using System.Threading.Tasks;
using Wallone.Core.Models;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Requests
{
    public class AuthorizeRequest
    {
        public static Task<string> GetLoginWithTokenAsync()
        {
            return RequestRouter<string>.GetWithTokenAsync(Pages.Token, null, null);
        }
        public static Task<string> GetLogoutAsync()
        {
            return RequestRouter<string, string>.PostWithTokenAsync(Pages.Logout, null, null);
        }

        public static Task<string> GetLoginAsync(string email, string password)
        {
            return RequestRouter<string, Login>.PostAsync(Pages.Login, new Login { email = email, password = password });
        }

        public static Task<string> GetRegisterAsync(string name, string email, string password,
            string password_confirmation)
        {
            return RequestRouter<string, Register>.PostAsync(Pages.Register,
                new Register
                {
                    username = name,
                    email = email,
                    password = password,
                    password_confirmation = password_confirmation
                });
        }
    }
}