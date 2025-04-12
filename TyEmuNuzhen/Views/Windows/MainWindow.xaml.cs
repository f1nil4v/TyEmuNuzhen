using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private List<Button> menuButtons;
        public MainWindow(string idRole, string idEmployee, string post)
        {
            InitializeComponent();
            menuButtons = new List<Button> { btnVolonteerMonitoring, btnConsultation, btnCuratorMonitoring, btnPreliminary, btnChildrens, btnArchive, btnProfile };
            switch (idRole)
            {
                case "1":
                    volonteerMenu.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.MonitoringPage());
                    this.Title += $" - Волонтёр: {VolonteerClass.GetVolonteerFullName(idEmployee)} ({RegionsClass.GetRegionName(VolonteerClass.idRegion)})";
                    SetActiveMenuItem(btnVolonteerMonitoring);
                    break;
                case "2":
                    curatorMenu.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.Curator.ChildrensWork.MonitoringPage());
                    this.Title += $" - Куратор: {CuratorClass.GetCuratorFullName(idEmployee)}";
                    SetActiveMenuItem(btnCuratorMonitoring);
                    break;
                default:
                    MessageBox.Show("Ошибка авторизации. Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private void SetActiveMenuItem(Button activeButton)
        {
            foreach (Button button in menuButtons)
            {
                button.Background = Brushes.Transparent;
            }
            activeButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#A3B92E");
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

        private void btnConsultation_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnConsultation);
        }

        private void btnCuratorMonitoring_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnCuratorMonitoring);
            mainFrame.Navigate(new Pages.Curator.ChildrensWork.MonitoringPage());
        }

        private void btnPreliminary_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnPreliminary);
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.PreliminaryInWork.PreliminaryInWorkPage());
        }

        private void btnChildrens_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnChildrens);
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage());
        }

        private void btnArchive_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnArchive);
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnProfile);
        }

        private void btnVolonteerMonitoring_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnVolonteerMonitoring);
        }
    }
}
