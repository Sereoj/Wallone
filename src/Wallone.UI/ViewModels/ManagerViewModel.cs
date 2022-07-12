using System;
using ModernWpf.Controls;
using Prism.Regions;
using Wallone.Core.Interfaces;
using Wallone.Core.Services;
using Wallone.Core.Services.Users;

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
            }
        }

        public void Open(NavigationViewItemInvokedEventArgs e)
        {
            var text = e.InvokedItemContainer.Content;

            var param = new NavigationParameters
            {
                {"Root", e.InvokedItemContainer.Tag.ToString()},
                {"Page", e.InvokedItemContainer.Name},
                {"ID", e.InvokedItemContainer.Uid},
                {"Text", text}
            };

            switch (e.InvokedItemContainer.Tag.ToString())
            {
                case "Downloads":
                    regionManager.RequestNavigate("PageRegion", "DownloadsPage", param);
                    break;
                case "Profile":
                    var paramProfile = new NavigationParameters
                    {
                        {"id", UserRepository.Fields.GetUserId()},
                        {"header", "Профиль"},
                        {"name", UserRepository.Fields.GetUsername()},
                        {"isProfile", UserRepository.UserService.IsUser(UserRepository.Fields.GetUserId())}
                    };
                    regionManager.RequestNavigate("PageRegion", "Profile", paramProfile);
                    break;
                case "Account":
                    regionManager.RequestNavigate("PageRegion", "Account", param);
                    break;
                default:
                    regionManager.RequestNavigate("PageRegion", e.IsSettingsInvoked ? "Settings" : "Wallpapers", param);
                    break;
            }
        }
    }
}