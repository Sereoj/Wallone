using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class ImagePreviewViewModel : BindableBase, INavigationAware
    {
        private readonly BitmapHelper bitmapHelper;

        private ImageSource backImageSource;
        private ImageSource frontImageSource;
        private bool isEnable;
        private int selectedIndex;
        private SinglePage simplePage;


        private bool startAnimationValue;

        private string text;
        private readonly DispatcherTimer transitionTimer;

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

        public bool StartAnimationValue
        {
            get => startAnimationValue;
            set => SetProperty(ref startAnimationValue, value);
        }

        public ImageSource FrontImageSource
        {
            get => frontImageSource;
            set => SetProperty(ref frontImageSource, value);
        }

        public ImageSource BackImageSource
        {
            get => backImageSource;
            set => SetProperty(ref backImageSource, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                SetProperty(ref selectedIndex, value);
                if (value != -1)
                {
                    FrontImageSource = bitmapHelper[Items[value].Uri];
                    Text = Items[value].Name;
                }
            }
        }

        public ObservableCollection<ThemePreviewItem> Items { get; } = new ObservableCollection<ThemePreviewItem>();

        public DelegateCommand PreviousCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Items.Clear();
            bitmapHelper.Clear();

            simplePage = (SinglePage) navigationContext.Parameters["simplePage"];

            if (ThemePreviewService.IsNotNull(simplePage.images))
            {
                foreach (var item in simplePage.images) SetImageList(item.times, UriHelper.Get(item.location));

                isEnable = true;
                SelectedIndex = Items.IndexOf(Items.FirstOrDefault());
                FrontImageSource = bitmapHelper[Items[SelectedIndex].Uri];
                Text = Items[SelectedIndex].Name;
                transitionTimer.Start();
            }
            else
            {
                FrontImageSource = new BitmapImage(UriHelper.Get("pack://application:,,,/Wallone.Common;component/Images/Placeholder.png"));
                transitionTimer.Stop();
                Text = "Неловкая ситуация =(";
                isEnable = false;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            transitionTimer.Stop();
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

        public void SetImageList(string type, Uri imagePath)
        {
            Items.Add(new ThemePreviewItem(type, imagePath));
        }
    }
}