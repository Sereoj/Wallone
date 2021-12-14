using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Diagnostics;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersNewViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private string header = "Новые";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        public WallpapersNewViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
