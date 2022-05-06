using Prism.Mvvm;
using Prism.Regions;
using System;
using Wallone.Core.Builders;
using Wallone.Core.Services;

namespace Wallone.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string host;

        private string name = "Настройки";

        private string prefix;

        public SettingsViewModel()
        {
        }

        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Host
        {
            get => host;
            set => SetProperty(ref host, value);
        }

        public string Prefix
        {
            get => prefix;
            set => SetProperty(ref prefix, value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Host = SettingsService.Get().Host;
            Prefix = SettingsService.Get().Prefix;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

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