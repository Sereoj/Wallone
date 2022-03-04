using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ArticleViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private double width = 270;
        public double Width { get => width; set => SetProperty(ref width, value); }

        private double height = 210;
        public double Height { get => height; set => SetProperty(ref height, value); }

        private BitmapImage imageSource;
        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string id;
        public string ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public DelegateCommand<MouseButtonEventArgs> ArticleMouseDownCommand { get; set; }
        public ArticleViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ArticleMouseDownCommand = new DelegateCommand<MouseButtonEventArgs>(OnArticleMouseDown);
        }

        private void OnArticleMouseDown(MouseButtonEventArgs obj)
        {
            var param = new NavigationParameters
            {
                { "ID", ID },
                { "Name", Name },
                { "ImageSource", ImageSource }
            };
            regionManager.RequestNavigate("PageRegion", "SinglePage", param);
        }
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

        public void SetSize(double w, double h)
        {
            Width = w;
            Height = h;
        }
    }
}