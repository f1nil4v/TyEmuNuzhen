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
    /// Логика взаимодействия для TransportTypePage.xaml
    /// </summary>
    public partial class TransportTypePage : Page
    {
        public TransportTypePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTransportTypes("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow();
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!TransportTypesClass.GetSameTranposrtType(null, referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!TransportTypesClass.AddTranposrtType(referenceBookValuesWindow.tbValue.Text))
                return;
            LoadTransportTypes(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!TransportTypesClass.DeleteTranposrtType(deleteBtn.Tag.ToString()))
                return;
            LoadTransportTypes(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            string valueDataRow = TransportTypesClass.GetTranposrtTypeName(changeBtn.Tag.ToString());
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow(valueDataRow);
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!TransportTypesClass.GetSameTranposrtType(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!TransportTypesClass.UpdateTranposrtType(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
                return;
            LoadTransportTypes(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadTransportTypes(querySearch);
            CountRecords();
        }

        private void LoadTransportTypes(string querySearch)
        {
            TransportTypesClass.GetTranposrtTypesList(querySearch);
            transportTypesGrid.ItemsSource = TransportTypesClass.dtTranposrtTypeS.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = TransportTypesClass.GetCountAllTranposrtTypes();
            string countResRecords = TransportTypesClass.dtTranposrtTypeS.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
