using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Diagnostics;
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
                    //_regionManager.RequestNavigate("ContentRegion", "Confirm");
                    break;
                default:
                    break;
            }
        }

        private async void Login()
        {
            var json = await UserService.GetLoginAsync(Email, Password);
            var objects = JObject.Parse(json);
            
            if(objects["auth.failed"] != null)
            {
                Message = objects["auth.failed"].ToString();
            }

            if (objects["email"] != null && objects["password"] != null)
            {
                Message = objects["email"][0].ToString() + "\n" + objects["password"][0].ToString();
            }

            if(objects["email"] != null)
            {
                Message = objects["email"][0].ToString();
            }

            if (objects["password"] != null)
            {
                Message = objects["password"][0].ToString();
            }

        }
    }
}
