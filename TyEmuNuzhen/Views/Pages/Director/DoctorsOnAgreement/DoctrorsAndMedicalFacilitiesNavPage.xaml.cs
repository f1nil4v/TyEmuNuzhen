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
using TyEmuNuzhen.Views.Pages.Director.Employees;

namespace TyEmuNuzhen.Views.Pages.Director.DoctorsOnAgreement
{
    /// <summary>
    /// Логика взаимодействия для DoctrorsAndMedicalFacilitiesNavPage.xaml
    /// </summary>
    public partial class DoctrorsAndMedicalFacilitiesNavPage : Page
    {
        public DoctrorsAndMedicalFacilitiesNavPage()
        {
            InitializeComponent();
            childFrame.Navigate(new DoctorsOnAgreementPage());
            childFrame.NavigationService.RemoveBackEntry();
        }

        private void doctorsLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            doctorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            medicalFacilitiesLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            childFrame.Navigate(new DoctorsOnAgreementPage());
            childFrame.NavigationService.RemoveBackEntry();
        }

        private void medicalFacilitiesLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            doctorsLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            medicalFacilitiesLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            childFrame.Navigate(new MedicalFacilitiesPage());
            childFrame.NavigationService.RemoveBackEntry();
        }
    }
}
