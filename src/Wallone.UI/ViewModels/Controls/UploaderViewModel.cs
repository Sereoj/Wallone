using System;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wallone.Core.Helpers;
using Wallone.UI.Services;

namespace Wallone.UI.ViewModels.Controls
{
    public class UploaderViewModel : BindableBase
    {
        private Uri cover;

        public UploaderViewModel()
        {
            ActionCommand = new DelegateCommand(OnAction);
        }

        public Uri Cover
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
                Cover = UriHelper.Get(fileDialog.FileName);
            }
        }
    }
}