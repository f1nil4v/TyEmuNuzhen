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
    /// Логика взаимодействия для VolonteersPage.xaml
    /// </summary>
    public partial class VolonteersPage : Page
    {
        public VolonteersPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RegionsClass.GetRegionsList();
            regionsCmbBox.ItemsSource = RegionsClass.dtRegions?.DefaultView;
            regionsCmbBox.DisplayMemberPath = "regionName";
            regionsCmbBox.SelectedValuePath = "ID";
            regionsCmbBox.SelectedIndex = 0;
            LoadVolonteers("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            AddUserWindow addUserWindow = new AddUserWindow(2);
            if (addUserWindow.ShowDialog() == false)
                return;
            LoadVolonteers(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            VolonteerClass.GetVolonteerData(deleteBtn.Tag.ToString());
            string idUser = VolonteerClass.dtVolonteerDataList.Rows[0]["idUser"].ToString();
            if (!VolonteerClass.DeleteVolonteer(deleteBtn.Tag.ToString()) || !UserClass.DeleteUser(idUser))
                return;
            LoadVolonteers(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            VolonteerClass.GetVolonteerData(changeBtn.Tag.ToString());
            string idUser = VolonteerClass.dtVolonteerDataList.Rows[0]["idUser"].ToString();
            string regionName = VolonteerClass.dtVolonteerDataList.Rows[0]["regionName"].ToString();
            string login = VolonteerClass.dtVolonteerDataList.Rows[0]["login"].ToString();
            string surname = VolonteerClass.dtVolonteerDataList.Rows[0]["surname"].ToString();
            string name = VolonteerClass.dtVolonteerDataList.Rows[0]["name"].ToString();
            string middleName = VolonteerClass.dtVolonteerDataList.Rows[0]["middleName"].ToString();
            string phoneNumber = VolonteerClass.dtVolonteerDataList.Rows[0]["phoneNumber"].ToString();
            string email = VolonteerClass.dtVolonteerDataList.Rows[0]["email"].ToString();
            AddUserWindow addUserWindow = new AddUserWindow(changeBtn.Tag.ToString(), login, surname, name, middleName, phoneNumber, email, regionName, idUser, "", 2);
            if (addUserWindow.ShowDialog() == false)
                return;
            LoadVolonteers(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadVolonteers(querySearch);
            CountRecords();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadVolonteers(querySearch);
            CountRecords();
        }

        private void regionsCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadVolonteers(querySearch);
            CountRecords();
        }

        private void LoadVolonteers(string querySearch)
        {
            string idRegion = regionsCmbBox.SelectedValue == null ? null : regionsCmbBox.SelectedValue.ToString();
            VolonteerClass.GetVolonteersList(querySearch, idRegion, SortValue());
            volonteersGrid.ItemsSource = VolonteerClass.dtVolonteersList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = VolonteerClass.GetCountAllVolonteers();
            string countResRecords = VolonteerClass.dtVolonteersList.Rows.Count.ToString();
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
                    sortValue = "volunteers.surname";
                    break;
                case 1:
                    sortValue = "volunteers.name";
                    break;
                case 2:
                    sortValue = "volunteers.middleName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }
    }
}
