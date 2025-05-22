using Microsoft.Win32;
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
using System.IO;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ProgramHistoryWindow.xaml
    /// </summary>
    public partial class ProgramHistoryWindow : Window
    {
        private string _idActualProgram;

        public ProgramHistoryWindow(string idActualProgram, string filePath)
        {
            InitializeComponent();
            _idActualProgram = idActualProgram;
            downloadConsentLbl.Tag = filePath;
            hospitalizationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            nanniesLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks.HistoryProgram.HospitalizationPage(_idActualProgram));
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void hospitalizationLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            hospitalizationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            nanniesLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks.HistoryProgram.HospitalizationPage(_idActualProgram));
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void nanniesLbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            hospitalizationLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#6D6B6E");
            nanniesLbl.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");
            mainFrame.Navigate(new Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks.HistoryProgram.NanniesPage(_idActualProgram));
            mainFrame.NavigationService.RemoveBackEntry();
        }

        private void downloadConsentLbl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string originalFileName = Path.GetFileName(downloadConsentLbl.Tag.ToString());
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = "Документы Word(*.docx) | *.docx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(downloadConsentLbl.Tag.ToString(), selectedPath);
            }
        }
    }
}
