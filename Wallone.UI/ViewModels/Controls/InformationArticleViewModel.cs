using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Wallone.Core.Builders;
using Wallone.Core.Controllers;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Schedulers;
using Wallone.Core.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class InformationArticleViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private SinglePage simplePage;
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
            simplePage = (SinglePage)navigationContext.Parameters["simplePage"];
            if (simplePage == null)
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
            var param = new NavigationParameters
            {
                {"id", simplePage.user.id},
                {"name", simplePage.user.username}
            };

            regionManager.RequestNavigate("PageRegion", "Profile", param);
        }

        private async void OnDownloadClicked()
        {
            try
            {
                SinglePageLogic.IsEnableDownloaded = false;

                Trace.WriteLine("Theme Download: " + SinglePageLogic.IsDownloaded);

                var builder = await themeBuilder
                    .HasDownloaded() //Если не установлена, проходим проверку
                    .ExistOrCreateDirectory() // Если папка существует или не создана
                    .Remove() //Если существует и статус false, то удалить
                    .SetImages(simplePage.links)
                    .ImageDownload(); //Разрешение на скачивание

                await builder.PreviewDownloadAsync();

                builder.CreateModel()
                    .Save()
                    .Build();

                if (builder.Exist())
                {
                    SinglePageLogic.IsEnableInstalled = true;
                }
                else
                {
                    SinglePageLogic.IsInstalled = false;
                    SinglePageLogic.IsEnableInstalled = false;
                }


                var data =
                    await SinglePageService.SetDownloadAsync(
                        AppConvert.BoolToString(builder.GetHasNotDownloaded()));

                Update(data);

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

            var theme = new ThemeController();

            if (builder.GetHasNotInstalled())
            {
                ThemeScheduler.Stop();
                theme.Load(builder.GetThemeModelFromFile()); // загружаем тему
                theme.Set(builder.GetThemeModelFromFile()); // устанавливаем как обои
                ThemeScheduler.SetInterval(1000);
                ThemeScheduler.Run();
                regionManager.RequestNavigate("TabSunTimes", "TabSunTimes");
            }
            else
            {
                ThemeService.Set(null);
            }

            SinglePageLogic.IsInstalled = builder.GetHasNotInstalled();

            SinglePageLogic.IsEnableInstalled = true;
        }

        private async void OnFavoriteClicked()
        {
            SinglePageLogic.IsEnableFavorited = false;
            Trace.WriteLine("Favorite");
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

            Trace.WriteLine("Like");

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

            Trace.WriteLine("(LOAD)Theme path:  " + themePath);

            if (AppSettingsService.ExistDirectory(themePath))
            {
                SinglePageLogic.IsDownloaded = true;

                SinglePageLogic.IsEnableInstalled = true;
                SinglePageLogic.IsInstalled = AppFormat.Compare(ThemeService.GetCurrentName(), themeName);
            }
            else
            {
                SinglePageLogic.IsEnableDownloaded = simplePage.isActive;
                SinglePageLogic.IsEnableLiked = simplePage.isActive;

                SinglePageLogic.IsDownloaded = false;
                SinglePageLogic.IsInstalled = false;
                SinglePageLogic.IsEnableInstalled = false;
            }

            Trace.WriteLine("(LOAD)Theme Favorited: " + SinglePageLogic.IsFavorited);
            Trace.WriteLine("(LOAD)Theme Liked: " + SinglePageLogic.IsLiked);

            Trace.WriteLine("(LOAD)Theme Downloaded: " + SinglePageLogic.IsDownloaded);
            Trace.WriteLine("(LOAD)Theme Installed: " + SinglePageLogic.IsInstalled);
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
            SinglePageItemsViewModel.Views = data?.views ?? simplePage.views;
            SinglePageItemsViewModel.Likes = data?.likes ?? simplePage.likes;
            SinglePageItemsViewModel.Downloads = data?.downloads ?? simplePage.downloads;
        }

        public void Loaded()
        {
            SettingsService.Load();

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

            Trace.WriteLine("(LOAD)Theme Name: " + simplePage.name);

            themeBuilder = new ThemeBuilder<ThemeCreatedBuilder>()
                .Query(new ThemeCreatedBuilder()) // Запрос к ThemeCreatedBuilder
                .SetName(AppFormat.Format(simplePage.name));

            categories(SinglePageService.GetCategories());
            tags(SinglePageService.GetTags());
            setStatusForButtons();
        }
    }
}