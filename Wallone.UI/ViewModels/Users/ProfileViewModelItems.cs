using System.Windows.Media;
using Prism.Mvvm;

namespace Wallone.UI.ViewModels.Users
{
    public class ProfileViewModelItems : BindableBase
    {
        private ImageSource avatar;

        private ImageSource cover;

        private string description;
        private string likes;
        private string name;

        private string publish;

        private string subscribers;
        private string subscriptions;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public ImageSource Cover
        {
            get => cover;
            set
            {
                SetProperty(ref cover, value);
                RaisePropertyChanged();
            }
        }

        public ImageSource Avatar
        {
            get => avatar;
            set
            {
                SetProperty(ref avatar, value);
                RaisePropertyChanged();
            }
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Subscribers
        {
            get => subscribers;
            set => SetProperty(ref subscribers, value);
        }

        public string Subscriptions
        {
            get => subscriptions;
            set => SetProperty(ref subscriptions, value);
        }

        public string Likes
        {
            get => likes;
            set => SetProperty(ref likes, value);
        }

        public string Publish
        {
            get => publish;
            set => SetProperty(ref publish, value);
        }
    }
}