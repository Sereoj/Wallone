using System.Windows.Input;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Wallone.UI.ViewModels.Controls
{
    public class ArticleViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string downloads;

        private string id;


        private BitmapImage imageSource;

        private string name;

        private string views;

        public ArticleViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ArticleMouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(OnArticleMouseDown);
        }

        public BitmapImage ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Downloads
        {
            get => downloads;
            set => SetProperty(ref downloads, value);
        }

        public string Views
        {
            get => views;
            set => SetProperty(ref views, value);
        }

        public string ID
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public DelegateCommand<MouseButtonEventArgs> ArticleMouseDownCommand { get; set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void OnArticleMouseDown(MouseButtonEventArgs obj)
        {
            var param = new NavigationParameters
            {
                {"ID", ID},
                {"Name", Name},
                {"ImageSource", ImageSource}
            };
            regionManager.RequestNavigate("PageRegion", "SinglePage", param);
        }
    }
}