using Prism.Mvvm;
using WinDynamicDesktop.UI.Interfaces;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ProfileViewModel : BindableBase
    {

        private string header = "Профиль";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        public ProfileViewModel()
        {

        }
    }
}
