using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class ImagePreviewViewModel : BindableBase, INavigationAware
    {
        private SimplePage simplePage;
        private DispatcherTimer transitionTimer;
        private readonly BitmapHelper bitmapHelper;
        private bool isEnable;

        private ImageSource imageSource;
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
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
            bitmapHelper = new BitmapHelper();

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
            if (isEnable)
            {
                if (SelectedIndex + 1 >= Items.Count)
                {
                    SelectedIndex = Items.IndexOf(Items.First());
                    ImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                else
                {
                    SelectedIndex++;
                    ImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                Text = Items[SelectedIndex].Name;
            }
        }

        private void onPrevious()
        {
            if (isEnable)
            {
                if (SelectedIndex - 1 == -1)
                {
                    SelectedIndex = Items.IndexOf(Items.Last());
                    ImageSource = ImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                else
                {
                    SelectedIndex--;
                    ImageSource = ImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                Text = Items[SelectedIndex].Name;
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Items.Clear();
            bitmapHelper.Clear();

            simplePage = (SimplePage)navigationContext.Parameters["simplePage"];

            if (ThemePreviewService.CheckItems(simplePage.images))
            {
                foreach (var item in simplePage.images)
                {
                    SetImageList(item.type, UriHelper.Get(item.location));
                }

                isEnable = true;
                SelectedIndex = Items.IndexOf(Items.FirstOrDefault());
                ImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                Text = Items[SelectedIndex].Name;
                transitionTimer.Start();
            }
            else
            {
                Text = "Произошла ошибка";
                isEnable = false;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        public void SetImageList(string type, Uri imagePath)
        {
            Items.Add(new ThemePreviewItem(type, imagePath));
        }
    }
}
