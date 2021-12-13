using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class PhotoViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; set; }

        public PhotoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string obj)
        {
            switch (obj)
            {
                case "Main":
                    _regionManager.RequestNavigate("ContentRegion", "Main");
                    break;
                case "Back":
                    _regionManager.RequestNavigate("ContentRegion", "Confirm");
                    break;
                default:
                    break;
            }
        }
    }
}
