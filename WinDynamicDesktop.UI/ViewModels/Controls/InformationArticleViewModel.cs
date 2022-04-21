using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.Core.Controllers;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels.Controls
{
    public class InformationArticleViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly BitmapHelper bitmapHelper;
        private SinglePage simplePage;
        public SinglePageItemsViewModel SinglePageItemsViewModel { get; set; } = new SinglePageItemsViewModel();
        public SinglePageLogicViewModel SinglePageLogic { get; set; } = new SinglePageLogicViewModel();

        public ObservableCollection<ItemTemplateViewModel> Categories { get; set; } = new ObservableCollection<ItemTemplateViewModel>();
        public ObservableCollection<ItemTemplateViewModel> Tags { get; set; } = new ObservableCollection<ItemTemplateViewModel>();

        public DelegateCommand ProfileCommand { get; set; }
        public DelegateCommand InstallCommand { get; set; }
        public DelegateCommand FavoriteCommand { get; set; }
        public DelegateCommand ReactionCommand { get; set; }
        public InformationArticleViewModel()
        {

        }

        public InformationArticleViewModel(IRegionManager regionManager)
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
            try
            {
                SinglePageLogic.IsEnableInstalled = false;
                var themeBuilder = await new ThemeBuilder<ThemeCreatedBuilder>()
                    .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                    .SetName(simplePage.name)
                    .CreateModel(simplePage.images) //Создаем модель данных
                    .HasNotInstalled(SinglePageLogic.IsInstalled) //Если не установлена, прооходим проверку
                    .ExistOrCreateDirectory() // Если папка существует или не создана
                    .Remove() //Если существует и статус false, то удалить
                    .Download(); //Разрешение на скачивание

                themeBuilder.Build();
                var themeController = new ThemeController(themeBuilder);
                themeController.SetWallpaper();

                SinglePage data = await SinglePageService.SetDownloadAsync(AppConvert.BoolToString(themeController.GetValueInstall()));

                update(data);
                SinglePageLogic.IsInstalled = themeController.GetValueInstall();
                SinglePageLogic.IsEnableInstalled = true;

            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private async void OnThemeFavorited()
        {
            try
            {
                SinglePageLogic.IsEnableFavorited = false;

                var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                    .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                    .HasNotFavorited(SinglePageLogic.IsFavorited); //Если не установлена, проходим проверку

                //Закидываем выполненные настройки в контроллер для отображения данных
                var themeController = new ThemeController(themeBuilder);

                SinglePage data = await SinglePageService.SetFavoriteAsync(AppConvert.BoolToString(themeController.GetValueFavorite()));
                update(data);

                SinglePageLogic.IsFavorited = themeController.GetValueFavorite();
                SinglePageLogic.IsEnableFavorited = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void OnReaction()
        {
            try
            {
                SinglePageLogic.IsEnableLiked = false;

                var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                    .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                    .HasNotLiked(SinglePageLogic.IsLiked); //Если не установлена, проходим проверку

                //Закидываем выполненные настройки в контроллер для отображения данных
                var themeController = new ThemeController(themeBuilder);

                SinglePage data = await SinglePageService.SetReactionAsync(AppConvert.BoolToString(themeController.GetValueReaction()));
                update(data);

                SinglePageLogic.IsLiked = themeController.GetValueReaction();
                SinglePageLogic.IsEnableLiked = true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async void setStatusForButtons()
        {
            var themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .SetName(simplePage.name)
                .HasNotInstalled(SinglePageService.GetInstall()); //Если не установлена, прооходим проверку

            SinglePageLogic.IsFavorited = SinglePageService.GetFavorite();
            SinglePageLogic.IsLiked = SinglePageService.GetReaction();

            var theme = themeBuilder.GetThemePath();
            SinglePageLogic.IsInstalled = AppSettingsService.ExistDirectory(theme);

            SinglePage data = await SinglePageService.SetDownloadAsync(AppConvert.BoolToString(SinglePageLogic.IsInstalled)); // true
            update(data); // update ui
        }

        private async void tags(List<Tag> list)
        {
            Tags.Clear();

            if (list != null)
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

            if (list != null)
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

        private void update(SinglePage data)
        {
            SinglePageItemsViewModel.Views = data?.views ?? simplePage.views;
            SinglePageItemsViewModel.Likes = data?.likes ?? simplePage.likes;
            SinglePageItemsViewModel.Downloads = data?.downloads ?? simplePage.likes;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            simplePage = (SinglePage)navigationContext.Parameters["simplePage"];
            if (simplePage == null)
            {
                var param = new NavigationParameters
                {
                    { "Text", "Не найдена страница.." }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
            else
            {
                Loaded();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void Loaded()
        {
            //SinglePageItems

            SinglePageItemsViewModel.Name = SinglePageService.GetHeader();
            SinglePageItemsViewModel.Username = SinglePageService.GetUsername();
            SinglePageItemsViewModel.Description = SinglePageService.GetDescription();
            SinglePageItemsViewModel.Likes = SinglePageService.GetLikes();
            SinglePageItemsViewModel.Views = SinglePageService.GetViews();
            SinglePageItemsViewModel.Downloads = SinglePageService.GetDownloads();
            SinglePageItemsViewModel.Brand = SinglePageService.GetBrand()?.Name;
            SinglePageItemsViewModel.Date = SinglePageService.GetData();

            if (SinglePageService.GetAvatar() != null)
            {
                SinglePageItemsViewModel.Avatar = (ImageSource)bitmapHelper[UriHelper.Get(SinglePageService.GetAvatar())];
            }

            categories(SinglePageService.GetCategories());
            tags(SinglePageService.GetTags());
            setStatusForButtons();

            bitmapHelper.Clear();
        }

    }
}
