using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class SinglePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private SimplePage simplePage;
        private string id;

        private double width;
        public double Width { get => width; set => SetProperty(ref width, value); }

        private string username;
        public string Username { get => username; set => SetProperty(ref username, value); }
        private string header;
        public string Header { get => header; set => SetProperty(ref header, value); }
        private string data1;
        public string Data { get => data1; set => SetProperty(ref data1, value); }
        private string category;
        public string Category { get => category; set => SetProperty(ref category, value); }
        private string brand;
        public string Brand { get => brand; set => SetProperty(ref brand, value); }
        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private string likes;
        public string Likes { get => likes; set => SetProperty(ref likes, value); }

        private string views;
        public string Views { get => views; set => SetProperty(ref views, value); }
        public SinglePageViewModel()
        {

        }
        public SinglePageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["ID"];
            Header = (string)navigationContext.Parameters["Name"];
            Loaded(id);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public async void Loaded(string id)
        {
            try
            {
                var data = await SimplePageService.GetPageAsync(id);

                if (!string.IsNullOrEmpty(data))
                {
                    var jArray = JArray.Parse(data);
                    simplePage = JsonConvert.DeserializeObject<SimplePage>(jArray[0].ToString());
                    new SimplePageService(simplePage);

                    Header = SimplePageService.GetHeader();
                    Username = SimplePageService.GetUsername();
                    Description = SimplePageService.GetDescription();
                    Likes = SimplePageService.GetLikes();
                    Views = SimplePageService.GetViews();
                    Brand = SimplePageService.GetBrand().Name;
                    Category = SimplePageService.GetCategory().Name;
                    Data = SimplePageService.GetData();

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
    }
}