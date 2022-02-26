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
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.Core.Controllers;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class SinglePageItemsViewModel : BindableBase
    {
        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        //Имя пользователя
        private string username;
        public string Username { get => username; set => SetProperty(ref username, value); }

        //Аватар
        private ImageSource avatar;
        public ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }

        //Дата публикации
        private string date;
        public string Date { get => date; set => SetProperty(ref date, value); }

        //Бренд
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
    }

    public class SinglePageAdsViewModel : BindableBase
    {
        private string text;
        public string Text { get => text; set => SetProperty(ref text, value); }

        private string link;
        public string Link { get => link; set => SetProperty(ref link, value); }

        private string isVisible;
        public string IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
    }

    public class SinglePageLogicViewModel : BindableBase
    {
        private string displayTextInstall;
        public string DisplayTextInstall { get => displayTextInstall; set => SetProperty(ref displayTextInstall, value); }

        private FontIcon displayTextFavorite;
        public FontIcon DisplayTextFavorite { get => displayTextFavorite; set => SetProperty(ref displayTextFavorite, value); }

        private FontIcon displayTextReation;
        public FontIcon DisplayTextReation { get => displayTextReation; set => SetProperty(ref displayTextReation, value); }

     
        private bool isInstalled;
        public bool IsInstalled
        { 
            get => isInstalled;
            set
            {
                DisplayTextInstall = value == true ? "Удалить" : "Установить";
                SetProperty(ref isInstalled, value);
            }
        }

        private bool isFavorited;
        public bool IsFavorited
        { 
            get => isFavorited;
            set
            {
                DisplayTextFavorite = value == true ? FontIconService.SetIcon("ultimate", "\uECB7") : FontIconService.SetIcon("ultimate", "\uECB8");
                SetProperty(ref isFavorited, value);
            }
        }

        private bool isLiked;
        public bool IsLiked
        {
            get => isLiked;
            set
            {
                DisplayTextReation = value == true ? FontIconService.SetIcon("ultimate", "\uECE9") : FontIconService.SetIcon("ultimate", "\uECEA");
                SetProperty(ref isLiked, value);
            }
        }
    }
    public class SinglePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private SinglePage simplePage;
        private readonly BitmapHelper bitmapHelper;
        private string id;

        public SinglePageItemsViewModel SinglePageItemsViewModel { get; set; } = new SinglePageItemsViewModel();
        public SinglePageAdsViewModel SinglePageAds { get; set; } = new SinglePageAdsViewModel();
        public SinglePageLogicViewModel SinglePageLogic { get; set; } = new SinglePageLogicViewModel();
        public ObservableCollection<ArticleViewModel> Posts { get; set; } = new ObservableCollection<ArticleViewModel>();
        public ObservableCollection<ItemTemplateViewModel> Categories { get; set; } = new ObservableCollection<ItemTemplateViewModel>();
        public ObservableCollection<ItemTemplateViewModel> Tags { get; set; } = new ObservableCollection<ItemTemplateViewModel>();

        public DelegateCommand ProfileCommand { get; set; }
        public DelegateCommand InstallCommand { get; set; }
        public DelegateCommand FavoriteCommand { get; set; }
        public DelegateCommand ReactionCommand { get; set; }

        public SinglePageViewModel()
        {
            Categories.Add(new ItemTemplateViewModel() { Text = "Test" });
            Categories.Add(new ItemTemplateViewModel() { Text = "Test1" });
        }

        public SinglePageViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

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
            var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .SetName(simplePage.name)
                .CreateModel() //Создаем модель данных
                .HasNotInstalled(SinglePageLogic.IsInstalled) //Если не установлена, прооходим проверку
                .ExistOrCreateDirectory() // Если папка существует или не создана
                .Remove() //Если существует и статус false, то удалить
                .Download() //Разрешение на скачивание
                .Build(); //Создаем конфиг

            var themeController = new ThemeController(themeBuilder);
            
            SinglePage data = await SinglePageService.SetDownloadAsync(AppConvert.BoolToString(themeController.GetValueInstall()));
            update(data);

            SinglePageLogic.IsInstalled = themeController.GetValueInstall();

        }

        private async void OnThemeFavorited()
        {
            var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .HasNotFavorited(SinglePageLogic.IsFavorited); //Если не установлена, проходим проверку

            //Закидываем выполненные настройки в контроллер для отображения данных
            var themeController = new ThemeController(themeBuilder);

            SinglePage data = await SinglePageService.SetFavoriteAsync(AppConvert.BoolToString(themeController.GetValueFavorite()));
            update(data);

            SinglePageLogic.IsFavorited = themeController.GetValueFavorite();
        }

        private async void OnReaction()
        {
            var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .HasNotLiked(SinglePageLogic.IsLiked); //Если не установлена, проходим проверку

            //Закидываем выполненные настройки в контроллер для отображения данных
            var themeController = new ThemeController(themeBuilder);

            SinglePage data = await SinglePageService.SetReactionAsync(AppConvert.BoolToString(themeController.GetValueReaction()));
            update(data);

            SinglePageLogic.IsLiked = themeController.GetValueReaction();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["ID"];
            SinglePageItemsViewModel.Name = (string)navigationContext.Parameters["Name"];
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

                    //SinglePageItems

                    SinglePageItemsViewModel.Name = SinglePageService.GetHeader();
                    SinglePageItemsViewModel.Username = SinglePageService.GetUsername();
                    SinglePageItemsViewModel.Description = SinglePageService.GetDescription();
                    SinglePageItemsViewModel.Likes = SinglePageService.GetLikes();
                    SinglePageItemsViewModel.Views = SinglePageService.GetViews();
                    SinglePageItemsViewModel.Downloads = SinglePageService.GetDownloads();
                    SinglePageItemsViewModel.Brand = SinglePageService.GetBrand()?.Name;
                    SinglePageItemsViewModel.Date = SinglePageService.GetData();

                    if(SinglePageService.GetAvatar() != null)
                    {
                        SinglePageItemsViewModel.Avatar = (ImageSource)bitmapHelper[UriHelper.Get(SinglePageService.GetAvatar())];
                    }

                    categories(SinglePageService.GetCategories());
                    tags(SinglePageService.GetTags());
                    posts(SinglePageService.GetPosts());
                    setStatusForButtons();

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
        private void setStatusForButtons()
        {
            var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .SetName(simplePage.name)
                .HasNotInstalled(SinglePageService.GetInstall()); //Если не установлена, прооходим проверку

            SinglePageLogic.IsInstalled = SinglePageService.GetInstall();
            SinglePageLogic.IsFavorited = SinglePageService.GetFavorite();
            SinglePageLogic.IsLiked = SinglePageService.GetReaction();

            if (themeBuilder.GetHasNotInstalled() == false)
            {
                SinglePageLogic.IsInstalled = AppConvert.Revert(themeBuilder.GetHasNotInstalled());
            }
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
            SinglePageItemsViewModel.Views = data?.views ?? simplePage.views;
            SinglePageItemsViewModel.Likes = data?.likes ?? simplePage.likes;
            SinglePageItemsViewModel.Downloads = data?.downloads ?? simplePage.likes;
        }
    }
}