using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.UI.Interfaces;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.ViewModels.Wallpapers
{
    public class DownloadsPageViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;

        private string header = "Установленные";
        private bool isContent;
        private bool isLoading;

        public DownloadsPageViewModel()
        {
        }

        public DownloadsPageViewModel(IRegionManager regionManager)
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

        public bool IsContent
        {
            get => isContent;
            set => SetProperty(ref isContent, value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Task.WhenAll(Loaded());
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Library.Clear();
            GC.Collect(2);
        }

        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public ObservableCollection<ArticleViewModel> Library { get; set; } =
            new ObservableCollection<ArticleViewModel>();

        public async Task Loaded()
        {
            try
            {
                IsLoading = true;

                var themeDirectory = AppSettingsService.GetThemesLocation();

                if (AppSettingsService.ExistDirectory(themeDirectory))
                {
                    AppSettingsService.CreateDirectory(themeDirectory);
                }

                foreach (var filePath in Directory.EnumerateFiles(AppSettingsService.GetThemesLocation(), "theme.json",
                             SearchOption.AllDirectories))
                {
                    var jsonText = await File.ReadAllTextAsync(filePath);

                    if (JsonHelper.IsValidJson(jsonText))
                    {
                        var item = JsonConvert.DeserializeObject<Theme>(jsonText);

                        if (item == null) continue;
                        if (!ThumbService.IsIdNotNull(item.Id)) continue;

                        Library.Add(new ArticleViewModel(regionManager)
                        {
                            ID = item.Id,
                            Name = ThumbService.ValidateName(item.Name),
                            ImageSource = BitmapHelper.CreateBitmapImage(ThumbService.ValidatePreview(UriHelper.Get(item.Preview))),
                            Views = ThumbService.ValidateViews("0"),
                            Downloads = ThumbService.ValidateDownloads("0")
                        });
                    }
                }

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
    }
}