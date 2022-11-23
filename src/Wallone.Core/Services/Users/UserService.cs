using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wallone.Core.Helpers;
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
            return Json<UserTokenble>.Decode(json);
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
            public static void SetUserId(string value)
            {
                user.id = value;
            }
            public static string GetUserId()
            {
                return user.id;
            }

            public static void SetUsername(string value)
            {
                user.username = value;
            }
            public static string GetUsername()
            {
                return user.username;
            }
        }

        public class UserService
        {
            public static bool ValidateWithToken(string data)
            {
                if (data != null)
                {
                    var user = UserTokenble(data);
                    string userId = user.id;
                    string username = user.name;

                    if (!string.IsNullOrEmpty(user.id) && !string.IsNullOrEmpty(user.name))
                    {
                        Fields.SetUserId(userId);
                        Fields.SetUsername(username);
                        return true;
                    }
                }

                return false;
            }

            public static bool IsUser(string id)
            {
                return Fields.GetUserId() == id;
            }
        }
    }
}