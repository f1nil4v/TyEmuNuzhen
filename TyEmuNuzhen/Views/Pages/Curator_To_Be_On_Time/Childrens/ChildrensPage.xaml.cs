using MaterialDesignColors.Recommended;
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

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens
{
    /// <summary>
    /// Логика взаимодействия для ChildrensPage.xaml
    /// </summary>
    public partial class ChildrensPage : Page
    {
        public ChildrensPage()
        {
            InitializeComponent();
            childrensFrame.Navigate(new MedicalExamination.MedicalExaminationChildrensPage());
            childrensFrame.NavigationService.RemoveBackEntry();
        }

        private void medicalExaminationLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            medicalExaminationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            toBeOnTimeLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            workCompleteLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            noProblemLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            childrensFrame.Navigate(new MedicalExamination.MedicalExaminationChildrensPage());
            childrensFrame.NavigationService.RemoveBackEntry();
        }

        private void toBeOnTimeLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            medicalExaminationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            toBeOnTimeLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            workCompleteLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            noProblemLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            childrensFrame.Navigate(new ToBeOnTime.ToBeOnTimePage());
            childrensFrame.NavigationService.RemoveBackEntry();
        }

        private void noProblemLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            medicalExaminationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            toBeOnTimeLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            workCompleteLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            noProblemLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            //childrensFrame.Navigate(new NoProblemPage());
            //childrensFrame.NavigationService.RemoveBackEntry();
        }

        private void workCompleteLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            medicalExaminationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            toBeOnTimeLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            workCompleteLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            noProblemLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            //childrensFrame.Navigate(new CompletedWorksPage());
            //childrensFrame.NavigationService.RemoveBackEntry();
        }
    }
}
