using Prism.Ioc;
using Prism.Modularity;
using System.IO;
using System.Windows;
using WinDynamicDesktop.Authorization;
using WinDynamicDesktop.Common;
using WinDynamicDesktop.Controls;
using WinDynamicDesktop.Core;
using WinDynamicDesktop.Core.Builders;
using WinDynamicDesktop.UI.Controls;
using WinDynamicDesktop.UI.ViewModels.Controls;
using WinDynamicDesktop.UI.ViewModels.Exceptions;
using WinDynamicDesktop.UI.ViewModels.Users;
using WinDynamicDesktop.UI.ViewModels.Wallpapers;
using WinDynamicDesktop.UI.Views;
using WinDynamicDesktop.UI.Views.Exceptions;
using WinDynamicDesktop.UI.Views.Users;
using WinDynamicDesktop.UI.Views.Wallpapers;

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
            containerRegistry.RegisterForNavigation<Indicator, IndicatorViewModel>();
            containerRegistry.RegisterForNavigation<Uploader, UploaderViewModel>();

            containerRegistry.RegisterForNavigation<Article, ArticleViewModel>();
            containerRegistry.RegisterForNavigation<ArticleMedium, ArticleViewModel>();

            containerRegistry.RegisterForNavigation<NotFound, NotFoundViewModel>();

            containerRegistry.RegisterForNavigation<Wallpapers, WallpapersViewModel>();

            containerRegistry.RegisterForNavigation<Profile, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<Account, AccountViewModel>();
            containerRegistry.RegisterForNavigation<Settings, ViewModels.SettingsViewModel>();

            containerRegistry.RegisterForNavigation<SinglePage, SinglePageViewModel>();
            containerRegistry.RegisterForNavigation<ImagePreview, ImagePreviewViewModel>();
            containerRegistry.RegisterForNavigation<InformationArticle, InformationArticleViewModel>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {

            new AppSettingsBuilder()
                .Query(new AppPathBulder()
                    .AppLocation(Directory.GetCurrentDirectory())
                    .Build())
                .Query(new ThemePathBuilder()
                    .ExistOrCreateDirectory("themes")
                    .UseForFolders("name")
                    .Build())
                .Query(new SettingsBuilder()
                    .UpdateOrCreateFile("app.settings")
                    .Build())
                .Query(new HostBuilder()
                    .SetHost()
                    .SetPrefix()
                    .Validate()
                    .Build()
                );

            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<CommonModule>();
            moduleCatalog.AddModule<ControlsModule>();
            moduleCatalog.AddModule<AuthorizationModule>();
        }
    }
}
