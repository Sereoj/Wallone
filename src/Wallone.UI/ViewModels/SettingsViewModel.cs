﻿using System;
using System.Diagnostics;
using Prism.Mvvm;
using Prism.Regions;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Models.Settings;
using Wallone.Core.Services;

namespace Wallone.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private bool isAutorun;
        private bool isGeolocation;

        private string name = "Настройки";

        private SettingsItemBuilder settings
            ;

        public SettingsViewModel()
        {
        }

        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            settings = new SettingsBuilder(SettingsService.Get())
                .ItemBuilder();

            IsAutorun = settings.GetAutorun() && Platformer.GetHelper().CheckAutorun();
            IsGeolocation = settings.GetGeolocation();
            IsAutoSetImage = settings.GetAutoSetImage();
            IsSetModel = settings.GetModelWindow();
            IsAnimation = settings.GetAnimation();
            IsCustomResolution = settings.GetUseCustomResolution();

            ThemeIndexSelected = settings.GetWindowTheme() switch
            {
                ModernWpf.ElementTheme.Default => 0,
                ModernWpf.ElementTheme.Light => 1,
                ModernWpf.ElementTheme.Dark => 2,
                _ => ThemeIndexSelected = 0
            };

            GeolocationIndexSelected = settings.GetGeolocationMode() switch
            {
                Geolocation.Custom => 0,
                Geolocation.Auto => 1,
                Geolocation.Windows => 2,
                _ => GeolocationIndexSelected = 1
            };

            ResolutionModeSelected = settings.GetResolutionMode() switch
            {
                ResolutionMode.Custom => 0,
                ResolutionMode.Template => 1,
                ResolutionMode.Auto => 2,
                _ => ResolutionModeSelected = 2
            };
            ResolutionTemplateSelected = settings.GetResolutionTemplate();

            Latitude = settings.GetLatitude();
            Longitude = settings.GetLongitude();

            ImgResolutionWidth = settings.GetResolutionWidth();
            ImgResolutionHeight = settings.GetResolutionHeight();

            settings.Build();


            SettingsService.Get().General.PropertyChanged += SettingsChanged;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool IsAutorun
        {
            get => isAutorun;
            set
            {
                Platformer
                    .GetHelper()
                    .SwitcherAutorun(AppSettingsService.GetApplicationPath(), value);
                SetProperty(ref isAutorun, value);
            }
        }

        public bool IsGeolocation
        {
            get => isGeolocation;
            set
            {
                settings
                    .SetGeolocation(value)
                    .Build();
                IsEnableListLocation = value;
                IsEnableCustomLocation = value;
                SetProperty(ref isGeolocation, value);
            }
        }


        private bool isAutoSetImage;
        public bool IsAutoSetImage
        {
            get { return isAutoSetImage; }
            set
            {
                settings
                    .SetAutoSetImage(value)
                    .Build();
                SetProperty(ref isAutoSetImage, value);
            }
        }

        private int themeIndexSelected;
        public int ThemeIndexSelected
        {
            get { return themeIndexSelected; }
            set
            {
                settings
                    .SetWindowTheme((ModernWpf.ElementTheme)value)
                    .Build();
                SetProperty(ref themeIndexSelected, value);
            }
        }


        private bool isSetModel;
        public bool IsSetModel
        {
            get { return isSetModel; }
            set
            {
                settings
                    .SetModelWindow(value)
                    .Build();
                SetProperty(ref isSetModel, value);
            }
        }

        private bool isAnimation;
        public bool IsAnimation
        {
            get { return isAnimation; }
            set
            {
                settings
                    .SetAnimation(value)
                    .Build();
                SetProperty(ref isAnimation, value);
            }
        }

        private int geolocationIndexSelected;
        public int GeolocationIndexSelected
        {
            get { return geolocationIndexSelected; }
            set
            {
                settings
                    .SetGeolocationMode((Geolocation)value)
                    .Build();
                UpdateUiGeolocation((Geolocation)value);
                SetProperty(ref geolocationIndexSelected, value);
            }
        }

        private double latitude;
        public double Latitude
        {
            get { return latitude; }
            set
            {
                settings
                    .SetLatitude(value)
                    .Build();
                SetProperty(ref latitude, value);
            }
        }


        private double longitude;
        public double Longitude
        {
            get { return longitude; }
            set
            {
                settings
                    .SetLongitude(value)
                    .Build();
                SetProperty(ref longitude, value);
            }
        }

        private bool isCustomResolution;
        public bool IsCustomResolution
        {
            get { return isCustomResolution; }
            set
            {
                settings
                    .SetUseCustomResolution(value)
                    .Build();
                IsEnableResolutionList = value;
                IsEnableResolutionCustom = value;
                SetProperty(ref isCustomResolution, value);
            }
        }


        private bool isEnableListLocation;
        public bool IsEnableListLocation
        {
            get { return isEnableListLocation; }
            set { SetProperty(ref isEnableListLocation, value); }
        }

        private bool isEnableCustomLocation;
        public bool IsEnableCustomLocation
        {
            get { return isEnableCustomLocation; }
            set { SetProperty(ref isEnableCustomLocation, value); }
        }


        private bool isEnableResolutionList;
        public bool IsEnableResolutionList
        {
            get { return isEnableResolutionList; }
            set { SetProperty(ref isEnableResolutionList, value); }
        }


        private int resolutionTemplateSelected;
        public int ResolutionTemplateSelected
        {
            get { return resolutionTemplateSelected; }
            set
            {
                settings
                    .SetResolutionTemplate(value)
                    .Build();
                SetProperty(ref resolutionTemplateSelected, value);
            }
        }


        private int resolutionModeSelected;
        public int ResolutionModeSelected
        {
            get { return resolutionModeSelected; }
            set
            {
                settings
                    .SetResolutionMode((ResolutionMode)value)
                    .Build();
                UpdateUiListResolution((ResolutionMode)value);
                SetProperty(ref resolutionModeSelected, value);
            }
        }

        private bool isEnableResolutionTemplate;
        public bool IsEnableResolutionTemplate
        {
            get { return isEnableResolutionTemplate; }
            set { SetProperty(ref isEnableResolutionTemplate, value); }
        }

        private bool isEnableResolutionCustom;
        public bool IsEnableResolutionCustom
        {
            get { return isEnableResolutionCustom; }
            set { SetProperty(ref isEnableResolutionCustom, value); }
        }

        private int imgResolutionWidth;
        public int ImgResolutionWidth
        {
            get { return imgResolutionWidth; }
            set
            {
                settings
                    .SetResolutionWidth(value)
                    .Build();
                SetProperty(ref imgResolutionWidth, value);
            }
        }

        private int imgResolutionHeight;
        public int ImgResolutionHeight
        {
            get { return imgResolutionHeight; }
            set
            {
                settings
                    .SetResolutionHeight(value)
                    .Build();
                SetProperty(ref imgResolutionHeight, value);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        private void SettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsAutorun = ((General)sender).AutoRun;
        }

        private void UpdateUiGeolocation(Geolocation geolocation)
        {
            switch (geolocation)
            {
                case Geolocation.Custom:
                    IsEnableCustomLocation = true;
                    break;
                case Geolocation.Auto:
                    IsEnableCustomLocation = false;
                    break;
                case Geolocation.Windows:
                    IsEnableCustomLocation = false;
                    break;
            }
        }

        private void UpdateUiListResolution(ResolutionMode value)
        {
            switch (value)
            {
                case ResolutionMode.Custom:
                    IsEnableResolutionCustom = true;
                    IsEnableResolutionTemplate = false;
                    break;
                case ResolutionMode.Template:
                    IsEnableResolutionCustom = false;
                    IsEnableResolutionTemplate = true;
                    break;
                case ResolutionMode.Auto:
                    IsEnableResolutionCustom = false;
                    IsEnableResolutionTemplate = false;
                    break;
                default:
                    IsEnableResolutionCustom = false;
                    IsEnableResolutionTemplate = false;
                    break;
            }
        }
    }
}