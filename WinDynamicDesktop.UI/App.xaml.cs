using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using WinDynamicDesktop.Authorization;
using WinDynamicDesktop.Common;
using WinDynamicDesktop.Core;
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

            containerRegistry.RegisterForNavigation<NotFound, ViewModels.NotFoundViewModel>();

            containerRegistry.RegisterForNavigation<Wallpapers, ViewModels.WallpapersViewModel>();
            containerRegistry.RegisterForNavigation<WallpapersNew, ViewModels.WallpapersNewViewModel>();
            containerRegistry.RegisterForNavigation<WallpapersPopular, ViewModels.WallpapersPopularViewModel>();
            containerRegistry.RegisterForNavigation<WallpapersWait, ViewModels.WallpapersWaitViewModel>();

            containerRegistry.RegisterForNavigation<SinglePage, ViewModels.SinglePageViewModel>();
            containerRegistry.RegisterForNavigation<ImagePreview, ViewModels.ImagePreviewViewModel>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<CommonModule>();
            moduleCatalog.AddModule<AuthorizationModule>();
        }
    }
}
