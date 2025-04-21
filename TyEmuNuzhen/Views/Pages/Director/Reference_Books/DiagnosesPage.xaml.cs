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
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Director.Reference_Books
{
    /// <summary>
    /// Логика взаимодействия для DiagnosesPage.xaml
    /// </summary>
    public partial class DiagnosesPage : Page
    {
        public DiagnosesPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDiagnoses("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow();
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!DiagnosesClass.GetSameDiagnosisName(null, referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данный диагноз уже есть в системе! Введите другое название диагноза.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DiagnosesClass.AddDiagnoses(referenceBookValuesWindow.tbValue.Text))
                return;
            LoadDiagnoses(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!DiagnosesClass.DeleteDiagnoses(deleteBtn.Tag.ToString()))
                return;
            LoadDiagnoses(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            string valueDataRow = DiagnosesClass.GetDiagnosisName(changeBtn.Tag.ToString());
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow(valueDataRow);
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!DiagnosesClass.GetSameDiagnosisName(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данный диагноз уже есть в системе! Введите другое название диагноза.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DiagnosesClass.UpdateDiagnoses(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
                return;
            LoadDiagnoses(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDiagnoses(querySearch);
            CountRecords();
        }

        private void LoadDiagnoses(string querySearch)
        {
            DiagnosesClass.GetDiagnosesList(querySearch);
            diagnosesGrid.ItemsSource = DiagnosesClass.dtDiagnosesList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = DiagnosesClass.GetCountAllDiagnoses();
            string countResRecords = DiagnosesClass.dtDiagnosesList.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
