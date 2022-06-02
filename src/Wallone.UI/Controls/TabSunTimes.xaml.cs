using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wallone.UI.Controls
{
    /// <summary>
    /// Логика взаимодействия для TabSunTimes.xaml
    /// </summary>
    public partial class TabSunTimes : UserControl
    {
        public TabSunTimes()
        {
            InitializeComponent();
        }

        private void TabSunTimes_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ActualWidth > 625)
            {
                SunrisePanel.Visibility = Visibility.Visible;
                DayPanel.Visibility = Visibility.Visible;
                SunsetPanel.Visibility = Visibility.Visible;
                NightPanel.Visibility = Visibility.Visible;
            }
            else
            {
                SunrisePanel.Visibility = Visibility.Collapsed;
                SunsetPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
