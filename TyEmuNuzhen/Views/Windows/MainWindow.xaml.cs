using System;
using System.Windows;
using System.Windows.Media.Animation;
using TyEmuNuzhen.MyClasses;


namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMenuOpen = false;
        private bool _closeAplication = true;
        public MainWindow(string idRole, string idEmployee, string post)
        {
            InitializeComponent();
            switch (idRole)
            {
                case "1":
                    volonteerMenu.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.MonitoringPage());
                    this.Title += $" - Волонтёр: {VolonteerClass.GetVolonteerFullName(idEmployee)} ({RegionsClass.GetRegionName(VolonteerClass.idRegion)})";
                    break;
                case "2":
                    curatorMenu.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.Curator.ChildrensPage());
                    this.Title += $" - Куратор: {CuratorClass.GetCuratorFullName(idEmployee)}";
                    break;
                default:
                    MessageBox.Show("Ошибка авторизации. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

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
            if (MessageBox.Show("Вы уверены, что хотите выйти из учётной записи?", "Выход", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                Window window = Application.Current.Windows[0];
                _closeAplication = false;
                this.Close();
                window.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!_closeAplication)
                return;
            Application.Current.Shutdown();
        }

    }
}
