using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Net;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class RegisterViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

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

        private string confirm;
        public string Confirm
        {
            get { return confirm; }
            set { SetProperty(ref confirm, value); }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public RegisterViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

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
                    var number = new Random().Next();
                    var name = "Guest";
                    var email = "guest_" + number + "@w2me.ru";
                    var password = temppass();
                    Register(name, email, password, password);
                    break;
                default:
                    break;
            }
        }

        private string temppass()
        {
            string abc = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM!@#$%^&*()"; //набор символов
            int kol = 8; // кол-во символов
            string result = "";

            Random rnd = new Random();
            int lng = abc.Length;
            for (int i = 0; i < kol; i++)
            {
                result += abc[rnd.Next(lng)];
            }
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
