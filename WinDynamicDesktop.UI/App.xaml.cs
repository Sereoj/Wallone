using Prism.Ioc;
using Prism.Modularity;
using System.IO;
using System.Windows;
using WinDynamicDesktop.Authorization;
using WinDynamicDesktop.Common;
using WinDynamicDesktop.Core;
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Controls;
using WinDynamicDesktop.UI.Views;

namespace WinDynamicDesktop.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Main>();
            containerRegistry.RegisterForNavigation<Indicator, ViewModels.IndicatorViewModel>();
            containerRegistry.RegisterForNavigation<Uploader, ViewModels.UploaderViewModel>();

            containerRegistry.RegisterForNavigation<Article, ViewModels.ArticleViewModel>();
            containerRegistry.RegisterForNavigation<ArticleMedium, ViewModels.ArticleViewModel>();

            containerRegistry.RegisterForNavigation<NotFound, ViewModels.NotFoundViewModel>();

            containerRegistry.RegisterForNavigation<Wallpapers, ViewModels.WallpapersViewModel>();
            containerRegistry.RegisterForNavigation<WallpapersNew, ViewModels.WallpapersNewViewModel>();
            containerRegistry.RegisterForNavigation<WallpapersPopular, ViewModels.WallpapersPopularViewModel>();
            containerRegistry.RegisterForNavigation<WallpapersWait, ViewModels.WallpapersWaitViewModel>();

            containerRegistry.RegisterForNavigation<InstalledWallpapers, ViewModels.InstalledWallpapersViewModel>();
            containerRegistry.RegisterForNavigation<FavoriteWallpapers, ViewModels.WallpapersFavoriteViewModel>();
            containerRegistry.RegisterForNavigation<LoadWallpapers, ViewModels.WallpapersLoadViewModel>();

            containerRegistry.RegisterForNavigation<Profile, ViewModels.ProfileViewModel>();
            containerRegistry.RegisterForNavigation<Account, ViewModels.AccountViewModel>();
            containerRegistry.RegisterForNavigation<Settings, ViewModels.SettingsViewModel>();

            containerRegistry.RegisterForNavigation<SinglePage, ViewModels.SinglePageViewModel>();
            containerRegistry.RegisterForNavigation<ImagePreview, ViewModels.ImagePreviewViewModel>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {

            var app = new AppSettingsBuilder()
                .Query(new AppPathBulder()
                    .AppLocation(Directory.GetCurrentDirectory())
                    .Build())
                .Query(new ThemePathBuilder()
                    .ExistOrCreateDirectory("themes")
                    .UseForFolders("name")
                    .Build())
                .Query(new SettingsBuilder()
                    .UpdateOrCreateFile("app.settings")
                    .Build()
                );

            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<CommonModule>();
            moduleCatalog.AddModule<AuthorizationModule>();
        }
    }
}
