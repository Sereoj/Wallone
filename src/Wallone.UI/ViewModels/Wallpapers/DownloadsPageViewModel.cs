using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Pages;
using Wallone.UI.Interfaces;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.ViewModels.Wallpapers
{
    public class DownloadsPageViewModel : BindableBase, INavigationAware, IPage
    {
        private readonly IRegionManager regionManager;

        public ManagerViewModel ManagerViewModel { get; }

        private string header = "Установленные";
        private bool isContent;
        private bool isLoading;

        public DownloadsPageViewModel()
        {
        }

        public DownloadsPageViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ManagerViewModel = new ManagerViewModel(regionManager);
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

                var themeDirectory = AppSettingsRepository.AppSettingsService.GetThemesLocation();

                if (AppSettingsRepository.AppSettingsService.ExistDirectory(themeDirectory))
                    AppSettingsRepository.AppSettingsService.CreateDirectory(themeDirectory);

                foreach (var filePath in Directory.EnumerateFiles(AppSettingsRepository.AppSettingsService.GetThemesLocation(), "theme.json",
                             SearchOption.AllDirectories))
                {
                    var jsonText = await File.ReadAllTextAsync(filePath);

                    if (JsonHelper.IsValidJson(jsonText))
                    {
                        var item = JsonConvert.DeserializeObject<Theme>(jsonText);

                        if (item == null) continue;
                        if (!ThumbService.IsIdNotNull(item.Uuid)) continue;

                        Library.Add(new ArticleViewModel(regionManager)
                        {
                            Uuid = item.Uuid,
                            Name = ThumbService.ValidateName(item.Name),
                            ImageSource =
                                BitmapHelper.CreateBitmapImage(
                                    ThumbService.ValidatePreview(UriHelper.Get(item.Preview))),
                            Views = ThumbService.ValidateViews("0"),
                            Downloads = ThumbService.ValidateDownloads("0")
                        });
                    }
                }

                IsLoading = false;
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }
        }
    }
}