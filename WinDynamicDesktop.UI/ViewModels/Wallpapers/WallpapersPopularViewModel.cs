﻿using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Interfaces;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class WallpapersPopularViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;
        private string header = "Популярные";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        public ObservableCollection<ArticleViewModel> Library { get; set; } = new ObservableCollection<ArticleViewModel>();
        public WallpapersPopularViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Loaded();
        }

        public async void Loaded()
        {
            try
            {
                var items = await ThumbService.GetThumbsAsync("popular", null);
                if(ThumbService.CheckItems(items))
                {
                    foreach (var item in items)
                    {
                        Library.Add(new ArticleViewModel(regionManager)
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                        });
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