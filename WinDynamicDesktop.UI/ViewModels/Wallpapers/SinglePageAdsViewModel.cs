﻿using System.Windows.Media;
using Prism.Mvvm;

namespace WinDynamicDesktop.UI.ViewModels.Wallpapers
{
    public class SinglePageAdsViewModel : BindableBase
    {
        private string text;
        public string Text { get => text; set => SetProperty(ref text, value); }

        private ImageSource imageSource;
        public ImageSource ImageSource { get => imageSource; set => SetProperty(ref imageSource, value); }

        private string link;
        public string Link { get => link; set => SetProperty(ref link, value); }

        private bool isVisible;
        public bool IsVisible { get => isVisible; set => SetProperty(ref isVisible, value); }
    }
}