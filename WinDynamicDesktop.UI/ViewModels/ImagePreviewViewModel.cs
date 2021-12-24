using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WinDynamicDesktop.Core.Models;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ImagePreviewViewModel : BindableBase
    {
        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        private int selectedIndex;
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
        }
        private void onNext()
        {
            if (SelectedIndex + 1 >= Items.Count)
            {
                SelectedIndex = Items.IndexOf(Items.First());
                ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            }else
            {
                SelectedIndex++;
                ImageSource = new BitmapImage(Items[SelectedIndex].Uri);
            }
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
        }
    }
}
