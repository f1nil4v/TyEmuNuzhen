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
using TyEmuNuzhen.MyClasses;
using System.IO;
using Microsoft.Win32;

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для SelectProgramAndPeriodWindow.xaml
    /// </summary>
    public partial class SelectProgramAndPeriodWindow : Window
    {
        private string _idChild;
        public string idProgram;
        public string filePathConsent;

        public SelectProgramAndPeriodWindow(string idChild)
        {
            InitializeComponent();
            _idChild = idChild;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProgramTypeClass.GetProgramTypeList();
            programTypeCmbBox.ItemsSource = ProgramTypeClass.dtProgramTypeList?.DefaultView;
            programTypeCmbBox.DisplayMemberPath = "programType";
            programTypeCmbBox.SelectedValuePath = "ID";
            programTypeCmbBox.SelectedIndex = 0;
            LoadProgramsHistory();
        }

        private void dateBeginPeriodPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateEndPeriodPicker.SelectedDate != null && dateEndPeriodPicker.SelectedDate >= dateBeginPeriodPicker.SelectedDate)
                LoadProgramsHistory();
        }

        private void dateEndPeriodPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateBeginPeriodPicker.SelectedDate != null && dateEndPeriodPicker.SelectedDate >= dateBeginPeriodPicker.SelectedDate)
                LoadProgramsHistory();
        }

        private void btnRefreshFiltrationDate_Click(object sender, RoutedEventArgs e)
        {
            dateBeginPeriodPicker.SelectedDate = null;
            dateEndPeriodPicker.SelectedDate = null;
            LoadProgramsHistory();
        }

        private void downloadReportBtn_Click(object sender, RoutedEventArgs e)
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

        private void selectProgramBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectProgramBtn = sender as Button;
            idProgram = selectProgramBtn.Tag.ToString();
            filePathConsent = ConsentsClass.GetFilePathAppealConsent(idProgram);
            DialogResult = true;
        }

        private void programTypeCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProgramsHistory();
        }

        private void LoadProgramsHistory()
        {
            string dateBeginPeriod = dateBeginPeriodPicker.SelectedDate == null ? null : dateBeginPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string dateEndPeriod = dateEndPeriodPicker.SelectedDate == null ? null : dateEndPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string idProgramType = programTypeCmbBox.SelectedValue.ToString();
            ActualProgramClass.GetProgramsHistoryList(_idChild, idProgramType, dateBeginPeriod, dateEndPeriod);
            programsHistoryGrid.ItemsSource = ActualProgramClass.dtProgramsHistoryList?.DefaultView;
        }
    }
}
