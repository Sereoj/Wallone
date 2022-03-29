using Prism.Mvvm;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ItemTemplateViewModel : BindableBase
    {
        private string text;
        public string Text { get => text; set => SetProperty(ref text, value); }
        public ItemTemplateViewModel()
        {

        }
    }
}
