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

        public UserSyncBuilder CreateUserModel()
        {
            UserRepository.Create();
            return this;
        }

        public UserSyncBuilder GetToken()
        {
            token = UserRepository.GetToken();
            if(string.IsNullOrEmpty(token))
            {
                token = new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder()
                    .GetToken();
            }
            return this;
        }

        private static async Task<JObject> GetUserData()
        {
            var json = await UserRepository.UserService.GetLoginWithTokenAsync();
            await Task.CompletedTask;
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
                if (data != null & UserRepository.UserService.ValidateWithToken(data))
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
            _ = LoggerService.LogAsync(this, $"isAuth {isAuth}");
        }
    }
}