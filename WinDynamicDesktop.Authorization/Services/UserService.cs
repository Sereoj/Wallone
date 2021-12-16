using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinDynamicDesktop.Authorization.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.Services
{
    public class UserService
    {
        public static Task<string> GetLoginAsync(string email, string password)
        {
            var items = RequestRouter<string, Login>.PostAsync("login", new Login() { email = email, password = password });
            return items;
        }
    }
}
