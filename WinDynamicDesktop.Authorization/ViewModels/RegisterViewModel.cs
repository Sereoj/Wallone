using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class RegisterViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public RegisterViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string obj)
        {
            switch (obj)
            {
                case "Login":
                    _regionManager.RequestNavigate("ContentRegion", "Login");
                    break;
                case "Confirm":
                    _regionManager.RequestNavigate("ContentRegion", "Confirm");
                    break;
                default:
                    break;
            }
        }
    }
}
