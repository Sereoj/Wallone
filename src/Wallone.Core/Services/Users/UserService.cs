using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallone.Core.Models;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Routers;

namespace Wallone.Core.Services.Users
{
    public class UserTokenble
    {
        public string id { get; set; }
        public string name { get; set; }
        public string token { get; set; }
    }
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

        public static User Get()
        {
            return user;
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

        public static UserTokenble UserTokenble(string json)
        {
            return JsonConvert.DeserializeObject<UserTokenble>(json);
        }
        public static string GetToken(string json)
        {
            var data = UserTokenble(json);
            SetToken(data?.token);
            return data?.token;
        }
        public static string GetToken()
        {
            return token;
        }

        public static void SetToken(string value)
        {
            token = value;
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
            public static Task<string> GetLogoutAsync()
            {
                var items = RequestRouter<string,string>.PostWithTokenAsync(Routers.Pages.Logout, null, null);
                return items;
            }

            public static Task<string> GetLoginWithTokenAsync()
            {
                var items = RequestRouter<string>.GetWithTokenAsync(Routers.Pages.Token, null, null);
                return items;
            }

            public static bool ValidateWithToken(string data)
            {
                if (data != null)
                {
                    var user = UserTokenble(data);
                    if (user.id != null && user.name != null)
                    {
                        Get().id = user.id;
                        Get().username = user.name;
                        return true;
                    }
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

            public static bool IsUser(string id)
            {
                return user.id == id;
            }
        }
    }
}