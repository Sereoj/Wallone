using Prism.Mvvm;

namespace Wallone.UI.ViewModels.Controls
{
    public class ItemTemplateViewModel : BindableBase
    {
        private string text;

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
    }
}