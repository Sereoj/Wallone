using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WinDynamicDesktop.UI.ViewModels;

namespace WinDynamicDesktop.UI.Interfaces
{
    interface IPage
    {
        public ObservableCollection<ArticleViewModel> Library { get; set; }
        public string Header { get; set; }
    }
}
