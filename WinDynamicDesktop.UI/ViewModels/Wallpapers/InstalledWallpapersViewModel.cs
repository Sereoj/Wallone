using Prism.Regions;
using Prism.Mvvm;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class InstalledWallpapersViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        private string header = "Установленные";
        public string Header { get => header; set => SetProperty(ref header, value); }

        public InstalledWallpapersViewModel()
        {

        }
        public InstalledWallpapersViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
