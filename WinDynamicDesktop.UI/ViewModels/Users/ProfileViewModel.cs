using ModernWpf.Controls;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class ProfileViewModelItems : BindableBase
    {
        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private ImageSource cover;
        public ImageSource Cover { get => cover; set => SetProperty(ref cover, value); }

        private ImageSource avatar;
        public ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }

        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private string subscribers;
        public string Subscribers { get => subscribers; set => SetProperty(ref subscribers, value); }
        private string subscriptions;
        public string Subscriptions { get => subscriptions; set => SetProperty(ref subscriptions, value); }
        private string likes;
        public string Likes { get => likes; set => SetProperty(ref likes, value); }

        private string publish;
        public string Publish { get => publish; set => SetProperty(ref publish, value); }

        public ProfileViewModelItems()
        {

        }
    }

    public class ProfileViewModelActions : BindableBase
    {
        private string actionText;
        public string ActionText { get => actionText; set => SetProperty(ref actionText, value); }
        private string actionStatus;
        public string ActionStatus
        {
            get => actionStatus;
            set
            {
                SetProperty(ref actionStatus, value);
                ActionText = value != "true" ? "Отписка" : "Подписка";
            }
        }

        private bool isEnableSub;
        public bool IsEnableSub
        {
            get { return isEnableSub; }
            set { SetProperty(ref isEnableSub, value); }
        }

        private bool isEnableEditProfile;
        public bool IsEnableEditProfile
        {
            get { return isEnableEditProfile; }
            set { SetProperty(ref isEnableEditProfile, value); }
        }

        private bool isEnableFacebook;
        public bool IsEnableFacebook
        {
            get { return isEnableFacebook; }
            set { SetProperty(ref isEnableFacebook, value); }
        }

        private bool isEnableGithub;
        public bool IsEnableGithub
        {
            get { return isEnableGithub; }
            set { SetProperty(ref isEnableGithub, value); }
        }

        private bool isEnableTwitter;
        public bool IsEnableTwitter
        {
            get { return isEnableTwitter; }
            set { SetProperty(ref isEnableTwitter, value); }
        }

        private bool isEnableVK;
        public bool IsEnableVK
        {
            get { return isEnableVK; }
            set { SetProperty(ref isEnableVK, value); }
        }


        public ProfileViewModelActions()
        {

        }
    }
    public class ProfileViewModel : BindableBase, INavigationAware
    {

        private string id;
        private Profile profilePage;
        private static readonly BitmapHelper bitmapHelper = new BitmapHelper();
        private readonly IRegionManager regionManager;

        public ProfileViewModelItems ProfileItemsVM { get; set; } = new ProfileViewModelItems();
        public ProfileViewModelActions ProfileActionsVM { get; set; } = new ProfileViewModelActions();
        public ObservableCollection<ArticleViewModel> Posts { get; set; } = new ObservableCollection<ArticleViewModel>();
        private string header = "Профиль";
        public string Header
        {
            get { return header; }
            set { SetProperty(ref header, value); }
        }

        private string titlePosts = "Опубликованные посты";
        public string TitlePosts
        {
            get { return titlePosts; }
            set { SetProperty(ref titlePosts, value); }
        }

        private bool isPosts;
        public bool IsPosts
        {
            get { return isPosts; }
            set
            { 
                SetProperty(ref isPosts, value); 
                TitlePosts = value == true ? "Опубликованные посты" : "Постов не существует";
            }
        }

        public DelegateCommand ActionCommand { get; set; }
        public DelegateCommand EditProfileCommand { get; set; }
        public ProfileViewModel()
        {

        }
        public ProfileViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            ActionCommand = new DelegateCommand(OnAction);
            EditProfileCommand = new DelegateCommand(OnEditProfile);

        }

        private void OnEditProfile()
        {
            regionManager.RequestNavigate("PageRegion", "Account");
        }

        private async void OnAction()
        {
            Profile data;
            switch (ProfileActionsVM.ActionStatus)
            {
                case "true":
                    data = await ProfileService.SetAppendFriendAsync();
                    update(data);
                    ProfileActionsVM.ActionStatus = "false";
                    break;
                case "false":
                    data = await ProfileService.SetRemoveFriendAsync();
                    update(data);
                    ProfileActionsVM.ActionStatus = "true";
                    break;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["id"];
            ProfileItemsVM.Name = (string)navigationContext.Parameters["name"];

            if(id != null)
            {
                ProfileActionsVM.IsEnableEditProfile = false;
                ProfileActionsVM.IsEnableSub = id != UserService.GetId();
                Loaded(id);
            }
            else
            {
                ProfileActionsVM.IsEnableEditProfile = true;
                ProfileActionsVM.IsEnableSub = false;
                ProfileItemsVM.Name = UserService.GetUsername();
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

                    ProfileItemsVM.Name = ProfileService.GetUsername();
                    ProfileItemsVM.Description = ProfileService.GetDescription();

                    ProfileItemsVM.Avatar = ProfileService.GetAvatar() == null ? null : (ImageSource)bitmapHelper[UriHelper.Get(ProfileService.GetAvatar())];
                    ProfileItemsVM.Cover = ProfileService.GetCover() == null
                        ? (ImageSource)App.Current.Resources["Placeholder1280"]
                        : bitmapHelper[UriHelper.Get(ProfileService.GetCover())];

                    ProfileItemsVM.Subscribers = ProfileService.GetSubscribers();
                    ProfileItemsVM.Subscriptions = ProfileService.GetSubscriptions();
                    ProfileItemsVM.Likes = ProfileService.GetLikes();
                    ProfileItemsVM.Publish = ProfileService.GetPublish();


                    ProfileActionsVM.IsEnableFacebook = ProfileService.GetFacebook() != null;
                    ProfileActionsVM.IsEnableTwitter = ProfileService.GetTwitter() != null;
                    ProfileActionsVM.IsEnableGithub = ProfileService.GetGithub() != null;
                    ProfileActionsVM.IsEnableVK = ProfileService.GetVK() != null;

                    switch (ProfileService.GetSubscriber())
                    {
                        case "true":
                            ProfileActionsVM.ActionStatus = "false";
                            break;
                        case "false":
                            ProfileActionsVM.ActionStatus = "true";
                            break;
                    }

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

        private void update(Profile data)
        {
            ProfileItemsVM.Subscribers = data?.subscribers_count;
            ProfileItemsVM.Subscriptions = data?.subscriptions_count;
            ProfileItemsVM.Likes = data?.users_like_count;
            ProfileItemsVM.Publish = data?.posts_count;
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
                    IsPosts = list.Count > 0;
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
