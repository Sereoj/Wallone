using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WinDynamicDesktop.Authorization.Services;
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
            
            Token();

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string obj)
        {
            switch (obj)
            {
                case "Login":
                    //TODO:
                    _regionManager.RequestNavigate("ContentRegion", "Login");
                    //_regionManager.RequestNavigate("ContentRegion", "Main");
                    break;
                case "Confirm":
                    Register();
                    //_regionManager.RequestNavigate("ContentRegion", "Confirm");
                    break;
                default:
                    break;
            }
        }
        private async void Token()
        {
            try
            {
                var token = SettingsService.Get().Token;

                if (token != null)
                {
                    var json = await UserService.GetLoginWithTokenAsync(token);
                    var objects = JObject.Parse(json);
                    var msg = UserService.ValidateWithToken(objects);

                    if (msg == "success")
                    {
                        _regionManager.RequestNavigate("ContentRegion", "Main");
                    }
                    Message = msg;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }
        private async void Register()
        {
            try
            {
                var json = await UserService.GetRegisterAsync(Name, Email, Password, Confirm);
                var objects = JObject.Parse(json);

                var msg = UserService.ValidateRegister(objects);
                if (UserService.GetToken() != null)
                {
                    SettingsService.Get().Token = UserService.GetToken();
                    _regionManager.RequestNavigate("ContentRegion", "Main");
                }
                Message = msg;
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }

        }
    }
}
