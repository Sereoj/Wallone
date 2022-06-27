using System;
using System.Net;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Users;

namespace Wallone.Authorization.ViewModels
{
    public class RegisterViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private string confirm;

        private string email;

        private string message;

        private string name;

        private string password;

        public RegisterViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
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

        public string Confirm
        {
            get => confirm;
            set => SetProperty(ref confirm, value);
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
                case "Login":
                    //TODO:
                    _regionManager.RequestNavigate("ContentRegion", "Login");
                    break;
                case "Confirm":
                    Register(Name, Email, Password, Confirm);
                    break;
                case "Guest":
                    //var number = new Random().Next();
                    //var name = "Guest";
                    //var email = "guest_" + number + "@w2me.ru";
                    //var password = temppass();
                    //Register(name, email, password, password);
                    break;
            }
        }

        private string temppass()
        {
            var abc = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!@#$%^&*()"; //набор символов
            var kol = 8; // кол-во символов
            var result = "";

            var rnd = new Random();
            var lng = abc.Length;
            for (var i = 0; i < kol; i++) result += abc[rnd.Next(lng)];
            return "guest_" + result;
        }

        private async void Register(string name, string email, string password, string confirm)
        {
            try
            {
                var json = await UserService.GetRegisterAsync(name, email, password, confirm);
                var s = AppEthernetService.GetStatus();

                switch (s)
                {
                    case HttpStatusCode.OK:
                        LoadRegister(json);
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

        private void LoadRegister(string json)
        {
            var objects = JObject.Parse(json);

            var msg = UserService.ValidateRegister(objects);
            if (UserService.GetToken() != null)
            {
                var settings = new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder();

                settings.SetEmail(Email);
                settings.SetToken(UserService.GetToken());
                SettingsRepository.Save();

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