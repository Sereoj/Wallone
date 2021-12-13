using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string obj)
        {
            switch (obj)
            {
                case "Register":
                    _regionManager.RequestNavigate("ContentRegion", "Register");
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
