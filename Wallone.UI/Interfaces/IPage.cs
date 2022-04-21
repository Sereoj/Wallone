using System.Collections.ObjectModel;
using Wallone.UI.ViewModels.Controls;

namespace Wallone.UI.Interfaces
{
    interface IPage
    {
        public ObservableCollection<ArticleViewModel> Library { get; set; }
        public string Header { get; set; }
    }
}
