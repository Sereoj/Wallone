using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Controls.ViewModels;
using Wallone.Core.Builders;
using Wallone.Core.Controllers;
using Wallone.Core.Models;
using Wallone.Core.Models.App;
using Wallone.Core.Schedulers;
using Wallone.Core.Services;

namespace Wallone.Authorization.ViewModels
{
    /*
     * Некая подушка загрузки форм.
     * 
     */
    public class LoadViewModel : BindableBase, INavigationAware
    {
        private readonly DispatcherTimer ehternetTimer;
        private readonly IRegionManager regionManager;
        private string header;

        private bool isConnect;

        private bool isInternet;

        private bool isLoading = true;

        private bool isUpdate;

        private string message;
        private readonly SettingsItemBuilder settings;

    public LoadViewModel(){}

    public LoadViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            Header = Common.Translation.Localization.AppName;

            settings = new SettingsBuilder(SettingsService.Get()).ItemBuilder();

            ehternetTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            ehternetTimer.Tick += (s, e) => { LoadData(); };

            ehternetTimer.Start();
        }

        public UpdateViewModel UpdateViewModel { get; set; } = new UpdateViewModel();
        public NoConnectServerViewModel NoConnectServerViewModel { get; set; } = new NoConnectServerViewModel();


        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public bool IsInternet
        {
            get => isInternet;
            set => SetProperty(ref isInternet, value);
        }

        public bool IsConnect
        {
            get => isConnect;
            set => SetProperty(ref isConnect, value);
        }

        public bool IsUpdate
        {
            get => isUpdate;
            set => SetProperty(ref isUpdate, value);
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
            LoggerService.Log(this, message);
        }

        private async void LoadData()
        {
                var statusServer = AppEthernetService.IsConnect(Router.domainApi); // true
                SetMessage("Проверка соединения c " + Router.OnlyNameDomain());

                NoConnectServerViewModel.SetStatus(statusServer);
                IsConnect = !NoConnectServerViewModel.IsShow();

                if (statusServer)
                {
                    ehternetTimer.Stop();
                    IsLoading = true;

                    var data = await AppVersionService.GetVersionAsync();
                    await Task.CompletedTask;
                    if (!string.IsNullOrEmpty(data))
                    {
                        var appVersion = JsonConvert.DeserializeObject<AppVersion>(data);
                        AppVersionService.SetVersion(appVersion?.Version);
                    }
                    else
                    {
                        AppVersionService.SetVersion(null);
                    }
                    SetMessage("Поиск обновления...");
                    await Task.Delay(1000);

                    var versionCurrent = AppVersionService.GetCurrentVersion();
                    var versionActual = AppVersionService.GetActualVersion();

                    var index = new AppUpdaterBuilder()
                        .Compare(versionCurrent, versionActual);

                    UpdateViewModel.SetStatus(index);
                    UpdateViewModel.SetCurrentVersion(versionCurrent);
                    UpdateViewModel.SetActualVersion(versionActual);

                    IsUpdate = UpdateViewModel.IsShow();

                    if (!IsUpdate)
                    {
                        SetMessage("Обновлений не найдено");
                        await Task.Delay(1000);

                        SetMessage("Подождите пару секунд..");

                        var useGeolocation = new SettingsBuilder(SettingsService.Get())
                            .ItemBuilder()
                            .GetGeolocation();

                        if (useGeolocation)
                        {
                            await UseGeolocation();
                            UseTheme();
                        }

                        var builder = await new UserSyncBuilder()
                            .GetToken()
                            .ValidateAsync();
                        await Task.Delay(1000);

                        var authParameters = new NavigationParameters();
                        authParameters.Add("IsAuth", builder.IsUserAuth());
                        if (builder.IsUserAuth())
                            regionManager.RequestNavigate("ContentRegion", "Main", authParameters);
                        else
                            regionManager.RequestNavigate("ContentRegion", "Login");

                        builder.Build();
                    }
                    else
                    {
                        IsLoading = false;
                        SetMessage("Найдена новая версия приложения!");
                    }
                }
                else
                {
                    IsLoading = false;
                    SetMessage("Нет соединения c " + Router.OnlyNameDomainApi());
                }

                GC.Collect(2);
        }

        private void UseTheme()
        {
            var themeName = settings
                .GetImage();

            ThemeService.SetCurrentName(themeName);

            var theme = new ThemeCreatedBuilder()
                .SetName(ThemeService.GetCurrentName())
                .HasDownloaded()
                .GetThemeModelFromFile();

            var controller = new ThemeController();
            controller.Load(theme);

            var themeScheduler = new ThemeScheduler(controller);
            themeScheduler.Start();
        }

        private async Task UseGeolocation()
        {
            if (settings.GetGeolocation())
            {
                var dataLocation = await LocationService.GetLocationAsync();
                await Task.CompletedTask;
                if (!string.IsNullOrEmpty(dataLocation))
                {
                    var location = JsonConvert.DeserializeObject<Location>(dataLocation);
                    if (location != null)
                    {
                        settings
                            .SetLatitude(location.latitude)
                            .SetLongitude(location.longitude)
                            .SetCountry(location.country)
                            .SetCity(location.city)
                            .SetMode(Mode.UseWebLocation)
                            .Build();
                    }
                }
            }
        }
    }
}