using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wallone.Core.Models;

namespace Wallone.Core.Services
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

        public static void Close()
        {
            token = null;
        }

        internal static Task<string> GetLoginWithTokenAsync()
        {
            var items = RequestRouter<string>.GetAsync("user", null, null);
            return items;
        }

        internal static bool ValidateWithToken(JObject data)
        {
            if (data["id"] != null && data["name"] != null)
            {
                user.id = data["id"].ToString();
                user.name = data["name"].ToString();
                return true;
            }

            return false;
        }

        public static Task<string> GetLoginAsync(string email, string password)
        {
            var items = RequestRouter<string, Login>.PostAsync("login", new Login {email = email, password = password});
            return items;
        }

        public static Task<string> GetRegisterAsync(string name, string email, string password,
            string password_confirmation)
        {
            var items = RequestRouter<string, Register>.PostAsync("register",
                new Register
                    {name = name, email = email, password = password, password_confirmation = password_confirmation});
            return items;
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
                foreach (var item in objects)
                    msg += item.Value[0] + " ";
            return msg;
        }

        public static string ValidateRegister(JObject objects)
        {
            return Validate(objects);
        }

        public static string ValidateLogin(JObject objects)
        {
            return objects["auth.failed"] != null ? objects["auth.failed"].ToString() : Validate(objects);
        }
    }
}