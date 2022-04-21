using Prism.Mvvm;
using Prism.Regions;

namespace Wallone.UI.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private string _title = "Wallone";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }
    }
}
