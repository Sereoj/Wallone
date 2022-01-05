using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        
        private string header = "Настройки";
        public string Header { get => header; set => SetProperty(ref header, value); }
        public SettingsViewModel()
        {

        }
        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
