using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Wallone.Authorization.ViewModels
{
    public class ConfirmViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public ConfirmViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

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
            }
        }
    }
}