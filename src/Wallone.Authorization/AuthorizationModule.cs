using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Wallone.Authorization.ViewModels;
using Wallone.Authorization.Views;

namespace Wallone.Authorization
{
    public class AuthorizationModule : IModule
    {
        private readonly IRegionManager regionManager;

        public AuthorizationModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Register, RegisterViewModel>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RequestNavigate("ContentRegion", "Main");
        }
    }
}