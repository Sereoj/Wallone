using Prism.Mvvm;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels.Users
{
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

        private bool isEnableFacebook = false;
        public bool IsEnableFacebook
        {
            get { return isEnableFacebook; }
            set { SetProperty(ref isEnableFacebook, value); }
        }

        private bool isEnableGithub = false;
        public bool IsEnableGithub
        {
            get { return isEnableGithub; }
            set { SetProperty(ref isEnableGithub, value); }
        }

        private bool isEnableTwitter = false;
        public bool IsEnableTwitter
        {
            get { return isEnableTwitter; }
            set { SetProperty(ref isEnableTwitter, value); }
        }

        private bool isEnableVK = false;
        public bool IsEnableVK
        {
            get { return isEnableVK; }
            set { SetProperty(ref isEnableVK, value); }
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
                IsEmptyPosts = value == false;
                TitlePosts = value == true ? "Опубликованные посты" : "Постов не существует";
            }
        }

        private bool isEmptyPosts;
        public bool IsEmptyPosts
        {
            get { return isEmptyPosts; }
            set => SetProperty(ref isEmptyPosts, value);
        }

        private bool isMyProfile;
        public bool IsMyProfile
        {
            get { return isMyProfile; }
            set
            {
                SetProperty(ref isMyProfile, value);
                if (value == true)
                {
                    IsEnableFacebook = false;
                    IsEnableTwitter = false;
                    IsEnableGithub = false;
                    IsEnableVK = false;
                }
                else
                {
                    IsEnableFacebook = ProfileService.GetFacebook() != null;
                    IsEnableTwitter = ProfileService.GetTwitter() != null;
                    IsEnableGithub = ProfileService.GetGithub() != null;
                    IsEnableVK = ProfileService.GetVK() != null;
                }
            }
        }

        private bool isEnableIcons;
        public bool IsEnableIcons
        {
            get => isEnableIcons;
            set
            {
                SetProperty(ref isEnableIcons, value);
            }
        }

        private bool isEnableText = true;
        public bool IsEnableText
        {
            get => isEnableText;
            set
            {
                SetProperty(ref isEnableText, value);
            }
        }

        public ProfileViewModelActions()
        {

        }
    }
}