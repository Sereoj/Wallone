using ModernWpf.Controls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Interfaces;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersNewViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;
        private string header = "Новые";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        public ObservableCollection<ArticleViewModel> Library { get; set; } = new ObservableCollection<ArticleViewModel>();
        public WallpapersNewViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            Loaded();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public async void Loaded()
        {
            var items = await ThumbService.GetThumbsAsync("new");
            foreach (var item in items)
            {
                Library.Add(new ArticleViewModel(regionManager)
                {
                    ID = item.ID,
                    Name = item.Name,
                    ImageSource = new BitmapImage(item.Preview)
                });
            }
        }
    }
}
