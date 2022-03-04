using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Interfaces;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;
        public ObservableCollection<ArticleViewModel> Library { get; set; } = new ObservableCollection<ArticleViewModel>();

        private string header = "Библиотека";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        public WallpapersViewModel()
        {
            Library.Add(new ArticleViewModel(regionManager));
            Library.Add(new ArticleViewModel(regionManager));
        }
        public WallpapersViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var root = (string)navigationContext.Parameters["Root"] ?? "Gallery";
            var page_id = (string)navigationContext.Parameters["ID"] ?? "Main";

            Trace.WriteLine(root + "//" + page_id);

            var pageBuilder = new PageBuilder() // Создаем билдер
                .Query(new PageGallaryBuilder()) //Говорим, что gallery
                .Catalog(root) //Устанввливам каталог
                .Page(page_id) //Устанавливаем страницу
                .Validate() //Валидация полученных значений
                .ShowAds(false) //Отображение рекламы
                .Build(); //Сборка

            Trace.WriteLine(pageBuilder.GetRouter());

            Header = pageBuilder.GetCatalog() + " " + pageBuilder.GetPage();
            Loaded(null, pageBuilder.GetRouter(), pageBuilder.GetFields());

        }

        public async void Loaded(string page, string router,List<Core.Models.Parameter> parameters)
        {
            Library.Clear();
            try
            {
                var items = await ThumbService.GetThumbsAsync(router, page, parameters);
                if (ThumbService.CheckItems(items))
                {
                    foreach (var item in items)
                    {
                        Library.Add(new ArticleViewModel(regionManager)
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
