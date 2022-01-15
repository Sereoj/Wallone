using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WinDynamicDesktop.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для ImagePreview.xaml
    /// </summary>
    public partial class ImagePreview : UserControl
    {
        public ImagePreview()
        {
            InitializeComponent();
            FrontThumb.Tag = FrontThumb;
        }
    }
}
