using System;
using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;
using TyEmuNuzhen.Views.Windows.DialogWindows.ReferenceBooks;

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
            HelpManagerClass.CurrentHelpKey = "DirectorReferMedicalCareTypePage";
            LoadMedicalHelpTypes("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            MedicalTypeWindow medicalTypeWindow = new MedicalTypeWindow();
            if (medicalTypeWindow.ShowDialog() == false)
                return;
            LoadMedicalHelpTypes(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
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
            MedicalTypeWindow medicalTypeWindow = new MedicalTypeWindow(changeBtn.Tag.ToString());
            if (medicalTypeWindow.ShowDialog() == false)
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
