using Prism.Mvvm;

namespace Wallone.Core.Models.Settings
{
    public class General : BindableBase
    {
        private string theme;
        public string Theme
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
    }
}