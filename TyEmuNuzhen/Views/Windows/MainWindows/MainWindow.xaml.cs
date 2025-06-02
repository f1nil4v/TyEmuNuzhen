using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Pages.Director.Employees;
using TyEmuNuzhen.Views.Windows.DialogWindows;


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
            menuButtons = new List<Button> { btnVolonteerMonitoring, btnNannies, btnCuratorMonitoring, btnPreliminary, btnChildrens, btnEmploees, btnRefernceBooks, btnDoctorsOnAgreement, btnOrphanages, btnSettings, };
            switch (idRole)
            {
                case "1":
                    volonteerMenu.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.MonitoringPage());
                    mainFrame.NavigationService.RemoveBackEntry();
                    this.Title += $" - Волонтёр: {VolonteerClass.GetVolonteerFullName(idEmployee)} ({RegionsClass.GetRegionName(VolonteerClass.idRegion)})";
                    SetActiveMenuItem(btnVolonteerMonitoring);
                    break;
                case "2":
                    curatorMenu.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.Curator.ChildrensWork.MonitoringPage());
                    mainFrame.NavigationService.RemoveBackEntry();
                    this.Title += $" - Куратор: {CuratorClass.GetCuratorFullName(idEmployee)}";
                    SetActiveMenuItem(btnCuratorMonitoring);
                    break;
                case "3":
                    directorMenu.Visibility = Visibility.Visible;
                    btnSettings.Visibility = Visibility.Visible;
                    mainFrame.Navigate(new Pages.Director.Employees.EmployeesPage());
                    mainFrame.NavigationService.RemoveBackEntry();
                    this.Title += $" - Директор: {DirectorClass.GetDirectorFullName(idEmployee)}";
                    SetActiveMenuItem(btnEmploees);
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

        private void btnNannies_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnNannies);
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.Nannies.NanniesPage());
        }

        private void btnCuratorMonitoring_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnCuratorMonitoring);
            mainFrame.Navigate(new Pages.Curator.ChildrensWork.MonitoringPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnPreliminary_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnPreliminary);
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.PreliminaryInWork.PreliminaryInWorkPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnChildrens_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnChildrens);
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage(1));
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnVolonteerMonitoring_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnVolonteerMonitoring);
            mainFrame.Navigate(new Pages.MonitoringPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnEmploees_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnEmploees);
            mainFrame.Navigate(new Pages.Director.Employees.EmployeesPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnRefernceBooks_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnRefernceBooks);
            mainFrame.Navigate(new Pages.Director.Reference_Books.ReferenceBooksPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnDoctorsOnAgreement_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnDoctorsOnAgreement);
            mainFrame.Navigate(new Pages.Director.DoctorsOnAgreement.DoctrorsAndMedicalFacilitiesNavPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }
        
        private void btnOrphanages_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnOrphanages);
            mainFrame.Navigate(new Pages.Director.Orphanages.OrphanagesPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(btnSettings);
            mainFrame.Navigate(new Pages.Director.Settings.SettingsPage());
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1)
            {
                HelpManagerClass.ShowHelp();
                e.Handled = true;
            }
        }
    }
}
