using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersFavoriteViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        private string header = "Избранные";
        public string Header { get => header; set => SetProperty(ref header, value); }
        public WallpapersFavoriteViewModel()
        {

        }
        public WallpapersFavoriteViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
