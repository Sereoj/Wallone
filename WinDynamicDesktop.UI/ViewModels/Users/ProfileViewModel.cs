using Prism.Mvvm;
using Prism.Regions;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ProfileViewModel : BindableBase, INavigationAware
    {

        private string header = "Профиль";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }
        public ProfileViewModel()
        {

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new System.NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new System.NotImplementedException();
        }
    }
}
