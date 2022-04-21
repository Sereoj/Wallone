using System;
using System.IO;
using System.Windows;

namespace Wallone.UI.Test.Loader
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var file = Path.Combine(Environment.CurrentDirectory, "Logo.svg");

            if (File.Exists(file))
            {
                Logo.UriSource = new Uri(file);
            }
            else
            {
                Logo.UriSource = null;
                MessageBox.Show("Нет файла " + file);
            }
        }
    }
}