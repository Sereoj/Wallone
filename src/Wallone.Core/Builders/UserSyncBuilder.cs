using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Wallone.Core.Models.App;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using Wallone.Core.Services.Users;

namespace Wallone.Core.Builders
{
    public class UserSyncBuilder
    {
        private bool isAuth;
        private string token;

        public UserSyncBuilder GetToken()
        {
            token = UserService.GetToken();
            return this;
        }

        private static async Task<JObject> GetUserData()
        {
            var json = await UserService.GetLoginWithTokenAsync();
            if (!string.IsNullOrEmpty(json))
            {
                var objects = JObject.Parse(json);
                return objects;
            }

            return null;
        }

        public async Task<UserSyncBuilder> ValidateAsync()
        {
            if (token != null)
            {
                JObject data = await GetUserData();
                await Task.CompletedTask;
                if (data != null & UserService.ValidateWithToken(data))
                {
                    isAuth = true;
                    return this;
                }
            }
            isAuth = false;
            return this;
        }

        public bool IsUserAuth()
        {
            return isAuth;
        }

        public void Build()
        {
            LoggerService.Log(this, $"isAuth {isAuth}");
        }
    }
}