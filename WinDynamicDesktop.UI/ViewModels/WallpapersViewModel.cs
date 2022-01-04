using ModernWpf.Controls;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Events;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Interfaces;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;

        public ObservableCollection<ArticleViewModel> Library { get; set; } = new ObservableCollection<ArticleViewModel>();

        private string header = "Библиотека";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        public WallpapersViewModel()
        {
            Library.Add(new ArticleViewModel(regionManager));
            Library.Add(new ArticleViewModel(regionManager));
        }
        public WallpapersViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            Loaded();

            this.eventAggregator.GetEvent<ScrollEvent>().Subscribe(ScrollLineReceived);
        }

        private void ScrollLineReceived(ScrollChangedEventArgs e)
        {
            if(e.VerticalOffset != 0)
            {
                if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
                {
                }
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public async void Loaded()
        {
            var items = await ThumbService.GetThumbsAsync(null);

            try
            {
                foreach (var item in items)
                {
                    Library.Add(new ArticleViewModel(regionManager)
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                    });
                }
            }
            catch(Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}
