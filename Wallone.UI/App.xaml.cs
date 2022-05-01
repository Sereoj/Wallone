using System.IO;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Wallone.Authorization;
using Wallone.Common;
using Wallone.Controls;
using Wallone.Core;
using Wallone.Core.Builders;
using Wallone.Core.Controllers;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;
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
using Profile = Wallone.UI.Views.Users.Profile;
using SinglePage = Wallone.UI.Views.Wallpapers.SinglePage;

namespace Wallone.UI
{
    /// <summary>
    ///     Interaction logic for App.xaml
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
            containerRegistry.RegisterForNavigation<Settings, SettingsViewModel>();

            containerRegistry.RegisterForNavigation<SinglePage, SinglePageViewModel>();
            containerRegistry.RegisterForNavigation<ImagePreview, ImagePreviewViewModel>();
            containerRegistry.RegisterForNavigation<InformationArticle, InformationArticleViewModel>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<CommonModule>();
            moduleCatalog.AddModule<ControlsModule>();
            moduleCatalog.AddModule<AuthorizationModule>();

            var app = new AppSettingsBuilder()
                .Query(new AppPathBuilder()
                    .AppLocation(Directory.GetCurrentDirectory())
                    .Build())
                .Query(new SettingsBuilder()
                    .UpdateOrCreateFile("app.settings")
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

            var theme = new ThemeCreatedBuilder()
                .SetName(AppFormat.Format(ThemeService.GetCurrentName()))
                .HasDownloaded();

            var themeExist = theme.Exist();

            if (themeExist)
            {
                var controller = new ThemeController();

            }
        }
    }
}