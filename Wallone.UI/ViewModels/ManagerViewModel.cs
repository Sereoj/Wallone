using System;
using Prism.Regions;
using Wallone.Core.Interfaces;

namespace Wallone.UI.ViewModels
{
    public class ManagerViewModel
    {
        private readonly IRegionManager regionManager;
        
        public ManagerViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Show(Pages pages, string exMessage)
        {
            switch (pages)
            {
                case Pages.Main:
                    break;
                case Pages.Single:
                    break;
                case Pages.Profile:
                    break;
                case Pages.ProfileEdit:
                    break;
                case Pages.NotFound:
                    regionManager.RequestNavigate("PageRegion", "NotFound", new NavigationParameters { { "Text", exMessage } });
                    break;
                default:
                    break;
            }
        }
    }
}