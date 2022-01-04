using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class ConfirmViewModel : BindableBase
    {
        readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }
        public ConfirmViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string obj)
        {
            switch (obj)
            {
                case "Photo":
                    _regionManager.RequestNavigate("ContentRegion", "Photo");
                    break;
                case "Back":
                    _regionManager.RequestNavigate("ContentRegion", "Register");
                    break;
                default:
                    break;
            }
        }
    }
}
