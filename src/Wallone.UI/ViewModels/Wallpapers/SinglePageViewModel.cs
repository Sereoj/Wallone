using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Pages;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.ViewModels.Wallpapers
{
    public class SinglePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public ManagerViewModel ManagerViewModel { get; }

        private string uuid;

        private bool isContent;

        private bool isLoading = true;
        private bool isPosts;

        private string name;
        private SinglePage singlePage;

        public SinglePageViewModel()
        {
        }

        //Вызывется раньше чем OnNavigatedTo, IsNavigationTarget, OnNavigatedFrom, инициализация
        public SinglePageViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            ManagerViewModel = new ManagerViewModel(regionManager);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

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

        public bool IsPosts
        {
            get => isLoading;
            set
            {
                SetProperty(ref isLoading, value);
                IsContent = value == false;
            }
        }

        public SinglePageAdsViewModel SinglePageAds { get; set; } = new SinglePageAdsViewModel();

        public ObservableCollection<ArticleViewModel> Posts { get; set; } =
            new ObservableCollection<ArticleViewModel>();


        //Вызывается после SinglePageViewModel, получает данные с другой страницы и отображает
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            uuid = (string) navigationContext.Parameters["Uuid"];
            Name = (string) navigationContext.Parameters["Name"] ?? "default";

            Loaded(uuid, name);
            LoadAds();
        }

        //Если данные получены отображение
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        // Отправка данных с этой страницы на другую страницу
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            GC.Collect(2);
        }

        public async void LoadAds()
        {
            try
            {
                var data = await SinglePageService.GetPageAdsAsync();

                if (!string.IsNullOrEmpty(data))
                {
                    var message = Json<Advertisement>.Decode(data);
                    SinglePageAds.IsVisible = true;
                    SinglePageAds.Text = message?.text;
                }
            }
            catch (Exception ex)
            {
                SinglePageAds.Text = ex.Message;
            }
        }

        public async void Loaded(string pageId, string pageName)
        {
            try
            {
                IsLoading = true;

                var theme = new ThemeCreatedBuilder()
                    .SetName(pageName);

                var data = await SinglePageService.GetPageAsync(uuid);
                singlePage = Json<SinglePage>.Decode(data);

                if (singlePage != null)
                {
                    if (theme.Exist())
                    {
                        if (theme.ValidateConfig())
                        {
                            var themeModel = theme.GetModelFromFile();

                            if (AppEthernetService.GetStatus() != HttpStatusCode.OK)
                            {
                                singlePage.name = themeModel.name;
                                //singlePage.uuid = themeModel.uuid;
                                if (singlePage.user == null && themeModel.user != null)
                                {
                                    singlePage.user = themeModel.user;
                                }
                            }
                            //singlePage.images = themeModel.images;
                        }
                        else
                        {
                            ManagerViewModel.Show(Pages.NotFound, "Ошибка конфигурации");
                        }
                    }


                    SinglePageService.Load(singlePage);

                    Name = SinglePageService.GetHeader();

                    var param = new NavigationParameters
                    {
                        {"singlePage", singlePage}
                    };
                    regionManager.RequestNavigate("Slider", "ImagePreview", param);
                    regionManager.RequestNavigate("Information", "InformationArticle", param);


                    if (IsNotPosts(SinglePageService.GetPosts()))
                    {
                        posts(SinglePageService.GetPosts());
                        IsPosts = true;
                    }
                    else
                    {
                        IsPosts = false;
                    }
                }

                IsLoading = false;
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }

            GC.Collect(1, GCCollectionMode.Forced);
        }

        private bool IsNotPosts(List<Thumb> thumbs)
        {
            return ThumbService.IsNotNull(thumbs);
        }

        private async void posts(List<Thumb> list)
        {
            Posts.Clear();
            try
            {
                foreach (var item in list)
                {
                    Posts.Add(new ArticleViewModel(regionManager)
                    {
                        Uuid = item.Uuid,
                        Name = item.Name,
                        ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                    });
                    await Task.CompletedTask;
                }
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }
        }
    }
}