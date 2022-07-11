using System.Windows;
using System.Windows.Navigation;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Wallone.Authorization;
using Wallone.Common;
using Wallone.Controls;
using Wallone.Core;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Loggers;
using Wallone.UI.Controls;
using Wallone.UI.ViewModels;
using Wallone.UI.ViewModels.Controls;
using Wallone.UI.ViewModels.Exceptions;
using Wallone.UI.ViewModels.Users;
using Wallone.UI.ViewModels.Wallpapers;
using Wallone.UI.Views;
using Wallone.UI.Views.Exceptions;
using Wallone.UI.Views.Users;
using Wallone.UI.Views.Wallpapers;

namespace Wallone.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            var container = Container.Resolve<MainWindow>();
            var appContext = new AppContext(container);
            return container;

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Main>();
            containerRegistry.RegisterForNavigation<Indicator, IndicatorViewModel>();
            containerRegistry.RegisterForNavigation<Uploader, UploaderViewModel>();

            containerRegistry.RegisterForNavigation<Article, ArticleViewModel>();
            containerRegistry.RegisterForNavigation<ArticleMedium, ArticleViewModel>();
            containerRegistry.RegisterForNavigation<TabSunTimes, TabSunTimesViewModel>();
            containerRegistry.RegisterForNavigation<TabInformation, TabInformationViewModel>();

            containerRegistry.RegisterForNavigation<NotFound, NotFoundViewModel>();

            containerRegistry.RegisterForNavigation<Wallpapers, WallpapersViewModel>();
            containerRegistry.RegisterForNavigation<DownloadsPage, DownloadsPageViewModel>();

            containerRegistry.RegisterForNavigation<Profile, ProfileViewModel>();
            containerRegistry.RegisterForNavigation<Account, AccountViewModel>();
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();

            containerRegistry.RegisterForNavigation<SinglePage, SinglePageViewModel>();
            containerRegistry.RegisterForNavigation<ImagePreview, ImagePreviewViewModel>();
            containerRegistry.RegisterForNavigation<InformationArticle, InformationArticleViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CommonModule>();
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<ControlsModule>();
            moduleCatalog.AddModule<AuthorizationModule>();
        }

        protected override void Initialize()
        {
            Init();
            base.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private static void Init()
        {

            var platformer = Platformer.GetHelper();

            var app = new AppSettingsBuilder()
                .Query(new AppLoggerBuilder()
                    .SetFileName(LoggerService.DefaultFileName())
                    .Activate()
                    .NewLine()
                    .StartLine()
                )
                .Query(new AppPathBuilder()
                    .AppLocation(platformer.GetCurrentFolder())
                    .ApplicationPath(AppSettingsRepository.AppSettingsService.AppPath())
                    .Build())
                .Query(new SettingsBuilder(AppSettingsRepository.GetSettings())
                    .UpdateOrCreateFile("app.settings")
                    .SetConfigName("theme.json")
                    .Build())
                .Query(new ThemePathBuilder()
                    .ExistOrCreateDirectory("themes")
                    .UseForFolders("name")
                    .Build())
                .Query(new HostBuilder()
                    .SetHost()
                    .SetPrefix()
                    .Validate()
                    .Build()
                );
        }
    }
}