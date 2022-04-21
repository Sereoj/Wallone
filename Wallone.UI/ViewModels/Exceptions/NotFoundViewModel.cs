using Prism.Mvvm;
using Prism.Regions;

namespace Wallone.UI.ViewModels.Exceptions
{
    public class NotFoundViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string text;

        public NotFoundViewModel()
        {
        }

        public NotFoundViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
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
            Text = (string) navigationContext.Parameters["Text"];
        }
    }
}