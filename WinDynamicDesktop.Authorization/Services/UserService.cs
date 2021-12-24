using Newtonsoft.Json.Linq;
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

        public static Task<string> GetRegisterAsync(string name, string email, string password, string password_confirmation)
        {
            var items = RequestRouter<string, Register>.PostAsync("register", new Register() { name = name, email = email, password = password, password_confirmation = password_confirmation });
            return items;
        }
    }
}
