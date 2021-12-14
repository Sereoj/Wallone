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
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class Wallpapers : BindableBase, INavigationAware
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
        public Wallpapers()
        {
            Library.Add(new ArticleViewModel(regionManager));
            Library.Add(new ArticleViewModel(regionManager));
        }
        public Wallpapers(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            ThumbService.GetThumbs(null);
            //foreach (var item in )
            //{
            //    Library.Add(new ArticleViewModel(regionManager)
            //    {
            //        ID = item.ID,
            //        Name = item.Name,
            //        ImageSource = new BitmapImage(item.Preview)
            //    });
            //}
            this.eventAggregator.GetEvent<ScrollEvent>().Subscribe(ScrollLineReceived);
        }

        private void ScrollLineReceived(ScrollChangedEventArgs e)
        {
            if(e.VerticalOffset != 0)
            {
                if (e.VerticalOffset == e.ExtentHeight - e.ViewportHeight)
                {
                    //Library.Add(new ArticleViewModel(regionManager)
                    //{
                    //    ID = "",
                    //    Name = "",
                    //});
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
    }
}
