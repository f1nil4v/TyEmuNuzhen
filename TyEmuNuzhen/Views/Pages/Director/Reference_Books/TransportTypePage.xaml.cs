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
using TyEmuNuzhen.Views.Windows.DialogWindows.ReferenceBooks;

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
            TransportTypesWindow transportTypesWindow = new TransportTypesWindow();
            if (transportTypesWindow.ShowDialog() == false)
                return;
            LoadTransportTypes(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
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
            TransportTypesWindow transportTypesWindow = new TransportTypesWindow(changeBtn.Tag.ToString());
            if (transportTypesWindow.ShowDialog() == false)
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
            TransportTypesClass.GetTranportTypesList(querySearch);
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
