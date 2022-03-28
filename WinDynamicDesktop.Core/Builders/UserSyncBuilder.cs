﻿using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using WinDynamicDesktop.Core.Models.App;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Core.Builders
{
    public class UserSyncBuilder
    {
        private string token;
        private bool isAuth;
        public UserSyncBuilder GetToken()
        {
            token = SettingsService.GetToken();
            return this;
        }

        private static async Task<JObject> GetUserData()
        {
            var json = await UserService.GetLoginWithTokenAsync();
            var objects = JObject.Parse(json);
            return objects;
        }

        public async Task<UserSyncBuilder> ValidateAsync()
        {
            if(token != null)
            {
                var data = await GetUserData();
                if (UserService.ValidateWithToken(data))
                {
                    isAuth = true;
                    return this;
                }
            }
            isAuth = false;
            return this;
        }

        public UserSyncBuilder GetThemes()
        {
            return this;
        }

        public bool IsUserAuth()
        {
            return isAuth;
        }

        public UserSyncBuilder GetSettings()
        {
            return this;
        }

        public UserSyncBuilder Build()
        {
            return this;
        }

        public Settings GetConfig()
        {
            throw new System.NotImplementedException();
        }

        public void GetCollection()
        {
            throw new System.NotImplementedException();
        }
    }
}