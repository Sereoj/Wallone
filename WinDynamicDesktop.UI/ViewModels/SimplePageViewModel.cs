using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class SimplePageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;
        private readonly IEventAggregator eventAggregator;
        private SimplePage simplePage;
        private string id;

        private string header;
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set { SetProperty(ref username, value); }
        }
        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public SimplePageViewModel()
        {

        }
        public SimplePageViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
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
            var data = await SimplePageService.GetPageAsync(id);

            try
            {
                var jArray = JArray.Parse(data);
                simplePage = JsonConvert.DeserializeObject<SimplePage>(jArray[0].ToString());
                new SimplePageService(simplePage);

                Header = SimplePageService.GetHeader();
                Username = SimplePageService.GetUsername();
                Description = SimplePageService.GetDescription();

                var param = new NavigationParameters
                {
                    { "simplePage", simplePage }
                };
                regionManager.RequestNavigate("Slider", "ImagePreview", param);
            }
            catch(Exception ex)
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