using ModernWpf.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
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
    public class SinglePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private SinglePage simplePage;
        private readonly BitmapHelper bitmapHelper;
        private string id;

        public ObservableCollection<ArticleViewModel> Posts { get; set; } = new ObservableCollection<ArticleViewModel>();
        public ObservableCollection<ItemTemplateViewModel> Categories { get; set; } = new ObservableCollection<ItemTemplateViewModel>();
        public ObservableCollection<ItemTemplateViewModel> Tags { get; set; } = new ObservableCollection<ItemTemplateViewModel>();
        private string username;
        public string Username { get => username; set => SetProperty(ref username, value); }

        private System.Windows.Media.ImageSource avatar;
        public System.Windows.Media.ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }
        private string header;
        public string Header { get => header; set => SetProperty(ref header, value); }
        private string data1;
        public string Data { get => data1; set => SetProperty(ref data1, value); }
        private string brand;
        public string Brand { get => brand; set => SetProperty(ref brand, value); }
        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private string likes;
        public string Likes { get => likes; set => SetProperty(ref likes, value); }

        private string views;
        public string Views { get => views; set => SetProperty(ref views, value); }

        private string downloads;
        public string Downloads { get => downloads; set => SetProperty(ref downloads, value); }

        private string installStatus;
        public string InstallStatus { get => installStatus; set => SetProperty(ref installStatus, value); }

        private string installText;
        public string InstallText { get => installText; set => SetProperty(ref installText, value); }

        private string favoriteStatus;
        public string FavoriteStatus { get => favoriteStatus; set => SetProperty(ref favoriteStatus, value); }

        private FontIcon favoriteText;
        public FontIcon FavoriteText { get => favoriteText; set => SetProperty(ref favoriteText, value); }

        private string reactionStatus;
        public string ReactionStatus { get => reactionStatus; set => SetProperty(ref reactionStatus, value); }

        private FontIcon reactionText;
        public FontIcon ReactionText { get => reactionText; set => SetProperty(ref reactionText, value); }

        private string ads;
        public string Ads { get => ads; set => SetProperty(ref ads, value); }

        public DelegateCommand ProfileCommand { get; set; }
        public DelegateCommand InstallCommand { get; set; }
        public DelegateCommand FavoriteCommand { get; set; }
        public DelegateCommand ReactionCommand { get; set; }

        public SinglePageViewModel()
        {
            Categories.Add(new ItemTemplateViewModel() { Text = "Test" });
            Categories.Add(new ItemTemplateViewModel() { Text = "Test1" });
        }

        public SinglePageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;

            bitmapHelper = new BitmapHelper();

            ProfileCommand = new DelegateCommand(OnProfileClicked);
            InstallCommand = new DelegateCommand(OnThemeInstalled);
            FavoriteCommand = new DelegateCommand(OnThemeFavorited);
            ReactionCommand = new DelegateCommand(OnReaction);
        }

        private void OnProfileClicked()
        {
            var param = new NavigationParameters
            {
                { "id", simplePage.user.id },
                { "name", simplePage.user.name }
            };

            regionManager.RequestNavigate("PageRegion", "Profile", param);
        }

        private async void OnThemeInstalled()
        {
            InstallText = InstallStatus == "true" ? "Установить" : "Удалить";
            InstallStatus = InstallStatus == "false" ? "true" : "false";

            SinglePage data = await SinglePageService.SetDownloadAsync(InstallStatus);
            update(data);
        }

        private async void OnThemeFavorited()
        {
            FavoriteText = FavoriteStatus == "true" ? FontIconService.SetIcon("ultimate", "\uECB8") : FontIconService.SetIcon("ultimate", "\uECB7");
            FavoriteStatus = FavoriteStatus == "false" ? "true" : "false";
            SinglePage data = await SinglePageService.SetFavoriteAsync(FavoriteStatus);
            update(data);
        }

        private async void OnReaction()
        {
            ReactionText = ReactionStatus == "true" ? FontIconService.SetIcon("ultimate", "\uECEA") : FontIconService.SetIcon("ultimate", "\uECE9");
            ReactionStatus = ReactionStatus == "false" ? "true" : "false";
            SinglePage data = await SinglePageService.SetReactionAsync(ReactionStatus);
            update(data);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["ID"];
            Header = (string)navigationContext.Parameters["Name"];
            Loaded(id);
            LoadAds();
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
                    Ads = message.text ?? "Не удалось загрузить =(";
                }
            }
            catch (Exception ex)
            {

                Ads = ex.Message;
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

                    Header = SinglePageService.GetHeader();
                    Username = SinglePageService.GetUsername();
                    Description = SinglePageService.GetDescription();
                    Likes = SinglePageService.GetLikes();
                    Views = SinglePageService.GetViews();
                    Downloads = SinglePageService.GetDownloads();
                    Brand = SinglePageService.GetBrand()?.Name;
                    Data = SinglePageService.GetData();

                    if(SinglePageService.GetAvatar() != null)
                    {
                        Avatar = (ImageSource)bitmapHelper[UriHelper.Get(SinglePageService.GetAvatar())];
                    }

                    categories(SinglePageService.GetCategories());
                    tags(SinglePageService.GetTags());
                    posts(SinglePageService.GetPosts());
                    setButtons();

                    bitmapHelper.Clear();

                    var param = new NavigationParameters
                    {
                        { "simplePage", simplePage }
                    };
                    regionManager.RequestNavigate("Slider", "ImagePreview", param);
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
        private void setButtons()
        {
            InstallStatus = SinglePageService.GetInstall();
            InstallText = SinglePageService.GetInstall() != "true" ? "Установить" : "Удалить";

            FavoriteStatus = SinglePageService.GetFavorite();
            FavoriteText = SinglePageService.GetFavorite() != "true" ? FontIconService.SetIcon("ultimate", "\uECB8") : FontIconService.SetIcon("ultimate", "\uECB7");

            ReactionStatus = SinglePageService.GetReaction();
            ReactionText = SinglePageService.GetReaction() != "true" ? FontIconService.SetIcon("ultimate", "\uECEA") : FontIconService.SetIcon("ultimate", "\uECE9");
        }
        private async void tags(List<Tag> list)
        {
            Tags.Clear();

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    Tags.Add(new ItemTemplateViewModel { Text = item.name });
                    await Task.CompletedTask;
                }
            }
            else
            {
                Tags.Add(new ItemTemplateViewModel { Text = "Unknown" });
                await Task.CompletedTask;
            }
        }
        private async void categories(List<Category> list)
        {
            Categories.Clear();

            if(list.Count > 0)
            {
                foreach (var item in list)
                {
                    Categories.Add(new ItemTemplateViewModel { Text = item.Name });
                    await Task.CompletedTask;
                }
            }
            else
            {
                Categories.Add(new ItemTemplateViewModel { Text = "Unknown" });
                await Task.CompletedTask;
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

        private void update(SinglePage data)
        {
            Views = data?.views ?? simplePage.views;
            Likes = data?.likes ?? simplePage.likes;
            Downloads = data?.downloads ?? simplePage.likes;
        }
    }
}