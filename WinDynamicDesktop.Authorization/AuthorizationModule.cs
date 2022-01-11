using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using WinDynamicDesktop.Authorization.Controls;
using WinDynamicDesktop.Authorization.Views;

namespace WinDynamicDesktop.Authorization
{
    public class AuthorizationModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public AuthorizationModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Load, ViewModels.LoadViewModel>();
            containerRegistry.RegisterForNavigation<MessageControl, ViewModels.MessageViewModel>();
            containerRegistry.RegisterForNavigation<Register, ViewModels.RegisterViewModel>();
            containerRegistry.RegisterForNavigation<Login, ViewModels.LoginViewModel>();
            containerRegistry.RegisterForNavigation<Confirm, ViewModels.ConfirmViewModel>();
            containerRegistry.RegisterForNavigation<Photo, ViewModels.PhotoViewModel>();
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate("ContentRegion", "Load");
        }
    }
}
