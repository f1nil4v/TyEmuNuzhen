using System;
using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Director.Reference_Books
{
    /// <summary>
    /// Логика взаимодействия для MedicalHelpTypePage.xaml
    /// </summary>
    public partial class MedicalHelpTypePage : Page
    {
        public MedicalHelpTypePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMedicalHelpTypes("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow();
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!MedicalHelpTypeClass.GetSameMedicalHelpType(null, referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!MedicalHelpTypeClass.AddMedicalHelpType(referenceBookValuesWindow.tbValue.Text))
                return;
            LoadMedicalHelpTypes(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!MedicalHelpTypeClass.DeleteMedicalHelpType(deleteBtn.Tag.ToString()))
                return;
            LoadMedicalHelpTypes(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            string valueDataRow = MedicalHelpTypeClass.GetMedicalHelpTypeName(changeBtn.Tag.ToString());
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow(valueDataRow);
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!MedicalHelpTypeClass.GetSameMedicalHelpType(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!MedicalHelpTypeClass.UpdateMedicalHelpType(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
                return;
            LoadMedicalHelpTypes(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadMedicalHelpTypes(querySearch);
            CountRecords();
        }

        private void LoadMedicalHelpTypes(string querySearch)
        {
            MedicalHelpTypeClass.GetMedicalHelpTypesList(querySearch);
            medicalHelpTypesGrid.ItemsSource = MedicalHelpTypeClass.dtMedicalHelpTypeS.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = MedicalHelpTypeClass.GetCountAllMedicalHelpTypes();
            string countResRecords = MedicalHelpTypeClass.dtMedicalHelpTypeS.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
