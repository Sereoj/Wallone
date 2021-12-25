using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ImagePreviewViewModel : BindableBase, INavigationAware
    {
        private SimplePage simplePage;
        private DispatcherTimer transitionTimer;

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            { 
                SetProperty(ref imageSource, value);
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }
        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { SetProperty(ref selectedIndex, value); }
        }

        public ObservableCollection<ThemePreviewItem> Items { get; } = new ObservableCollection<ThemePreviewItem>();

        public DelegateCommand PreviousCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }
        public ImagePreviewViewModel()
        {
            PreviousCommand = new DelegateCommand(onPrevious);
            NextCommand = new DelegateCommand(onNext);

            transitionTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            transitionTimer.Tick += (s, e) => onNext();
        }
        private void onNext()
        {
            if (SelectedIndex + 1 >= Items.Count)
            {
                SelectedIndex = Items.IndexOf(Items.First());
                ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            }
            else
            {
                SelectedIndex++;
                ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            }
            Text = Items[SelectedIndex].Name;
        }

        private void onPrevious()
        {
            if(SelectedIndex - 1 == -1)
            {
                SelectedIndex = Items.IndexOf(Items.Last());
                ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            }
            else
            {
                SelectedIndex--;
                ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            }
            Text = Items[SelectedIndex].Name;
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Items.Clear();

            simplePage = (SimplePage)navigationContext.Parameters["simplePage"];

            SetImageList("Рассвет", simplePage.images.sunrise);
            SetImageList("День", simplePage.images.day);
            SetImageList("Закат", simplePage.images.sunset);
            SetImageList("Ночь", simplePage.images.night);

            SelectedIndex = Items.IndexOf(Items.First());
            ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            Text = Items[SelectedIndex].Name;
            transitionTimer.Start();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public void SetImageList(string type, string[] imageList)
        {
            for (int i = 0; i < imageList.Length; i++)
            {
                if (imageList[i] != null)
                {
                    Items.Add(new ThemePreviewItem(type, new Uri(imageList[i])));
                }
            }
        }
    }
}
