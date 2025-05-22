using System;
using System.IO;
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
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using TyEmuNuzhen.Views.Windows;
using Microsoft.Win32;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks.HistoryProgram
{
    /// <summary>
    /// Логика взаимодействия для NanniesPage.xaml
    /// </summary>
    public partial class NanniesPage : Page
    {
        private string _idActualProgram;

        public NanniesPage(string idActualProgram)
        {
            InitializeComponent();
            _idActualProgram = idActualProgram;
            LoadNannies();
        }

        private void downloadAgreementBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadBtn = sender as Button;
            string originalFileName = Path.GetFileName(downloadBtn.Tag.ToString());
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = "Документы Word(*.docx) | *.docx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(downloadBtn.Tag.ToString(), selectedPath);
            }
        }

        private void downloadActOfCompletedWorksBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadBtn = sender as Button;
            string originalFileName = Path.GetFileName(downloadBtn.Tag.ToString());
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = "Документы Word(*.docx) | *.docx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(downloadBtn.Tag.ToString(), selectedPath);
            }
        }
        private void LoadNannies()
        {
            NanniesOnProgramClass.GetHistoryNannyOnProgramData(_idActualProgram);
            nanniesGrid.ItemsSource = NanniesOnProgramClass.dtHistoryNannyOnProgramData.DefaultView;
        }
    }
}
