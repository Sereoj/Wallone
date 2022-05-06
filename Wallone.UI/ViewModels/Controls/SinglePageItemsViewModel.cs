using Prism.Mvvm;
using System.Windows.Media;

namespace Wallone.UI.ViewModels.Controls
{
    public class SinglePageItemsViewModel : BindableBase
    {
        //Аватар
        private ImageSource avatar;

        //Бренд
        private string brand;

        //Дата публикации
        private string date;

        private string description;

        private string downloads;

        private string likes;
        private string name;

        //Имя пользователя
        private string username;

        private string views;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
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

        public string Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public string Brand
        {
            get => brand;
            set => SetProperty(ref brand, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Likes
        {
            get => likes;
            set => SetProperty(ref likes, value);
        }

        public string Views
        {
            get => views;
            set => SetProperty(ref views, value);
        }

        public string Downloads
        {
            get => downloads;
            set => SetProperty(ref downloads, value);
        }
    }
}