using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Wallone.Authorization.ViewModels
{
    public class PhotoViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public PhotoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

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
            }
        }
    }
}