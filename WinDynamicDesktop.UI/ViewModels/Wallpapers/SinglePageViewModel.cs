using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class SinglePageAdsViewModel : BindableBase
    {
        private string text;
        public string Text { get => text; set => SetProperty(ref text, value); }

        private ImageSource imageSource;
        public ImageSource ImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        private string link;
        public string Link { get => link; set => SetProperty(ref link, value); }

        private bool isVisible;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
    }
    public class SinglePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private SinglePage simplePage;
        private readonly BitmapHelper bitmapHelper;
        private string id = null;

        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        public SinglePageAdsViewModel SinglePageAds { get; set; } = new SinglePageAdsViewModel();
        public ObservableCollection<ArticleViewModel> Posts { get; set; } = new ObservableCollection<ArticleViewModel>();

        public SinglePageViewModel()
        {
        }

        public SinglePageViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            bitmapHelper = new BitmapHelper();
        }

   

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["ID"];

            if (id != null)
            {
                Loaded(id);
                LoadAds();
            }
            else
            {
                var param = new NavigationParameters
                {
                    { "Text", "Не найдена страница.." }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void LoadAds()
        {
            try
            {
                var data = await SinglePageService.GetPageAdsAsync();

                if (!string.IsNullOrEmpty(data))
                {
                    var message = JsonConvert.DeserializeObject<Text>(data);
                    SinglePageAds.IsVisible = true;
                    SinglePageAds.Text = message.text ?? "Не удалось загрузить =(";
                }
            }
            catch (Exception ex)
            {

                SinglePageAds.Text = ex.Message;
            }
        }
        public async void Loaded(string id)
        {
            try
            {
                var data = await SinglePageService.GetPageAsync(id);

                if (!string.IsNullOrEmpty(data))
                {
                    //var jArray = JArray.Parse(data);
                    simplePage = JsonConvert.DeserializeObject<SinglePage>(data);
                        
                    SinglePageService.Load(simplePage);

                    Name = SinglePageService.GetHeader();

                    var param = new NavigationParameters
                    {
                        { "simplePage", simplePage }
                    };
                    regionManager.RequestNavigate("Slider", "ImagePreview", param);
                    regionManager.RequestNavigate("Information", "InformationArticle", param);

                    posts(SinglePageService.GetPosts());
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
        private async void posts(List<Thumb> list)
        {
            Posts.Clear();
            try
            {
                if (ThumbService.CheckItems(list))
                {
                    foreach (var item in list)
                    {
                        Posts.Add(new ArticleViewModel(regionManager)
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