using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TyEmuNuzhen.Views.Pages.Director.Reference_Books;

namespace TyEmuNuzhen.Views.Pages.Director.Employees
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        public EmployeesPage()
        {
            InitializeComponent();
            employeesFrame.Navigate(new DirectorsPage());
        }

        private void directorsLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            directorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            curatorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            volonteersLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            employeesFrame.Navigate(new DirectorsPage());
            employeesFrame.NavigationService.RemoveBackEntry();
        }

        private void curatorsLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            directorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            curatorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            volonteersLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            employeesFrame.Navigate(new CuratorsPage());
            employeesFrame.NavigationService.RemoveBackEntry();
        }

        private void volonteersLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            directorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            curatorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            volonteersLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            employeesFrame.Navigate(new VolonteersPage());
            employeesFrame.NavigationService.RemoveBackEntry();
        }
    }
}
