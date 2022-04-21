using System.Windows.Media;
using Prism.Mvvm;

namespace Wallone.UI.ViewModels.Controls
{
    public class SinglePageItemsViewModel : BindableBase
    {
        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        //Имя пользователя
        private string username;
        public string Username { get => username; set => SetProperty(ref username, value); }

        //Аватар
        private ImageSource avatar;
        public ImageSource Avatar { get => avatar; set => SetProperty(ref avatar, value); }

        //Дата публикации
        private string date;
        public string Date { get => date; set => SetProperty(ref date, value); }

        //Бренд
        private string brand;
        public string Brand { get => brand; set => SetProperty(ref brand, value); }

        private string description;
        public string Description { get => description; set => SetProperty(ref description, value); }

        private string likes;
        public string Likes { get => likes; set => SetProperty(ref likes, value); }

        private string views;
        public string Views { get => views; set => SetProperty(ref views, value); }

        private string downloads;
        public string Downloads { get => downloads; set => SetProperty(ref downloads, value); }
    }
}