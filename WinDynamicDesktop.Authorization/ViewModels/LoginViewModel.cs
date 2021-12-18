using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Windows;
using WinDynamicDesktop.Authorization.Services;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;

        private string email;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        }

        private string paswword;
        public string Password
        {
            get { return paswword; }
            set { SetProperty(ref paswword, value); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string obj)
        {
            switch (obj)
            {
                case "Register":
                    _regionManager.RequestNavigate("ContentRegion", "Register");
                    break;
                case "Confirm":
                    Login();
                    break;
                default:
                    break;
            }
        }

        private async void Login()
        {
            string msg = null;
            var json = await UserService.GetLoginAsync(Email, Password);
            var objects = JObject.Parse(json);

            if(objects["auth.failed"] != null)
            {
                msg = objects["auth.failed"].ToString();
            }
            else if(objects["token"] != null)
            {
                _regionManager.RequestNavigate("ContentRegion", "Main");
            }
            else if(objects is JObject)
            {
                foreach (var item in objects)
                {
                    msg += item.Value[0] + " ";
                }
            }
            Message = msg;
        }
    }
}
