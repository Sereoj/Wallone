using Prism.Mvvm;

namespace Wallone.Controls.ViewModels
{
    public class UpdateViewModel : BindableBase
    {

        private int status;
        public int Status
        {
            get { return status; }
            set { SetProperty(ref status, value); }
        }


        private string versionCurrent;
        public string VersionCurrect
        {
            get { return versionCurrent; }
            set { SetProperty(ref versionCurrent, value); }
        }


        private string versionActual;
        public string VersionActual
        {
            get { return versionActual; }
            set { SetProperty(ref versionActual, value); }
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
