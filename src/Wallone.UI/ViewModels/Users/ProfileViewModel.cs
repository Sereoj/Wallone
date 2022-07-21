using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using RestSharp;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Interfaces;
using Wallone.Core.Models;
using Wallone.Core.Services;
using Wallone.Core.Services.App;
using Wallone.Core.Services.Pages;
using Wallone.Core.Services.Users;
using Wallone.UI.Services;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.ViewModels.Users
{
    public class ProfileViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public ManagerViewModel ManagerViewModel { get; }

        private string header = "Профиль";

        private string id;

        private bool isContent;

        private bool isInternet;

        private bool isLoading = true;
        private Profile profilePage;

        public ProfileViewModel()
        {
        }

        public ProfileViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            ManagerViewModel = new ManagerViewModel(regionManager);

            ActionCommand = new DelegateCommand(OnAction);
            EditProfileCommand = new DelegateCommand(OnEditProfile);
            SizeChangedCommand = new DelegateCommand<SizeChangedEventArgs>(OnSizeChanged);
        }

        public ProfileViewModelItems ProfileItemsVM { get; set; } = new ProfileViewModelItems();
        public ProfileViewModelActions ProfileActionsVM { get; set; } = new ProfileViewModelActions();

        public ObservableCollection<ArticleViewModel> Posts { get; set; } =
            new ObservableCollection<ArticleViewModel>();

        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                SetProperty(ref isLoading, value);
                IsContent = value == false;
            }
        }

        public bool IsInternet
        {
            get => isInternet;
            set
            {
                SetProperty(ref isInternet, value);
                IsContent = value == false;
            }
        }

        public bool IsContent
        {
            get => isContent;
            set => SetProperty(ref isContent, value);
        }

        private bool isProfile;
        public bool IsProfile
        {
            get => isProfile;
            set => SetProperty(ref isProfile, value);
        }

        public DelegateCommand ActionCommand { get; set; }
        public DelegateCommand EditProfileCommand { get; set; }

        public DelegateCommand<SizeChangedEventArgs> SizeChangedCommand { get; set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            id = (string)navigationContext.Parameters["id"];
            Header = (string)navigationContext.Parameters["header"];
            ProfileItemsVM.Name = (string)navigationContext.Parameters["name"];
            IsProfile = (bool)navigationContext.Parameters["isProfile"];

            var items = new SettingsBuilder(SettingsRepository.Get())
                .ItemBuilder();

            if (IsProfile)
            {

                var userId = UserRepository.Fields.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    ProfileActionsVM.IsEnableEditProfile = true;
                    ProfileActionsVM.IsEnableSub = false;
                    Loaded(userId, true);
                }
                else
                {
                    if (string.IsNullOrEmpty(items.GetToken()) || string.IsNullOrEmpty(items.GetEmail()))
                    {
                        regionManager.RequestNavigate("ContentRegion", "Login");
                    }
                    else
                    {
                        regionManager.RequestNavigate("ContentRegion", "Register");
                    }
                }
            }
            else
            {
                ProfileActionsVM.IsEnableEditProfile = false;
                ProfileActionsVM.IsEnableSub = id != UserRepository.Fields.GetUserId();
                Loaded(id, false);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            GC.Collect(2);
        }

        private void OnSizeChanged(SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                if (e.NewSize.Width > 625)
                {
                    ProfileActionsVM.IsEnableText = true;
                    ProfileActionsVM.IsEnableIcons = false;
                }
                else
                {
                    ProfileActionsVM.IsEnableText = false;
                    ProfileActionsVM.IsEnableIcons = true;
                }
            }
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

        public async void Loaded(string id, bool isMyProfile)
        {
            try
            {
                IsLoading = true;
                var data = await ProfileService.GetPageAsync(id);

                if (!string.IsNullOrEmpty(data))
                {
                    var message = JObject.Parse(data);
                    if (!string.IsNullOrEmpty(message["message"]?.ToString()))
                    {
                        ManagerViewModel.Handler();
                    }
                    else
                    {
                        profilePage = JsonConvert.DeserializeObject<Profile>(data);
                        ProfileService.Load(profilePage);

                        ProfileItemsVM.Name = ProfileService.GetUsername();
                        ProfileItemsVM.Description = ProfileService.GetDescription();

                        ProfileItemsVM.Avatar = UriHelper.Get(ProfileService.GetAvatar());
                        ProfileItemsVM.Cover = UriHelper.Get(ProfileService.GetCover());

                        ProfileItemsVM.Subscribers = ProfileService.GetSubscribers();
                        ProfileItemsVM.Subscriptions = ProfileService.GetSubscriptions();
                        ProfileItemsVM.Likes = ProfileService.GetLikes();
                        ProfileItemsVM.Publish = ProfileService.GetPublish();

                        ProfileActionsVM.IsMyProfile = isMyProfile;

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
                    }
                }

                IsLoading = false;
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }

            GC.Collect(1, GCCollectionMode.Forced);
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
                if (ThumbService.IsNotNull(list))
                {
                    foreach (var item in list)
                    {
                        Posts.Add(new ArticleViewModel(regionManager)
                        {
                            Uuid = item.Uuid,
                            Name = item.Name,
                            ImageSource = new BitmapImage(UriHelper.Get(item.Preview))
                        });
                        await Task.CompletedTask;
                    }

                    ProfileActionsVM.IsPosts = list.Count > 0;
                }
            }
            catch (Exception ex)
            {
                ManagerViewModel.Show(Pages.NotFound, ex.Message);
            }
        }
    }
}