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

            SettingsService.Get().General.PropertyChanged += SettingsChanged;
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


        private bool isAutoSetImage;
        public bool IsAutoSetImage
        {
            get { return isAutoSetImage; }
            set
            {
                settings
                    .SetAutoSetImage(value)
                    .Build();
                SetProperty(ref isAutoSetImage, value);
            }
        }

        private int themeIndexSelected;
        public int ThemeIndexSelected
        {
            get { return themeIndexSelected; }
            set
            {
                settings
                    .SetWindowTheme((ModernWpf.ElementTheme)value)
                    .Build();
                SetProperty(ref themeIndexSelected, value);
            }
        }


        private bool isSetModel;
        public bool IsSetModel
        {
            get { return isSetModel; }
            set
            {
                settings
                    .SetModelWindow(value)
                    .Build();
                SetProperty(ref isSetModel, value);
            }
        }

        private bool isAnimation;
        public bool IsAnimation
        {
            get { return isAnimation; }
            set
            {
                settings
                    .SetAnimation(value)
                    .Build();
                SetProperty(ref isAnimation, value);
            }
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            settings = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            ThemeIndexSelected = settings.GetWindowTheme() switch
            {
                ModernWpf.ElementTheme.Default => 0,
                ModernWpf.ElementTheme.Light => 1,
                ModernWpf.ElementTheme.Dark => 2,
                _ => ThemeIndexSelected = 0
            };
            IsAutorun = settings.GetAutorun() && Platformer.GetHelper().CheckAutorun();
            IsGeolocation = settings.GetGeolocation();
            IsAutoSetImage = settings.GetAutoSetImage();
            IsSetModel = settings.GetModelWindow();
            IsAnimation = settings.GetAnimation();
            settings.Build();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsAutorun = ((General)sender).AutoRun;
        }
    }
}