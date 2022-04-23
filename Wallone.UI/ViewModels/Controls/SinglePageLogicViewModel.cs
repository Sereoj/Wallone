using ModernWpf.Controls;
using Prism.Mvvm;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class SinglePageLogicViewModel : BindableBase
    {
        private FontIcon displayTextFavorite;
        private string displayTextInstall;

        private FontIcon displayTextReation;

        private bool isEnableFavorited = true;

        private bool isEnableInstalled = true;

        private bool isEnableLiked = true;

        private bool isFavorited;


        private bool isInstalled;

        private bool isLiked;

        public string DisplayTextInstall
        {
            get => displayTextInstall;
            set => SetProperty(ref displayTextInstall, value);
        }

        public FontIcon DisplayTextFavorite
        {
            get => displayTextFavorite;
            set => SetProperty(ref displayTextFavorite, value);
        }

        public FontIcon DisplayTextReation
        {
            get => displayTextReation;
            set => SetProperty(ref displayTextReation, value);
        }

        public bool IsInstalled
        {
            get => isInstalled;
            set
            {
                SetProperty(ref isInstalled, value);
                RaisePropertyChanged();
                DisplayTextInstall = value ? "Удалить" : "Установить";
            }
        }

        public bool IsEnableInstalled
        {
            get => isEnableInstalled;
            set => SetProperty(ref isEnableInstalled, value);
        }

        public bool IsFavorited
        {
            get => isFavorited;
            set
            {
                SetProperty(ref isFavorited, value);
                RaisePropertyChanged();
                DisplayTextFavorite =
                    value
                        ? FontIconService.SetIcon("ultimate", "\uECB7")
                        : FontIconService.SetIcon("ultimate", "\uECB8");
            }
        }

        public bool IsEnableFavorited
        {
            get => isEnableFavorited;
            set => SetProperty(ref isEnableFavorited, value);
        }

        public bool IsLiked
        {
            get => isLiked;
            set
            {
                SetProperty(ref isLiked, value);
                RaisePropertyChanged();
                DisplayTextReation =
                    value
                        ? FontIconService.SetIcon("ultimate", "\uECE9")
                        : FontIconService.SetIcon("ultimate", "\uECEA");
            }
        }

        public bool IsEnableLiked
        {
            get => isEnableLiked;
            set => SetProperty(ref isEnableLiked, value);
        }
    }
}