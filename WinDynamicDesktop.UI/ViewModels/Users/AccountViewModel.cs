using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class AccountViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        private string header = "Аккаунт";
        public string Header { get => header; set => SetProperty(ref header, value); }
        public AccountViewModel()
        {

        }
        public AccountViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
