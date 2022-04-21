using Prism.Mvvm;
using Prism.Regions;
using System;
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string name = "Настройки";
        public string Name { get => name; set => SetProperty(ref name, value); }

        public SettingsViewModel()
        {

        }
        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        private string host;

        public string Host { get => host; set => SetProperty(ref host, value); }

        private string prefix;

        public string Prefix { get => prefix; set => SetProperty(ref prefix, value); }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Host = SettingsService.Get().Host;
            Prefix = SettingsService.Get().Prefix;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (SettingsService.Exist())
            {
                SettingsService.Get().Host = Host;
                SettingsService.Get().Prefix = Prefix;
                SettingsService.Save();

                new HostBuilder()
                    .SetHost()
                    .SetPrefix()
                    .Validate()
                    .Build();
            }

            GC.Collect(2);
        }
    }
}
