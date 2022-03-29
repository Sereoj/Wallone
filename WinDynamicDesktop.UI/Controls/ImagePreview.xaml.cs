using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
            //FrontThumb.Tag = FrontThumb;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Trace.WriteLine("C: " + ActualWidth);
        }
    }
}
