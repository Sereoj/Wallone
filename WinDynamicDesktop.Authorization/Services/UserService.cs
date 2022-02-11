using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinDynamicDesktop.Authorization.Models;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.Services
{
    public class UserService
    {
        private static readonly User user = new User();
        private static string token;

        public static string GetId()
        {
            return user.id;
        }

        public static string GetUsername()
        {
            return user.name;
        }
        public static string GetToken()
        {
            return token;
        }
        public static Task<string> GetLoginAsync(string email, string password)
        {
            var items = RequestRouter<string, Login>.PostAsync("login", new Login() { email = email, password = password });
            return items;
        }

        public static Task<string> GetLoginWithTokenAsync()
        {
            var items = RequestRouter<string>.GetAsync("user", null, null);
            return items;
        }

        public static Task<string> GetRegisterAsync(string name, string email, string password, string password_confirmation)
        {
            var items = RequestRouter<string, Register>.PostAsync("register", new Register() { name = name, email = email, password = password, password_confirmation = password_confirmation });
            return items;
        }
        public static string ValidateRegister(JObject objects)
        {
            return Validate(objects);
        }
        public static string ValidateLogin(JObject objects)
        {
            return objects["auth.failed"] != null ? objects["auth.failed"].ToString() : Validate(objects);
        }

        public static string ValidateWithToken(JObject objects)
        {
            if(objects["id"] != null)
            {
                user.id = objects["id"].ToString();
                user.name = objects["name"].ToString();
                return "success";
            }
            return null;
        }
        private static string Validate(JObject objects)
        {
            if (objects["token"] != null)
            {
                user.id = objects["id"].ToString();
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
