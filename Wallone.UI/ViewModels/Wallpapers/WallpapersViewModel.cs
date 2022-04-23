using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.UI.Interfaces;
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

        private string router;
        private List<Parameter> parameters;

        public WallpapersViewModel()
        {
            Library.Add(new ArticleViewModel(regionManager));
            Library.Add(new ArticleViewModel(regionManager));
        }

        public WallpapersViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
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
            GC.Collect(2);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var root = (string) navigationContext.Parameters["Root"] ?? "Gallery";
            var page_id = (string) navigationContext.Parameters["ID"] ?? "Main";

            var pageBuilder = new PageBuilder() // Создаем билдер
                .Query(new PageGallaryBuilder()) //Говорим, что gallery
                .Catalog(root) //Устанввливам каталог
                .Page(page_id) //Устанавливаем страницу
                .Validate() //Валидация полученных значений
                .ShowAds(false) //Отображение рекламы
                .Build(); //Сборка

            router = pageBuilder.GetRouter();
            parameters = pageBuilder.GetFields();

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
                Loaded(null, router, parameters);
                router = null;
                parameters = null;
            }
        }

        public async void Loaded(string page, string router, List<Parameter> parameters)
        {
            Library.Clear();
            try
            {
                IsLoading = true;

                var items = await ThumbService.GetThumbsAsync(router, page, parameters);
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
            if (ThumbService.CheckItems(items))
            {
                foreach (var item in items)
                {
                    Library.Add(new ArticleViewModel(regionManager)
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ImageSource = new BitmapImage(UriHelper.Get(item.Preview)),
                        Views = item.Views,
                        Downloads = item.Downloads
                    });
                    await Task.CompletedTask;
                }
            }
            else
            {
                var param = new NavigationParameters
                {
                    {"Text", "Это не ошибка, просто не найдены изображения!"}
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}