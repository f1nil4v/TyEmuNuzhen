using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TyEmuNuzhen.Views.Pages.Director.Reference_Books
{
    /// <summary>
    /// Логика взаимодействия для ReferenceBooksPage.xaml
    /// </summary>
    public partial class ReferenceBooksPage : Page
    {
        public ReferenceBooksPage()
        {
            InitializeComponent();
            referenceBookFrame.Navigate(new DiagnosesPage());
        }

        private void diagnoses_sLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagnoses_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            doctorPosts_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            regions_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            documentTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            medicalHelpTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            transportTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            referenceBookFrame.Navigate(new DiagnosesPage());
            referenceBookFrame.NavigationService.RemoveBackEntry();
        }

        private void doctorPosts_sLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagnoses_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            doctorPosts_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            regions_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            documentTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            medicalHelpTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            transportTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            referenceBookFrame.Navigate(new DoctorPostsPage());
            referenceBookFrame.NavigationService.RemoveBackEntry();
        }

        private void regions_sLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagnoses_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            doctorPosts_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            regions_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            documentTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            medicalHelpTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            transportTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            referenceBookFrame.Navigate(new RegionsPage());
            referenceBookFrame.NavigationService.RemoveBackEntry();
        }

        private void documentTypes_sLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagnoses_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            doctorPosts_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            regions_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            documentTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            medicalHelpTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            transportTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            referenceBookFrame.Navigate(new DocumentsTypePage());
            referenceBookFrame.NavigationService.RemoveBackEntry();
        }

        private void medicalHelpTypes_sLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagnoses_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            doctorPosts_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            regions_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            documentTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            medicalHelpTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            transportTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            referenceBookFrame.Navigate(new MedicalHelpTypePage());
            referenceBookFrame.NavigationService.RemoveBackEntry();
        }

        private void transportTypes_sLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagnoses_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            doctorPosts_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            regions_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            documentTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            medicalHelpTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            transportTypes_sLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            referenceBookFrame.Navigate(new TransportTypePage());
            referenceBookFrame.NavigationService.RemoveBackEntry();
        }
    }
}
