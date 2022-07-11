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
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
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

        private string header = "Библиотека";

        private bool isContent;

        private bool isInternet;

        private bool isLoading = true;

        private bool isNoItems = true;

        private int pagination = 1;
        private bool isNextPage;
        private int countPosts = 1;

        public WallpapersViewModel() { }

        public WallpapersViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ManagerViewModel = new ManagerViewModel(regionManager);
            ViewerScrollChangedCommand = new DelegateCommand<ScrollChangedEventArgs>(OnViewerScrollChanged);
        }

        public PageGalleryBuilder PageBuilder { get; private set; }

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

        public bool IsNoItems
        {
            get => isNoItems;
            set => SetProperty(ref isNoItems, value);
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
            var page = (string) navigationContext.Parameters["Page"];
            var pageId = (string)navigationContext.Parameters["ID"];
            Header = (string) navigationContext.Parameters["Text"] ?? "Библиотека";

            PageBuilder = new PageGalleryBuilder()
                .SetApplicationRouter(page)
                .SetPagination(pagination)
                .SetBrand(page, pageId)
                .SetCategory(page, pageId)
                .ValidateRouter()
                .CreatePageQuery();

            Loaded(PageBuilder.GetWebsiteRouter(), PageBuilder.GetPageQuery(), true);
        }

        public ObservableCollection<ArticleViewModel> Library { get; set; } =
            new ObservableCollection<ArticleViewModel>();

        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
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
            else if (data.offset100 - 100 == data.offset)
            {
                NewPage();
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
                Loaded(PageBuilder.GetWebsiteRouter(), PageBuilder.GetPageQuery(), false);
            }
        }

        public async void Loaded(string router, List<Parameter> parameters, bool isLoaded)
        {
            try
            {
                IsInternet = false;
                if (isLoaded)
                {
                    IsLoading = true;
                }

                isNextPage = false;

                var items = await ThumbService.GetThumbsAsync(router, parameters);
                await LoadImages(items);
                Validate();

                PageBuilder.ClearQuery();
                isNextPage = true;

                if (IsLoading)
                {
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }
        }

        private async Task LoadImages(List<Thumb> items)
        {
            if (ThumbService.IsNotNull(items))
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
            await Task.CompletedTask;
        }

        private void Validate()
        {
            var status = AppEthernetService.GetStatus();

            if (status == 0)
            {
                IsInternet = true;
            }
            else
            {
                IsNoItems = Library.Count == 0;
            }
        }
    }
}
