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
        private SinglePage simplePage;
        private DispatcherTimer transitionTimer;
        private readonly BitmapHelper bitmapHelper;
        private bool isEnable;


        private bool startAnimationValue;
        public bool StartAnimationValue
        {
            get { return startAnimationValue; }
            set { SetProperty(ref startAnimationValue, value); }
        }
        private ImageSource frontImageSource;
        public ImageSource FrontImageSource
        {
            get { return frontImageSource; }
            set { SetProperty(ref frontImageSource, value); }
        }

        private ImageSource backImageSource;
        public ImageSource BackImageSource
        {
            get { return backImageSource; }
            set { SetProperty(ref backImageSource, value); }
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
            set
            { 
                SetProperty(ref selectedIndex, value);
                if(value !=  -1)
                {
                    FrontImageSource = bitmapHelper[Items[value].Uri];
                    Text = Items[value].Name;
                }
            }
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
            transitionTimer.Tick += (s, e) =>
            {
                StartAnimationValue = false;
                onNext();
                StartAnimationValue = true;
            };

        }

        private void onNext()
        {
            if (isEnable)
            {
                StartAnimationValue = false;
                BackImageSource = FrontImageSource;

                if (SelectedIndex + 1 >= Items.Count)
                {
                    SelectedIndex = Items.IndexOf(Items.First());
                    FrontImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                else
                {
                    SelectedIndex++;
                    FrontImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }

                StartAnimationValue = true;
                Text = Items[SelectedIndex].Name;
            }
        }

        private void onPrevious()
        {
            if (isEnable)
            {
                StartAnimationValue = false;
                BackImageSource = FrontImageSource;

                if (SelectedIndex - 1 == -1)
                {
                    SelectedIndex = Items.IndexOf(Items.Last());
                    FrontImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                else
                {
                    SelectedIndex--;
                    FrontImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                }
                StartAnimationValue = true;
                Text = Items[SelectedIndex].Name;
            }
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Items.Clear();
            bitmapHelper.Clear();

            simplePage = (SinglePage)navigationContext.Parameters["simplePage"];

            if (ThemePreviewService.CheckItems(simplePage.images))
            {
                foreach (var item in simplePage.images)
                {
                    SetImageList(item.times, UriHelper.Get(item.location));
                }

                isEnable = true;
                SelectedIndex = Items.IndexOf(Items.FirstOrDefault());
                FrontImageSource = bitmapHelper[Items[SelectedIndex].Uri];
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
