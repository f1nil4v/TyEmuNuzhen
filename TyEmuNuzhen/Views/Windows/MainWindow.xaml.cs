using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;


namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMenuOpen = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (isMenuOpen)
            {
                Storyboard sb = this.FindResource("CloseMenu") as Storyboard;
                sb.Begin();
            }
            else
            {
                Storyboard sb = this.FindResource("OpenMenu") as Storyboard;
                sb.Begin();
            }

            isMenuOpen = !isMenuOpen;
        }
    }
}
