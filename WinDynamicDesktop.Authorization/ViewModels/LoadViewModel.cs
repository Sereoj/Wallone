using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using Prism.Regions;
using WinDynamicDesktop.Authorization.Interfaces;
using WinDynamicDesktop.Authorization.Services;
using WinDynamicDesktop.Controls.ViewModels;
using WinDynamicDesktop.Core.Builders;
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

        public UpdateViewModel UpdateViewModel { get; set; } = new UpdateViewModel();
        public NoConnectServerViewModel NoConnectServerViewModel { get; set; } = new NoConnectServerViewModel();

        public NoNetworkViewModel NoNetworkViewModel { get; set; } = new NoNetworkViewModel();

        public LoadingViewModel LoadingViewModel { get; set; } = new LoadingViewModel();

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

        private bool isLoading = true;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private bool isInternet = false;
        public bool IsInternet
        {
            get { return isInternet; }
            set { SetProperty(ref isInternet, value); }
        }

        private bool isConnect = false;
        public bool IsConnect
        {
            get { return isConnect; }
            set { SetProperty(ref isConnect, value); }
        }

        private bool isUpdate = false;
        public bool IsUpdate
        {
            get { return isUpdate; }
            set { SetProperty(ref isUpdate, value); }
        }

        public LoadViewModel()
        {
        }

        public LoadViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            Header = "Wallone";

            LoadData();
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
        }
        private async void LoadData()
        {
            var status = AppEthernetService.IsConnect();
            NoNetworkViewModel.SetStatus(status);
            IsInternet = NoNetworkViewModel.IsShow();

            var version = await AppVersionService.GetVersionAsync();
            AppVersionService.SetVersion(version);

            if (status)
            {

                string verionCurrent = AppVersionService.GetCurrentVersion();
                var verionActual = AppVersionService.GetActualVersion();

                var index = new AppUpdaterBuilder()
                            .Compare(verionCurrent, verionActual);
                
                UpdateViewModel.SetStatus(index);
                UpdateViewModel.SetCurrentVersion(verionCurrent);
                UpdateViewModel.SetActualVersion(verionActual);

                IsUpdate = UpdateViewModel.IsShow();

            }
        }
        private async void ValidateAuth()
        {
            JObject objects = await Auth();
            string msg = UserService.ValidateWithToken(objects);
            //isAuth = msg == "success";
        }

        private static async Task<JObject> Auth()
        {
            var json = await UserService.GetLoginWithTokenAsync();
            var objects = JObject.Parse(json);
            return objects;
        }

        private void GetToken()
        {
            try
            {
                string token = SettingsService.GetToken();

                if(!string.IsNullOrEmpty(token))
                {
                    ValidateAuth();
                }
            }catch(Exception ex)
            {

            }
        }
    }
}
