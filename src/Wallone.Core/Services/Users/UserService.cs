using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wallone.Core.Models;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services.Users
{
    public class UserFactory
    {
        public static User Create()
        {
            return new User();
        }
    }
    public class UserRepository
    {
        private static User user;
        private static string token;

        public static void Create()
        {
            user = UserFactory.Create();
        }
        public static void Remove()
        {
            user = null;
        }
        public static void Load(User userModel)
        {
            if(user != null)
            {
                user = userModel;
            }
            else
            {
                _= LoggerService.LogAsync(typeof(UserRepository), "Модель пользователя не найдена!", Message.Error);
            }
        }
        public static string GetToken()
        {
            return token;
        }
        public static void Close()
        {
            token = null;
        }
        public class Fields
        {
            public static string GetUserId()
            {
                return user.id;
            }
            public static string GetUsername()
            {
                return user.username;
            }
        }

        public class UserService
        {
            internal static Task<string> GetLoginWithTokenAsync()
            {
                var items = RequestRouter<string>.GetAsync("user", null, null);
                return items;
            }

            public static bool ValidateWithToken(JObject data)
            {
                if (data != null)
                    if (data["id"] != null && data["name"] != null)
                    {
                        user.id = data["id"].ToString();
                        user.username = data["name"].ToString();
                        return true;
                    }

                return false;
            }

            public static Task<string> GetLoginAsync(string email, string password)
            {
                var items = RequestRouter<string, Login>.PostAsync(Routers.Pages.Login, new Login { email = email, password = password });
                return items;
            }

            public static Task<string> GetRegisterAsync(string name, string email, string password,
                string password_confirmation)
            {
                var items = RequestRouter<string, Register>.PostAsync(Routers.Pages.Register,
                    new Register
                    {
                        username = name,
                        email = email,
                        password = password,
                        password_confirmation = password_confirmation
                    });
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

                foreach (var item in objects)
                    if (item.Value != null)
                        msg += item.Value[0] + "\n";
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

            public static bool IsUser(string id)
            {
                return user.id == id;
            }
        }
    }
}