using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.UI.Interfaces;
using Wallone.UI.Services;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.ViewModels.Wallpapers
{
    public class WallpapersViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;
        private string header = "Библиотека";

        private bool isContent;

        private bool isInternet;

        private bool isLoading = true;
        private List<Parameter> parameters;

        private string router;

        public WallpapersViewModel()
        {
            Library.Add(new ArticleViewModel(null));
            Library.Add(new ArticleViewModel(null));
        }

        public WallpapersViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            ViewerScrollChangedCommand = new DelegateCommand<ScrollChangedEventArgs>(OnViewerScrollChanged);
        }

        public IPageBulder PageBuilder { get; private set; }

        public DelegateCommand<ScrollChangedEventArgs> ViewerScrollChangedCommand { get; set; }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                SetProperty(ref isLoading, value);
                IsContent = value == false;
            }
        }

        public bool IsInternet
        {
            get => isInternet;
            set
            {
                SetProperty(ref isInternet, value);
                IsContent = value == false;
            }
        }

        public bool IsContent
        {
            get => isContent;
            set => SetProperty(ref isContent, value);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Library.Clear();
            GC.Collect(2);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var root = (string) navigationContext.Parameters["Root"] ?? "Gallery";
            var page_id = (string) navigationContext.Parameters["ID"] ?? "Main";

            PageBuilder = new PageBuilder() // Создаем билдер
                .Query(new PageGallaryBuilder()) //Говорим, что gallery
                .Catalog(root) //Устанввливам каталог
                .Page(page_id) //Устанавливаем страницу
                .Validate() //Валидация полученных значений
                .Pagination("1")
                .ShowAds(false) //Отображение рекламы
                .Build(); //Сборка

            router = PageBuilder.GetRouter();
            parameters = PageBuilder.GetFields();
            Header = (string) navigationContext.Parameters["Text"] ?? "Библиотека";
        }

        public ObservableCollection<ArticleViewModel> Library { get; set; } =
            new ObservableCollection<ArticleViewModel>();

        public string Header
        {
            get => header;
            set
            {
                SetProperty(ref header, value);
                //При изменении заголовка обновлять контент

                if (router == "wallpapers" || parameters.Count > 0) Loaded(null, router, parameters);
                router = null;
                parameters = null;
            }
        }

        private void OnViewerScrollChanged(ScrollChangedEventArgs e)
        {
            var data = ScrollViewerService.Get(ref e);

            if (e.ViewportHeight + e.VerticalOffset == e.ExtentHeight)
            {
                PageBuilder.Pagination("2");
                PageBuilder.Build();

                router = PageBuilder.GetRouter();
                parameters = PageBuilder.GetFields();

                Loaded(null,router, parameters);
            }
        }

        public async void Loaded(string page, string router, List<Parameter> parameters)
        {
            try
            {
                IsLoading = true;
                Trace.WriteLine("Router: " + router + "|Page: " + page);
                var items = await ThumbService.GetThumbsAsync(router, page, parameters);
                Trace.WriteLine("Количество постов: " + items.Count);
                await LoadImages(items);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    {"Text", ex.Message}
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }

        private async Task LoadImages(List<Thumb> items)
        {
            if (ThumbService.IsNotNull(items))
                foreach (var item in items)
                    if (ThumbService.IsIdNotNull(item.ID))
                        Library.Add(new ArticleViewModel(regionManager)
                        {
                            ID = item.ID,
                            Name = ThumbService.ValidateName(item.Name),
                            ImageSource = new BitmapImage(UriHelper.Get(item.Preview)),
                            Views = ThumbService.ValidateViews(item.Views),
                            Downloads = ThumbService.ValidateDownloads(item.Downloads)
                        });
            await Task.CompletedTask;
        }
    }
}