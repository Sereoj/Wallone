using Prism.Mvvm;

namespace Wallone.Controls.ViewModels
{
    public class NoConnectServerViewModel : BindableBase
    {
        private bool status;

        public bool Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public void SetStatus(bool status)
        {
            Status = status;
        }

        public bool IsShow()
        {
            return Status;
        }
    }
}