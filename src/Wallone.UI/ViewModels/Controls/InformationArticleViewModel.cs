﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Wallone.Core.Builders;
using Wallone.Core.Controllers;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Schedulers;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Pages;
using Wallone.Core.Services.Users;

namespace Wallone.UI.ViewModels.Controls
{
    public class InformationArticleViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private SinglePage singlePage;
        private ThemeCreatedBuilder themeBuilder;

        public InformationArticleViewModel()
        {
        }

        public InformationArticleViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            ProfileCommand = new DelegateCommand(OnProfileClicked);
            DownloadCommand = new DelegateCommand(OnDownloadClicked);
            InstallCommand = new DelegateCommand(OnInstallClicked);
            FavoriteCommand = new DelegateCommand(OnFavoriteClicked);
            ReactionCommand = new DelegateCommand(OnReactionClicked);
        }

        public SinglePageItemsViewModel SinglePageItemsViewModel { get; set; } = new SinglePageItemsViewModel();
        public SinglePageLogicViewModel SinglePageLogic { get; set; } = new SinglePageLogicViewModel();

        public ObservableCollection<ItemTemplateViewModel> CategoriesCollection { get; set; } =
            new ObservableCollection<ItemTemplateViewModel>();

        public ObservableCollection<ItemTemplateViewModel> TagsCollection { get; set; } =
            new ObservableCollection<ItemTemplateViewModel>();

        public DelegateCommand ProfileCommand { get; set; }

        public DelegateCommand DownloadCommand { get; set; }
        public DelegateCommand InstallCommand { get; set; }
        public DelegateCommand FavoriteCommand { get; set; }
        public DelegateCommand ReactionCommand { get; set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            singlePage = (SinglePage)navigationContext.Parameters["singlePage"];
            if (singlePage == null)
            {
                var param = new NavigationParameters
                {
                    {"Text", "Не найдена страница.."}
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
            TagsCollection.Clear();
            CategoriesCollection.Clear();
        }

        private void OnProfileClicked()
        {
            var user = SinglePageService.GetUser();

            var paramProfile = new NavigationParameters
            {
                {"id", user?.id},
                {"header", "Профиль"},
                {"name", user?.username},
                {"isProfile", UserRepository.UserService.IsUser(user?.id)}
            };
            regionManager.RequestNavigate("PageRegion", "Profile", paramProfile);
        }

        private async void OnDownloadClicked()
        {
            try
            {
                SinglePageLogic.IsEnableDownloaded = false;

                var settingsItemBuilder = new SettingsBuilder(SettingsRepository.Get())
                    .ItemBuilder();

                var links = singlePage.links;

                var builder = await themeBuilder
                    .HasDownloaded() //Если не установлена, проходим проверку
                    .ExistOrCreateDirectory() // Если папка существует или не создана
                    .Remove() //Если существует и статус false, то удалить
                    .SetLinksForDownload(links)
                    .ImageDownload(); //Разрешение на скачивание

                await builder.PreviewDownloadAsync();

                builder.CreateModel()
                    .Save()
                    .Build();

                if (builder.Exist())
                {
                    SinglePageLogic.IsEnableInstalled = true;

                    if(settingsItemBuilder.GetAutoSetImage())
                    {
                        OnInstallClicked();
                    }

                    var data =
                        await SinglePageService.SetDownloadAsync(
                            AppConvert.BoolToString(builder.GetHasNotDownloaded()));
                    Update(data);
                }
                else
                {
                    ThemeRepository.ThemeService.Disable();
                    SinglePageLogic.IsInstalled = false;
                    SinglePageLogic.IsEnableInstalled = false;
                }

                SinglePageLogic.IsDownloaded = builder.GetHasNotDownloaded();
                SinglePageLogic.IsEnableDownloaded = true;
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

        private void OnInstallClicked()
        {
            SinglePageLogic.IsEnableInstalled = false;

            var builder = themeBuilder
                .HasInstalled(SinglePageLogic.IsInstalled);

            var settingsItemBuilder = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder();

            if (builder.GetHasNotInstalled())
            {
               var location = settingsItemBuilder.GetLocation();

                var model = builder.GetThemeModelFromFile();

                var themeController = new ThemeController<Theme>(model,
                    new GeolocationController<Location>(location));

                if(themeController.ValidateFields())
                {
                    if (!ThemeScheduler.Is())
                    {
                        new ThemeScheduler(themeController);
                    }
                    ThemeScheduler.Refresh();

                    if (settingsItemBuilder.GetModelWindow())
                    {
                        regionManager.RequestNavigate("TabSunTimes", "TabSunTimes");
                    }
                }
                else
                {
                    var param = new NavigationParameters();
                    param.Add("text", themeController.Messages());

                    regionManager.RequestNavigate("TabSunTimes", "TabInformation", param);
                }
            }
            else
            {
                ThemeRepository.ThemeService.Disable();
            }

            SinglePageLogic.IsInstalled = builder.GetHasNotInstalled();

            SinglePageLogic.IsEnableInstalled = true;
        }

        private async void OnFavoriteClicked()
        {
            SinglePageLogic.IsEnableFavorited = false;

            themeBuilder
                .HasFavorited(SinglePageLogic.IsFavorited)
                .Build();

            var data =
                await SinglePageService.SetFavoriteAsync(AppConvert.BoolToString(themeBuilder.GetHasNotFavorited()));

            SinglePageLogic.IsFavorited = themeBuilder.GetHasNotFavorited();

            Update(data);

            SinglePageLogic.IsEnableFavorited = true;
        }

        private async void OnReactionClicked()
        {
            SinglePageLogic.IsEnableLiked = false;

            themeBuilder
                .HasNotLiked(SinglePageLogic.IsLiked)
                .Build();

            var data =
                await SinglePageService.SetReactionAsync(AppConvert.BoolToString(themeBuilder.GetHasNotLiked()));

            Update(data);

            SinglePageLogic.IsLiked = themeBuilder.GetHasNotLiked();
            SinglePageLogic.IsEnableLiked = true;
        }


        private void setStatusForButtons()
        {
            SinglePageLogic.IsFavorited = SinglePageService.GetFavorite();
            SinglePageLogic.IsLiked = SinglePageService.GetReaction();

            SinglePageLogic.IsEnableFavorited = SinglePageService.HasFavorite();
            SinglePageLogic.IsEnableLiked = SinglePageService.HasReaction();

            var themeName = themeBuilder.GetName();
            var themePath = themeBuilder.GetThemePath();

            if (AppSettingsRepository.AppSettingsService.ExistDirectory(themePath))
            {
                SinglePageLogic.IsEnableDownloaded = true;
                SinglePageLogic.IsDownloaded = true;

                SinglePageLogic.IsEnableInstalled = true;
                SinglePageLogic.IsInstalled = AppFormat.Compare(ThemeRepository.ThemeService.GetCurrentThemeName(), themeName);
            }
            else
            {
                SinglePageLogic.IsEnableDownloaded = singlePage.isActive;
                SinglePageLogic.IsEnableLiked = singlePage.isActive;

                SinglePageLogic.IsDownloaded = false;
                SinglePageLogic.IsInstalled = false;
                SinglePageLogic.IsEnableInstalled = false;
            }
        }

        private async void tags(List<Tag> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    TagsCollection.Add(new ItemTemplateViewModel { Text = item.name });
                    await Task.CompletedTask;
                }
            }
            else
            {
                TagsCollection.Add(new ItemTemplateViewModel { Text = "Unknown" });
                await Task.CompletedTask;
            }
        }

        private async void categories(List<CategoryShort> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    CategoriesCollection.Add(new ItemTemplateViewModel { Text = item.name });
                    await Task.CompletedTask;
                }
            }
            else
            {
                CategoriesCollection.Add(new ItemTemplateViewModel { Text = "Unknown" });
                await Task.CompletedTask;
            }
        }

        private void Update(SinglePage data)
        {
            SinglePageItemsViewModel.Views = data?.views ?? singlePage.views;
            SinglePageItemsViewModel.Likes = data?.likes ?? singlePage.likes;
            SinglePageItemsViewModel.Downloads = data?.downloads ?? singlePage.downloads;
        }

        public void Loaded()
        {
            SettingsRepository.Load();

            SinglePageItemsViewModel.Name = SinglePageService.GetHeader();
            SinglePageItemsViewModel.Username = SinglePageService.GetUsername();
            SinglePageItemsViewModel.Description = SinglePageService.GetDescription();
            SinglePageItemsViewModel.Likes = SinglePageService.GetLikes();
            SinglePageItemsViewModel.Views = SinglePageService.GetViews();
            SinglePageItemsViewModel.Downloads = SinglePageService.GetDownloads();
            SinglePageItemsViewModel.Brand = SinglePageService.GetBrand()?.name;
            SinglePageItemsViewModel.Date = SinglePageService.GetData();


            if (SinglePageService.GetAvatar() != null)
                SinglePageItemsViewModel.Avatar =
                    UriHelper.Get(SinglePageService.GetAvatar());

            themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .SetName(singlePage.name);

            categories(new List<CategoryShort>()
            {
                new CategoryShort()
                {
                    id = "0",
                    name = SinglePageService.GetCategory()
                }
            });
            tags(SinglePageService.GetTags());
            setStatusForButtons();
        }
    }
}