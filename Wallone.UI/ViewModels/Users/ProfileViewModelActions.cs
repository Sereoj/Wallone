using Prism.Mvvm;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Users
{
    public class ProfileViewModelActions : BindableBase
    {
        private string actionStatus;
        private string actionText;

        private bool isEmptyPosts;

        private bool isEnableEditProfile;

        private bool isEnableFacebook;

        private bool isEnableGithub;

        private bool isEnableIcons;

        private bool isEnableSub;

        private bool isEnableText = true;

        private bool isEnableTwitter;

        private bool isEnableVK;

        private bool isMyProfile;

        private bool isPosts;


        private string titlePosts = "Опубликованные посты";

        public string ActionText
        {
            get => actionText;
            set => SetProperty(ref actionText, value);
        }

        public string ActionStatus
        {
            get => actionStatus;
            set
            {
                SetProperty(ref actionStatus, value);
                ActionText = value != "true" ? "Отписка" : "Подписка";
            }
        }

        public bool IsEnableSub
        {
            get => isEnableSub;
            set => SetProperty(ref isEnableSub, value);
        }

        public bool IsEnableEditProfile
        {
            get => isEnableEditProfile;
            set => SetProperty(ref isEnableEditProfile, value);
        }

        public bool IsEnableFacebook
        {
            get => isEnableFacebook;
            set => SetProperty(ref isEnableFacebook, value);
        }

        public bool IsEnableGithub
        {
            get => isEnableGithub;
            set => SetProperty(ref isEnableGithub, value);
        }

        public bool IsEnableTwitter
        {
            get => isEnableTwitter;
            set => SetProperty(ref isEnableTwitter, value);
        }

        public bool IsEnableVK
        {
            get => isEnableVK;
            set => SetProperty(ref isEnableVK, value);
        }

        public string TitlePosts
        {
            get => titlePosts;
            set => SetProperty(ref titlePosts, value);
        }

        public bool IsPosts
        {
            get => isPosts;
            set
            {
                SetProperty(ref isPosts, value);
                IsEmptyPosts = value == false;
                TitlePosts = value ? "Опубликованные посты" : "Постов не существует";
            }
        }

        public bool IsEmptyPosts
        {
            get => isEmptyPosts;
            set => SetProperty(ref isEmptyPosts, value);
        }

        public bool IsMyProfile
        {
            get => isMyProfile;
            set
            {
                SetProperty(ref isMyProfile, value);
                if (value)
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

        public bool IsEnableIcons
        {
            get => isEnableIcons;
            set => SetProperty(ref isEnableIcons, value);
        }

        public bool IsEnableText
        {
            get => isEnableText;
            set => SetProperty(ref isEnableText, value);
        }
    }
}