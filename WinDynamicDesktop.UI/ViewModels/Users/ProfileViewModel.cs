using ModernWpf.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Authorization.Services;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ProfileViewModel : BindableBase, INavigationAware
    {

        private string id;
        private Profile profilePage;
        private static BitmapHelper bitmapHelper = new BitmapHelper();
        private readonly IRegionManager regionManager;

        public ObservableCollection<ArticleViewModel> Posts { get; set; } = new ObservableCollection<ArticleViewModel>();
        private string header = "Профиль";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }
        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private ImageSource cover;
        public ImageSource Cover { get => cover; set => SetProperty(ref cover, value); }

        private ImageSource avatar;
        public ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }

        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private string friends;
        public string Friends { get => friends; set => SetProperty(ref friends, value); }

        private string likes;
        public string Likes { get => likes; set => SetProperty(ref likes, value); }

        private string publish;
        public string Publish { get => publish; set => SetProperty(ref publish, value); }

        private FontIcon actionText;
        public FontIcon ActionText { get => actionText; set => SetProperty(ref actionText, value); }

        private string actionStatus;
        public string ActionStatus { get => actionStatus; set => SetProperty(ref actionStatus, value); }

        public DelegateCommand ActionCommand { get; set; }
        public ProfileViewModel()
        {

        }
        public ProfileViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            ActionCommand = new DelegateCommand(OnAction,CanAction);

        }

        private bool CanAction()
        {
            //return true;
            return ActionStatus != null;
        }

        private async void OnAction()
        {
            FontIcon icon;
            switch (ActionStatus)
            {
                case "true":
                    icon = FontIconService.SetIcon("ultimate", "\uED5E");
                    icon.FontSize = 16;
                    ActionText = icon;
                    await ProfileService.SetAppendFriendAsync();
                    ActionStatus = "false";
                    break;
                case "false":
                    icon = FontIconService.SetIcon("ultimate", "\uED5D");
                    icon.FontSize = 16;
                    ActionText = icon;
                    await ProfileService.SetRemoveFriendAsync();
                    ActionStatus = "true";
                    break;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["id"];
            Name = (string)navigationContext.Parameters["name"];

            if(id != null)
            {
                Loaded(id);
            }
            else
            {
                Loaded(UserService.GetId());
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new System.NotImplementedException();
        }

        public async void Loaded(string id)
        {
            try
            {
                var data = await ProfileService.GetPageAsync(id);

                if (!string.IsNullOrEmpty(data))
                {
                    //var jArray = JArray.Parse(data);
                    profilePage = JsonConvert.DeserializeObject<Profile>(data);
                    ProfileService.Load(profilePage);

                    Name = ProfileService.GetUsername();
                    Description = ProfileService.GetDescription();

                    Avatar = ProfileService.GetAvatar() == null ? null : (ImageSource)bitmapHelper[UriHelper.Get(ProfileService.GetAvatar())];
                    Cover = ProfileService.GetCover() == null
                        ? (ImageSource)App.Current.Resources["Placeholder1280"]
                        : bitmapHelper[UriHelper.Get(ProfileService.GetCover())];

                    Friends = ProfileService.GetFriends();
                    Likes = ProfileService.GetLikes();
                    Publish = ProfileService.GetPublish();


                    posts(ProfileService.GetPosts());
                    bitmapHelper.Clear();
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }

        private async void posts(List<Thumb> list)
        {
            Posts.Clear();
            try
            {

                if (ThumbService.CheckItems(list))
                {
                    foreach (var item in list)
                    {
                        Posts.Add(new ArticleViewModel(regionManager)
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                        });
                        await Task.CompletedTask;
                    }
                }
            }
            catch (Exception ex)
            {
                var param = new NavigationParameters
                {
                    { "Text", ex.Message }
                };

                regionManager.RequestNavigate("PageRegion", "NotFound", param);
            }
        }
    }
}
