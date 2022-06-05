using Prism.Mvvm;

namespace Wallone.Core.Models.Settings
{
    public class General : BindableBase
    {
        private ModernWpf.ElementTheme theme;
        public ModernWpf.ElementTheme Theme
        {
            get { return theme; }
            set { SetProperty(ref theme, value); }
        }

        private string image;
        public string Image
        {
            get { return image; }
            set { SetProperty(ref image, value); }
        }

        private bool autoRun;
        public bool AutoRun
        {
            get { return autoRun; }
            set { SetProperty(ref autoRun, value); }
        }


        private bool geolocation;
        public bool Geolocation
        {
            get { return geolocation; }
            set { SetProperty(ref geolocation, value); }
        }


        private Geolocation geolocationMode;
        public Geolocation GeolocationMode
        {
            get { return geolocationMode; }
            set { SetProperty(ref geolocationMode, value); }
        }

        private bool autoSetImage;
        public bool AutoSetImage
        {
            get { return autoSetImage; }
            set { SetProperty(ref autoSetImage, value); }
        }

        private bool isArcticModal;
        public bool IsArcticModal
        {
            get { return isArcticModal; }
            set { SetProperty(ref isArcticModal, value); }
        }


        private bool animation;
        public bool Animation
        {
            get { return animation; }
            set { SetProperty(ref animation, value); }
        }

        private bool useCustomResolution;
        public bool UseCustomResolution
        {
            get { return useCustomResolution; }
            set { SetProperty(ref useCustomResolution, value); }
        }

        private ResolutionMode resolutionMode;
        public ResolutionMode ResolutionMode
        {
            get { return resolutionMode; }
            set { SetProperty(ref resolutionMode, value); }
        }

        private int resolutionTemplate;
        public int ResolutionTemplate
        {
            get { return resolutionTemplate; }
            set { SetProperty(ref resolutionTemplate, value); }
        }

        private int resolutionWidth;
        public int ResolutionWidth
        {
            get { return resolutionWidth; }
            set { SetProperty(ref resolutionWidth, value); }
        }


        private int resolutionHeight;
        public int ResolutionHeight
        {
            get { return resolutionHeight; }
            set { SetProperty(ref resolutionHeight, value); }
        }
    }
}