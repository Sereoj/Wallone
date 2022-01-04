using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class NotFoundViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string text;
        public string Text { get => text; set => SetProperty(ref text, value); }
        public NotFoundViewModel()
        {

        }
        public NotFoundViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            Text = (string)navigationContext.Parameters["Text"];
        }
    }
}