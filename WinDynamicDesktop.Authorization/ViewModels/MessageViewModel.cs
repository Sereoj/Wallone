using Prism.Mvvm;

namespace WinDynamicDesktop.Authorization.ViewModels
{
    public class MessageViewModel : BindableBase
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        public MessageViewModel()
        {

        }
    }
}
