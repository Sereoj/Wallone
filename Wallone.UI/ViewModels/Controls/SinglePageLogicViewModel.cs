using ModernWpf.Controls;
using Prism.Mvvm;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class SinglePageLogicViewModel : BindableBase
    {
        private string displayTextDownload;
        private FontIcon displayTextFavorite;
        private string displayTextInstall;

        private FontIcon displayTextReation;
        private bool isDownloaded;

        private bool isEnableDownloaded = true;
        private bool isEnableFavorited = true;
        private bool isEnableInstalled = true;
        private bool isEnableLiked = true;

        private bool isFavorited;
        private bool isInstalled;
        private bool isLiked;

        public string DisplayTextDownload
        {
            get => displayTextDownload;
            set => SetProperty(ref displayTextDownload, value);
        }

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

        public bool IsDownloaded
        {
            get => isDownloaded;
            set
            {
                SetProperty(ref isDownloaded, value);
                RaisePropertyChanged();
                DisplayTextDownload = value ? "Удалить" : "Загрузить";
            }
        }

        public bool IsInstalled
        {
            get => isInstalled;
            set
            {
                SetProperty(ref isInstalled, value);
                RaisePropertyChanged();
                DisplayTextInstall = value ? "Выключить" : "Включить";
            }
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

        public bool IsEnableDownloaded
        {
            get => isEnableDownloaded;
            set => SetProperty(ref isEnableDownloaded, value);
        }

        public bool IsEnableInstalled
        {
            get => isEnableInstalled;
            set => SetProperty(ref isEnableInstalled, value);
        }

        public bool IsEnableFavorited
        {
            get => isEnableFavorited;
            set => SetProperty(ref isEnableFavorited, value);
        }

        public bool IsEnableLiked
        {
            get => isEnableLiked;
            set => SetProperty(ref isEnableLiked, value);
        }
    }
}