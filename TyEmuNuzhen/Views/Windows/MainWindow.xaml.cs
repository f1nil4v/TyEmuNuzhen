using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using static System.Net.WebRequestMethods;


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
            MainFrame.Navigate(new Pages.MonitoringPage());
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

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {

            DialogResult result = System.Windows.Forms.MessageBox.Show("Вы уверены, что хотите выйти?", "Выход", (MessageBoxButtons)MessageBoxButton.YesNo, (MessageBoxIcon)MessageBoxImage.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
                Window window = System.Windows.Application.Current.MainWindow;
                window.Close();
            }
        }
    }
}
