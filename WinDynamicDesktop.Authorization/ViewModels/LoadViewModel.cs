using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using Prism.Regions;
using WinDynamicDesktop.Authorization.Interfaces;
using WinDynamicDesktop.Authorization.Services;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    /*
     * Некая подушка загрузки форм.
     * 
     */
    public class LoadViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;

        private string header;
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }


        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public LoadViewModel()
        {
            // Здесь по хорошему сделать проверку на доступность сервера и т.д
        }

        public LoadViewModel(IRegionManager regionManager)
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
            if(!SettingsService.CheckFirstLaunch())
            {
                Token();
            }
        }

        private async void Token()
        {
            try
            {
                var token = SettingsService.GetToken();
                if (token != null)
                {
                    var json = await UserService.GetLoginWithTokenAsync();
                    var objects = JObject.Parse(json);
                    var msg = UserService.ValidateWithToken(objects);

                    if (msg == "success")
                    {
                        regionManager.RequestNavigate("ContentRegion", "Main");
                    }
                    else
                    {
                        regionManager.RequestNavigate("ContentRegion", "Login");
                    }
                }
                else
                {
                    regionManager.RequestNavigate("ContentRegion", "Register");
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
    }
}
