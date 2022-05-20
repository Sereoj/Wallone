using System;
using System.Net;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Services;

namespace Wallone.Authorization.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private string email;

        private string message;

        private string password;

        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            Autocomplete();
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

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
            }
        }

        private void Autocomplete()
        {
            var userEmail = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder()
                .GetEmail();

            Email = userEmail ?? "user@w2me.ru";
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
                    case HttpStatusCode.MethodNotAllowed:
                        Message = "Неверная отправка данных";
                        break;
                    case HttpStatusCode.InternalServerError:
                        Message = "Ошибка сервера";
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

                var settings = new SettingsBuilder(SettingsService.Get())
                    .ItemBuilder();

                settings.SetEmail(Email);
                settings.SetToken(UserService.GetToken());
                SettingsService.Save();

                _regionManager.RequestNavigate("ContentRegion", "Main");
            }
            else
            {
                Message = msg;
            }

            GC.Collect(2);
        }
    }
}