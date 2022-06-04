using System;
using System.Diagnostics;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models.Settings;
using Wallone.Core.Services;

namespace Wallone.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private bool isAutorun;
        private bool isGeolocation;

        private string name = "Настройки";

        private SettingsItemBuilder settings
            ;

        public SettingsViewModel()
        {
        }

        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            SettingsService.Get().General.PropertyChanged += General_PropertyChanged;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool IsAutorun
        {
            get => isAutorun;
            set
            {
                Platformer
                    .GetHelper()
                    .SwitcherAutorun(AppSettingsService.GetApplicationPath(), value);
                SetProperty(ref isAutorun, value);
            }
        }

        public bool IsGeolocation
        {
            get => isGeolocation;
            set
            {
                settings
                    .SetGeolocation(value)
                    .Build();
                SetProperty(ref isGeolocation, value);
            }
        }



        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            settings = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            IsAutorun = settings.GetAutorun() && Platformer.GetHelper().CheckAutorun();
            IsGeolocation = settings.GetGeolocation();
            settings.Build();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void General_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsAutorun = ((General)sender).AutoRun;
        }
    }
}