using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Pages;
using Wallone.UI.Interfaces;
using Wallone.UI.Services;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.ViewModels.Wallpapers
{
    public class WallpapersViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;

        public ManagerViewModel ManagerViewModel { get; }

        private string header;

        private bool isContent;

        private bool isLoading = true;

        private bool isNoItems;

        private int pagination = 1;
        private bool isNextPage;
        private int countPosts;

        public WallpapersViewModel()
        {
        }

        public WallpapersViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ManagerViewModel = new ManagerViewModel(regionManager);
            ViewerScrollChangedCommand = new DelegateCommand<ScrollChangedEventArgs>(OnViewerScrollChanged);
        }

        public PageGalleryBuilder PageBuilder { get; private set; } = new PageGalleryBuilder();

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

        public bool IsContent
        {
            get => isContent;
            set => SetProperty(ref isContent, value);
        }

        public bool IsNoItems
        {
            get => isNoItems;
            set
            {
                SetProperty(ref isNoItems, value);
                IsContent = value == false;
            }
        }

        public ObservableCollection<ArticleViewModel> Library { get; set; } =
            new ObservableCollection<ArticleViewModel>();

        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            pagination = 1;
            Library.Clear();
            GC.Collect(2);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var page = (string)navigationContext.Parameters["Page"];
            var pageId = (string)navigationContext.Parameters["ID"];
            Header = (string)navigationContext.Parameters["Text"];

            PageBuilder
                .SetApplicationRouter(page)
                .SetPagination(pagination)
                .SetCategory(page, pageId)
                .ValidateRouter()
                .CreatePageQuery();

            _ = Loaded(PageBuilder.GetWebsiteRouter(), PageBuilder.GetPageQuery(), true);
            _ = Loaded(PageBuilder.GetWebsiteRouter(), PageBuilder.GetPageQuery(), true);
        }

        private void OnViewerScrollChanged(ScrollChangedEventArgs e)
        {
            var data = ScrollViewerService.Get(ref e);

            if (data.percent80 < data.offset && data.percent90 > data.offset)
            {
                if (isNextPage && countPosts != 0)
                {
                    NewPage();
                }
            }
        }

        private void NewPage()
        {
            if (isNextPage && countPosts != 0)
            {
                pagination++;
                PageBuilder.SetPagination(pagination)
                    .ValidateRouter()
                    .CreatePageQuery();
                _ = Loaded(PageBuilder.GetWebsiteRouter(), PageBuilder.GetPageQuery(), false);
            }
        }

        /*
         * Отображение анимации
         */
        private void SetLoading(bool value, bool revert = false)
        {
            if (revert)
            {
                IsLoading = false;
                return;
            }

            if (value)
            {
                IsLoading = true;
            }
        }

        public async Task Loaded(string router, List<Parameter> parameters, bool isLoaded)
        {
            try
            {

                SetLoading(isLoaded);
                isNextPage = false;

                var items = await ThumbService.GetThumbsAsync(router, parameters);
                var statusCode = AppEthernetService.GetStatus();

                if (Validate(items) && statusCode == HttpStatusCode.OK)
                {
                    LoadImages(items);
                    PageBuilder.ClearQuery();
                    isNextPage = true;
                }
                SetLoading(isLoaded, true);
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }
        }

        private static bool Validate(List<Thumb> items)
        {
            if (!ThumbService.IsNotNull(items))
                return false;
            return items.Count != 0;
        }

        private void LoadImages(List<Thumb> items)
        {
            countPosts = items.Count;

            foreach (var item in items)
            {
                if (ThumbService.IsIdNotNull(item.Uuid))
                {
                    Library.Add(new ArticleViewModel(regionManager)
                    {
                        Uuid = item.Uuid,
                        Name = ThumbService.ValidateName(item.Name),
                        ImageSource = new BitmapImage(UriHelper.Get(item.Preview)),
                        Views = ThumbService.ValidateViews(item.Views),
                        Downloads = ThumbService.ValidateDownloads(item.Downloads)
                    });
                }
            }
        }
    }
}
