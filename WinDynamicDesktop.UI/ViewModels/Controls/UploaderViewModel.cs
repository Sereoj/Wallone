using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class UploaderViewModel : BindableBase
    {

        private string file;
        public string File
        {
            get { return file; }
            set { SetProperty(ref file, value); }
        }
        public DelegateCommand ActionCommand { get; set; }
        public UploaderViewModel()
        {
            ActionCommand = new DelegateCommand(OnAction);
        }

        private void OnAction()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                File = fileDialog.FileName;
            }
        }
    }
}
