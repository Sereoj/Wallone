using Prism.Regions;
using Prism.Mvvm;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersLoadViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        private string header = "Установленные";
        public string Header { get => header; set => SetProperty(ref header, value); }

        public WallpapersLoadViewModel()
        {

        }
        public WallpapersLoadViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
