using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using WinDynamicDesktop.Authorization.Services;
using WinDynamicDesktop.Core.Models.App;
using WinDynamicDesktop.Core.Services;

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

            Autocomplete();

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
        private void Autocomplete()
        {
            var email = SettingsService.Get().Email;
            if (email != null)
                Email = email;
        }
        private async void Login()
        {
            try
            {
                var json = await UserService.GetLoginAsync(Email, Password);
                var objects = JObject.Parse(json);

                var msg = UserService.ValidateLogin(objects);
                if (UserService.GetToken() != null)
                {
                    var settings = SettingsService.Get();

                    settings.Token = UserService.GetToken();
                    _regionManager.RequestNavigate("ContentRegion", "Main");
                }

                Message = msg;
            }
            catch (InvalidOperationException ex)
            {
                Message = ex.Message;
            }
            catch (ArgumentException)
            {
                Message = "Ошибка чтения данных! Ждите обновление.";
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
    }
}
