using System.Windows.Controls;
using Wallone.Core.Helpers;

namespace Wallone.UI.Controls
{
    /// <summary>
    ///     Логика взаимодействия для ArticleMedium.xaml
    /// </summary>
    public partial class ArticleMedium : UserControl
    {
        public ArticleMedium()
        {
            InitializeComponent();
            RandomColors.Background = new ColorHelper().Get();
        }
    }
}