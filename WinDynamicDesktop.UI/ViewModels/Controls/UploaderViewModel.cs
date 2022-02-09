using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class UploaderViewModel : BindableBase
    {
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
                AccountService.SetCover(fileDialog.FileName);
            }
        }
    }
}
