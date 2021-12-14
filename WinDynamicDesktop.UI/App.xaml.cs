﻿using Prism.Ioc;
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
            containerRegistry.RegisterForNavigation<Wallpapers, ViewModels.Wallpapers>();
            containerRegistry.RegisterForNavigation<WallpapersNew, ViewModels.WallpapersNew>();
            containerRegistry.RegisterForNavigation<WallpapersPopular, ViewModels.WallpapersPopular>();
            containerRegistry.RegisterForNavigation<WallpapersWait, ViewModels.WallpapersWait>();
            containerRegistry.RegisterForNavigation<SimplePage, ViewModels.SimplePageViewModel>();
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