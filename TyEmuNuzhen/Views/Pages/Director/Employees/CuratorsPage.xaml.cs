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

namespace TyEmuNuzhen.Views.Pages.Director.Employees
{
    /// <summary>
    /// Логика взаимодействия для CuratorsPage.xaml
    /// </summary>
    public partial class CuratorsPage : Page
    {
        public CuratorsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurators("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            AddUserWindow addUserWindow = new AddUserWindow(3);
            if (addUserWindow.ShowDialog() == false)
                return;
            LoadCurators(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            CuratorClass.GetCuratorData(deleteBtn.Tag.ToString());
            string idUser = CuratorClass.dtCuratorDataList.Rows[0]["idUser"].ToString();
            if (!CuratorClass.DeleteCurator(deleteBtn.Tag.ToString()) || !UserClass.DeleteUser(idUser))
                return;
            LoadCurators(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            CuratorClass.GetCuratorData(changeBtn.Tag.ToString());
            string idUser = CuratorClass.dtCuratorDataList.Rows[0]["idUser"].ToString();
            string idRole = CuratorClass.dtCuratorDataList.Rows[0]["idRole"].ToString();
            string login = CuratorClass.dtCuratorDataList.Rows[0]["login"].ToString();
            string surname = CuratorClass.dtCuratorDataList.Rows[0]["surname"].ToString();
            string name = CuratorClass.dtCuratorDataList.Rows[0]["name"].ToString();
            string middleName = CuratorClass.dtCuratorDataList.Rows[0]["middleName"].ToString();
            string phoneNumber = CuratorClass.dtCuratorDataList.Rows[0]["phoneNumber"].ToString();
            string email = CuratorClass.dtCuratorDataList.Rows[0]["email"].ToString();
            AddUserWindow addUserWindow = new AddUserWindow(changeBtn.Tag.ToString(), login, surname, name, middleName, phoneNumber, email, "", idUser, idRole, 3);
            if (addUserWindow.ShowDialog() == false)
                return;
            LoadCurators(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadCurators(querySearch);
            CountRecords();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadCurators(querySearch);
            CountRecords();
        }

        private void LoadCurators(string querySearch)
        {
            CuratorClass.GetCuratorsList(querySearch, SortValue());
            curatorsGrid.ItemsSource = CuratorClass.dtCuratorsList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = CuratorClass.GetCountAllCurators();
            string countResRecords = CuratorClass.dtCuratorsList.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }

        private string SortValue()
        {
            string sortValue;
            int sortIndex = sortCmbBox.SelectedIndex;
            switch (sortIndex)
            {
                case 0:
                    sortValue = "curators.surname";
                    break;
                case 1:
                    sortValue = "curators.name";
                    break;
                case 2:
                    sortValue = "curators.middleName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }
    }
}
