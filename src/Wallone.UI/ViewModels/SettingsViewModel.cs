using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;
using Wallone.Core.Builders;
using Wallone.Core.Helpers;
using Wallone.Core.Models;
using Wallone.Core.Models.Settings;
using Wallone.Core.Services.App;

namespace Wallone.UI.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private bool isAutorun;
        private bool isGeolocation;

        private string name = "Настройки";

        private SettingsItemBuilder settings;

        public DelegateCommand HelperGeoCommand { get; set; }
        public DelegateCommand HelperImageCommand { get; set; }
        public SettingsViewModel()
        {
        }

        public SettingsViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            settings = new SettingsBuilder(SettingsRepository.Get())
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

            //
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
            IsLog = settings.GetLog();
            settings.Build();

            SettingsRepository.Get().General.PropertyChanged += SettingsChanged;

            HelperGeoCommand = new DelegateCommand(OnHelperGeo);
            HelperImageCommand = new DelegateCommand(OnHelperImage);
        }

        private void OnHelperImage()
        {
          
        }

        private void OnHelperGeo()
        {
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
                    .SwitcherAutorun(AppSettingsRepository.AppSettingsService.GetApplicationPath(), value);
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
            get => isAutoSetImage;
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
            get => themeIndexSelected;
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
            get => isSetModel;
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
            get => isAnimation;
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
            get => geolocationIndexSelected;
            set
            {
                settings
                    .SetGeolocationMode(IntToGeolocation(value))
                    .Build();
                UpdateUiGeolocation(IntToGeolocation(value));
                SetProperty(ref geolocationIndexSelected, value);
            }
        }

        private Geolocation IntToGeolocation(int value)
        {
            return value switch
            {
                0 => Geolocation.Custom,
                1 => Geolocation.Auto,
                2 => Geolocation.Windows,
                _ => Geolocation.Auto,
            };
        }

        private double latitude;
        public double Latitude
        {
            get => latitude;
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
            get => longitude;
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
            get => isCustomResolution;
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
            get => isEnableListLocation;
            set { SetProperty(ref isEnableListLocation, value); }
        }

        private bool isEnableCustomLocation;
        public bool IsEnableCustomLocation
        {
            get => isEnableCustomLocation;
            set { SetProperty(ref isEnableCustomLocation, value); }
        }


        private bool isEnableResolutionList;
        public bool IsEnableResolutionList
        {
            get => isEnableResolutionList;
            set { SetProperty(ref isEnableResolutionList, value); }
        }


        private int resolutionTemplateSelected;
        public int ResolutionTemplateSelected
        {
            get => resolutionTemplateSelected;
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
            get => resolutionModeSelected;
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
            get => isEnableResolutionTemplate;
            set { SetProperty(ref isEnableResolutionTemplate, value); }
        }

        private bool isEnableResolutionCustom;
        public bool IsEnableResolutionCustom
        {
            get => isEnableResolutionCustom;
            set { SetProperty(ref isEnableResolutionCustom, value); }
        }

        private int imgResolutionWidth;
        public int ImgResolutionWidth
        {
            get => imgResolutionWidth;
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
            get => imgResolutionHeight;
            set
            {
                settings
                    .SetResolutionHeight(value)
                    .Build();
                SetProperty(ref imgResolutionHeight, value);
            }
        }

        private bool isLog;
        public bool IsLog
        {
            get => isLog;
            set
            {
                settings
                    .SetLog(value)
                    .Build();
                new AppLoggerBuilder()
                    .Set(value);
                SetProperty(ref isLog, value);
            }
        }

        private string themePath;
        public string ThemePath
        {
            get => themePath;
            set => SetProperty(ref themePath, value);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ThemePath = AppSettingsRepository.AppSettingsService.GetThemesLocation();
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
            if (sender != null)
            {
                var general = (General)sender;
                IsAutorun = general.AutoRun;
            }
        }

        private void UpdateUiGeolocation(Geolocation geolocation)
        {
            switch (geolocation)
            {
                case Geolocation.Auto:
                    IsEnableCustomLocation = false;
                    break;
                case Geolocation.Windows:
                    IsEnableCustomLocation = false;
                    break;
                case Geolocation.Custom:
                    IsEnableCustomLocation = true;
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