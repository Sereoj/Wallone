using Prism.Mvvm;
using System.Windows.Media;

namespace Wallone.UI.ViewModels.Wallpapers
{
    public class SinglePageAdsViewModel : BindableBase
    {
        private ImageSource imageSource;

        private bool isVisible;

        private string link;
        private string text;

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        public string Link
        {
            get => link;
            set => SetProperty(ref link, value);
        }

        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }
    }
}