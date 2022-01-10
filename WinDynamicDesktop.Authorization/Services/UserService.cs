using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using WinDynamicDesktop.Authorization.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.Services
{
    public class UserService
    {
        private static string token;
        public static Task<string> GetLoginAsync(string email, string password)
        {
            var items = RequestRouter<string, Login>.PostAsync("login", new Login() { email = email, password = password });
            return items;
        }

        public static Task<string> GetRegisterAsync(string name, string email, string password, string password_confirmation)
        {
            var items = RequestRouter<string, Register>.PostAsync("register", new Register() { name = name, email = email, password = password, password_confirmation = password_confirmation });
            return items;
        }
        public static string GetToken()
        {
            return token;
        }
        public static string ValidateRegister(JObject objects)
        {
            return validate(objects);
        }
        public static string ValidateLogin(JObject objects)
        {
            if (objects["auth.failed"] != null)
            {
                return objects["auth.failed"].ToString();
            }

            return validate(objects);
        }

        private static string validate(JObject objects)
        {
            if (objects["token"] != null)
            {
                return token = objects["token"].ToString();
            }

            string msg = null;

            if (objects is JObject)
            {
                foreach (var item in objects)
                {
                    msg += item.Value[0] + " ";
                }
            }
            return msg;
        }
    }
}
