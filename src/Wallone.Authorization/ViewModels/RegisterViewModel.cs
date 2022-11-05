using System;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Routers;
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
            OpenLinkCommand = new DelegateCommand(OnOpenLink);
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
        public DelegateCommand OpenLinkCommand { get; set; }

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
            }
        }

        private void OnOpenLink()
        {
            Process.Start(new ProcessStartInfo(Router.domain + $"/{Pages.Policy}") { UseShellExecute = true });
        }

        private async void Register(string name, string email, string password, string confirm)
        {
            try
            {
                var json = await UserRepository.UserService.GetRegisterAsync(name, email, password, confirm);
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
            UserRepository.Create();
            var msg = UserRepository.UserService.ValidateRegister(objects);
            var token = UserRepository.GetToken();

            if (token != null)
            {
                var settings = new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder();

                settings.SetEmail(Email);
                settings.SetToken(token);
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