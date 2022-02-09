using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Interfaces;
using WinDynamicDesktop.UI.Services;

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

        public DelegateCommand<ScrollChangedEventArgs> ScrollCommand { get; set; }
        public WallpapersViewModel()
        {
            Library.Add(new ArticleViewModel(regionManager));
            Library.Add(new ArticleViewModel(regionManager));
        }
        public WallpapersViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            ScrollCommand = new DelegateCommand<ScrollChangedEventArgs>(ScrollChanged);
        }

        private void ScrollChanged(ScrollChangedEventArgs obj)
        {

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
            var fields = new List<Core.Models.Parameter>();
            var root = (string)navigationContext.Parameters["Root"];
            var page = (string)navigationContext.Parameters["Page"];
            var page_id = (string)navigationContext.Parameters["ID"];

            if (page != null)
            {
                switch (root)
                {
                    case "brand":
                        fields.Add(new Core.Models.Parameter() { Name = "brand_id", Value = page_id });
                        break;
                    case "category":
                        fields.Add(new Core.Models.Parameter() { Name = "category_id", Value = page_id });
                        break;
                    default:
                        fields = null;
                        break;
                }
            }
            Loaded(null, fields);
        }

        public async void Loaded(string page, List<Core.Models.Parameter> parameters)
        {
            Library.Clear();
            try
            {
                var items = await ThumbService.GetThumbsAsync(page, parameters);

                if (ThumbService.CheckItems(items))
                {
                    foreach (var item in items)
                    {
                        Library.Add(new ArticleViewModel(regionManager)
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                        });
                        await Task.CompletedTask;
                    }
                }
                else
                {
                    var param = new NavigationParameters
                    {
                        { "Text", "Это не ошибка, просто не найдены изображения!" }
                    };

                    regionManager.RequestNavigate("PageRegion", "NotFound", param);
                }
            }
            catch (Exception ex)
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
