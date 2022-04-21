using System.Windows.Media;
using Prism.Mvvm;

namespace Wallone.UI.ViewModels.Users
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
}