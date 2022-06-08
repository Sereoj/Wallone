using System;
using ModernWpf;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Models.Settings;
using Wallone.Core.Services;
using Wallone.UI.Properties;

namespace Wallone.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private string title = Common.Translation.Localization.AppName;

        private ModernWpf.ElementTheme currentTheme;
        public ModernWpf.ElementTheme CurrentTheme { get => currentTheme; set => SetProperty(ref currentTheme, value); }

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            new SettingsBuilder(SettingsService.Get())
            .ItemBuilder()
            .SetWindowTheme(currentTheme)
            .Build();

            SettingsService.Get().General.PropertyChanged += SettingsChanged;
        }


        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
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
            CurrentTheme = ((General)sender).Theme switch
            {
                ElementTheme.Default => ElementTheme.Default,
                ElementTheme.Light => ElementTheme.Light,
                ElementTheme.Dark => ElementTheme.Dark,
                _ => ElementTheme.Default
            };
        }
    }
}