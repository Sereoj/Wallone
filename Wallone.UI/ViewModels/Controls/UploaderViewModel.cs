using System.Windows.Media;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Wallone.Core.Helpers;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class UploaderViewModel : BindableBase
    {
        private static readonly BitmapHelper bitmapHelper = new BitmapHelper();
        private ImageSource cover;

        public UploaderViewModel()
        {
            ActionCommand = new DelegateCommand(OnAction);
        }

        public ImageSource Cover
        {
            get => cover;
            set => SetProperty(ref cover, value);
        }

        public DelegateCommand ActionCommand { get; set; }

        private void OnAction()
        {
            var fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                AccountService.SetCover(fileDialog.FileName);
                Cover = bitmapHelper[UriHelper.Get(fileDialog.FileName)];
            }
        }
    }
}