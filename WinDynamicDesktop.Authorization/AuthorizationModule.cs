using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
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
            containerRegistry.RegisterForNavigation<Register>();
            containerRegistry.RegisterForNavigation<Login>();
            containerRegistry.RegisterForNavigation<Confirm>();
            containerRegistry.RegisterForNavigation<Photo>();
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate("ContentRegion", "Register");
        }
    }
}
