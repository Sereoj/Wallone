using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Media;
using WinDynamicDesktop.Core.Helpers;
using WinDynamicDesktop.UI.Services;

namespace WinDynamicDesktop.UI.ViewModels
{
    public class UploaderViewModel : BindableBase
    {
        private static readonly BitmapHelper bitmapHelper = new BitmapHelper();
        private ImageSource cover;
        public ImageSource Cover { get => cover; set => SetProperty(ref cover, value); }
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
                Cover = bitmapHelper[UriHelper.Get(ProfileService.GetCover())];
            }
        }
    }
}
