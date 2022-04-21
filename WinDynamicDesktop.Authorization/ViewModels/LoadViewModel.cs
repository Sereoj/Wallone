
/* Необъединенное слияние из проекта "WinDynamicDesktop.Authorization (net5.0-windows10.0.18362)"
До:
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
После:
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Json;
using System;
using Prism.Diagnostics;
*/
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using System.Threading.Tasks;
using WinDynamicDesktop.Controls.ViewModels;
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.Core.Models.App;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    /*
     * Некая подушка загрузки форм.
     * 
     */
    public class LoadViewModel : BindableBase, INavigationAware
    {
        private IRegionManager regionManager;

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

        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
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
        private void SetMessage(string message)
        {
            Message = message;
        }
        private async void LoadData()
        {
            bool status = AppEthernetService.IsConnect(Router.domainExample); // true
            SetMessage("Проверка соединения с интернетом");
            await Task.Delay(2000);

            NoNetworkViewModel.SetStatus(status);
            IsInternet = !NoNetworkViewModel.IsShow(); // false

            if (IsInternet)
            {
                SetMessage("Нет интернет соединения");
                IsLoading = false;
            }
            else
            {
                bool statusServer = AppEthernetService.IsConnect(Router.domainApi); // true
                SetMessage("Проверка соединения c " + Router.OnlyNameDomain());
                await Task.Delay(2000);

                NoConnectServerViewModel.SetStatus(statusServer);
                IsConnect = !NoConnectServerViewModel.IsShow();

                if (statusServer)
                {
                    string data = await AppVersionService.GetVersionAsync();
                    var appVersion = JsonConvert.DeserializeObject<AppVersion>(data);

                    AppVersionService.SetVersion(appVersion.Version);
                    SetMessage("Поиск обновления...");
                    await Task.Delay(2000);

                    string verionCurrent = AppVersionService.GetCurrentVersion();
                    string verionActual = AppVersionService.GetActualVersion();

                    int index = new AppUpdaterBuilder()
                                .Compare(verionCurrent, verionActual);

                    UpdateViewModel.SetStatus(index);
                    UpdateViewModel.SetCurrentVersion(verionCurrent);
                    UpdateViewModel.SetActualVersion(verionActual);

                    IsUpdate = UpdateViewModel.IsShow();

                    if (!IsUpdate)
                    {
                        SetMessage("Обновлений не найдено");
                        await Task.Delay(1000);

                        SetMessage("Подождите пару секунд..");
                        var builder = await new UserSyncBuilder()
                            .GetToken()
                            .ValidateAsync();
                        await Task.Delay(1000);

                        if (builder.IsUserAuth())
                        {
                            regionManager.RequestNavigate("ContentRegion", "Main");
                        }
                        else
                        {
                            regionManager.RequestNavigate("ContentRegion", "Login");
                        }
                    }
                }
                else
                {
                    IsLoading = false;
                    SetMessage("Нет соединения c " + Router.OnlyNameDomainApi());
                }

            }
        }
    }
}
