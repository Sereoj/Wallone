using Prism.Mvvm;

namespace Wallone.Controls.ViewModels
{
    public class UpdateViewModel : BindableBase
    {
        private int status;


        private string versionActual;


        private string versionCurrent;

        public int Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string VersionCurrect
        {
            get => versionCurrent;
            set => SetProperty(ref versionCurrent, value);
        }

        public string VersionActual
        {
            get => versionActual;
            set => SetProperty(ref versionActual, value);
        }

        public void SetStatus(int index)
        {
            Status = index;
        }

        public void SetCurrentVersion(string verionCurrent)
        {
            VersionCurrect = verionCurrent;
        }

        public void SetActualVersion(string verionActual)
        {
            VersionActual = verionActual;
        }

        public bool IsShow()
        {
            return Status != 0;
        }
    }
}