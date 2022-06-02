using System;
using System.Windows.Controls;
using Wallone.Core.Helpers;

namespace Wallone.UI.Controls
{
    /// <summary>
    ///     Логика взаимодействия для Article.xaml
    /// </summary>
    public partial class Article : UserControl
    {
        public Article()
        {
            InitializeComponent();

            RandomColors.Background = new ColorHelper().Get();
            GC.Collect();
        }
    }
}