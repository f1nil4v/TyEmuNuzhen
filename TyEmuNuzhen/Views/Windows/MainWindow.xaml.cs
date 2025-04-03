using System;
using System.Windows;
using System.Windows.Media.Animation;


namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isMenuOpen = false;

        public MainWindow(string idVolonteer, string idRegion)
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
            if (MessageBox.Show("Закрыть приложение?", "Выход", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
