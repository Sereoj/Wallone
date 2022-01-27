using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class AccountViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private User account;

        private string header = "Аккаунт";
        public string Header { get => header; set => SetProperty(ref header, value); }

        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private System.Windows.Media.ImageSource avatar;
        public System.Windows.Media.ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }

        private System.Windows.Media.ImageSource cover;
        public System.Windows.Media.ImageSource Cover { get => cover; set => SetProperty(ref cover, value); }

        private System.DateTime? dOB;
        public System.DateTime? DOB { get => dOB; set => SetProperty(ref dOB, value); }

        private string github;
        public string Github { get => github; set => SetProperty(ref github, value); }

        private string facebook;
        public string Facebook { get => facebook; set => SetProperty(ref facebook, value); }

        private string vk;
        public string VK { get => vk; set => SetProperty(ref vk, value); }

        private string twitter;
        public string Twitter { get => twitter; set => SetProperty(ref twitter, value); }
        public AccountViewModel()
        {

        }

        public AccountViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Loaded();
        }

        public async void Loaded()
        {
            try
            {
                var data = await AccountService.GetPageAsync();

                if (!string.IsNullOrEmpty(data))
                {
                    //var jArray = JArray.Parse(data);
                    account = JsonConvert.DeserializeObject<User>(data);
                    AccountService.Load(account);

                    Name = AccountService.GetUsername();
                    Description = AccountService.GetDescription();

                    DOB = AccountService.GetDOB();

                    Github = AccountService.GetGithub();
                    Facebook = AccountService.GetFacebook();
                    VK = AccountService.GetVK();
                    Twitter = AccountService.GetTwitter();
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}
