﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private readonly DispatcherTimer transitionTimer;

        private BitmapImage backImageSource;
        private BitmapImage frontImageSource;
        private bool isEnable;
        private int selectedIndex;
        private SinglePage singlePage;


        private bool startAnimationValue;

        private string text;

        public ImagePreviewViewModel()
        {
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

        public BitmapImage FrontImageSource
        {
            get => frontImageSource;
            set
            {
                SetProperty(ref frontImageSource, value);
                RaisePropertyChanged();
            }
        }

        public BitmapImage BackImageSource
        {
            get => backImageSource;
            set
            {
                SetProperty(ref backImageSource, value);
                RaisePropertyChanged();
            }
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
                    FrontImageSource = BitmapHelper.CreateBitmapImage(Items[value].Uri);
                    Text = Items[value].Name.ToString();
                }
            }
        }

        public ObservableCollection<ThemePreviewItem> Items { get; } = new ObservableCollection<ThemePreviewItem>();

        public DelegateCommand PreviousCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            singlePage = (SinglePage) navigationContext.Parameters["singlePage"];

            if (singlePage != null)
            {
                if (ThemePreviewService.IsNotNull(singlePage.images))
                {
                    foreach (var item in singlePage.images) SetImageList(item.times, UriHelper.Get(item.location));

                    isEnable = true;
                    SelectedIndex = Items.IndexOf(Items.FirstOrDefault());
                    FrontImageSource = BitmapHelper.CreateBitmapImage(Items[SelectedIndex].Uri);
                    Text = Items[SelectedIndex].Name.ToString();
                    transitionTimer.Start();
                }
                else
                {
                    FrontImageSource = BitmapHelper.CreateBitmapImage(
                        UriHelper.Get("/Wallone.Common;component/Images/Placeholder.png"));
                    transitionTimer.Stop();
                    Text = "Неловкая ситуация =(";
                    isEnable = false;
                }
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Items.Clear();
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
                    FrontImageSource = BitmapHelper.CreateBitmapImage(Items[SelectedIndex].Uri);
                }
                else
                {
                    SelectedIndex++;
                    FrontImageSource = BitmapHelper.CreateBitmapImage(Items[SelectedIndex].Uri);
                }

                StartAnimationValue = true;
                Text = Items[SelectedIndex].Name.ToString();
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
                    FrontImageSource = BitmapHelper.CreateBitmapImage(Items[SelectedIndex].Uri);
                }
                else
                {
                    SelectedIndex--;
                    FrontImageSource = BitmapHelper.CreateBitmapImage(Items[SelectedIndex].Uri);
                }

                StartAnimationValue = true;
                Text = Items[SelectedIndex].Name.ToString();
            }
        }

        public void SetImageList(Times type, Uri imagePath)
        {
            Items.Add(new ThemePreviewItem(type, imagePath));
        }
    }
}