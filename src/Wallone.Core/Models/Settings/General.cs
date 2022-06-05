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
    }
}