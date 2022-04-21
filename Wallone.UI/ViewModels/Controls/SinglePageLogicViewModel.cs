using ModernWpf.Controls;
using Prism.Mvvm;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class SinglePageLogicViewModel : BindableBase
    {
        private string displayTextInstall;
        public string DisplayTextInstall { get => displayTextInstall; set => SetProperty(ref displayTextInstall, value); }

        private FontIcon displayTextFavorite;
        public FontIcon DisplayTextFavorite { get => displayTextFavorite; set => SetProperty(ref displayTextFavorite, value); }

        private FontIcon displayTextReation;
        public FontIcon DisplayTextReation { get => displayTextReation; set => SetProperty(ref displayTextReation, value); }


        private bool isInstalled;
        public bool IsInstalled
        {
            get => isInstalled;
            set
            {
                SetProperty(ref isInstalled, value);
                DisplayTextInstall = value == true ? "Удалить" : "Установить";
            }
        }

        private bool isEnableInstalled = true;
        public bool IsEnableInstalled { get => isEnableInstalled; set => SetProperty(ref isEnableInstalled, value); }

        private bool isFavorited;
        public bool IsFavorited
        {
            get => isFavorited;
            set
            {
                SetProperty(ref isFavorited, value);
                DisplayTextFavorite = value == true ? FontIconService.SetIcon("ultimate", "\uECB7") : FontIconService.SetIcon("ultimate", "\uECB8");
            }
        }

        private bool isEnableFavorited = true;
        public bool IsEnableFavorited { get => isEnableFavorited; set => SetProperty(ref isEnableFavorited, value); }

        private bool isLiked;
        public bool IsLiked
        {
            get => isLiked;
            set
            {
                SetProperty(ref isLiked, value);
                DisplayTextReation = value == true ? FontIconService.SetIcon("ultimate", "\uECE9") : FontIconService.SetIcon("ultimate", "\uECEA");
            }
        }

        private bool isEnableLiked = true;
        public bool IsEnableLiked { get => isEnableLiked; set => SetProperty(ref isEnableLiked, value); }
    }
}