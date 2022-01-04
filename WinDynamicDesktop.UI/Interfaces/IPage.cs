using System.Collections.ObjectModel;
using WinDynamicDesktop.UI.ViewModels;

namespace WinDynamicDesktop.UI.Interfaces
{
    interface IPage
    {
        public ObservableCollection<ArticleViewModel> Library { get; set; }
        public string Header { get; set; }
    }
}
