using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Net;
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

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
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
            Email = email ?? "user@w2me.ru";
        }
        private async void Login()
        {
            try
            {
                var json = await UserService.GetLoginAsync(Email, Password);
                var s = AppEthernetService.GetStatus();

                switch (s)
                {
                    case HttpStatusCode.OK:
                        LoadLogin(json);
                        break;
                    case HttpStatusCode.NotFound:
                        Message = "Страница не существует";
                        break;
                }
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

        private void LoadLogin(string json)
        {
            var objects = JObject.Parse(json);

            var msg = UserService.ValidateLogin(objects);

            if (UserService.GetToken() != null)
            {
                var settings = SettingsService.Get();
                settings.Email = Email;
                settings.Token = UserService.GetToken();
                SettingsService.Save();
                _regionManager.RequestNavigate("ContentRegion", "Main");
            }
            else
            {
                Message = msg;
            }
        }
    }
}
