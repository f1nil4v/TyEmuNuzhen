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
using TyEmuNuzhen.Views.Windows.DialogWindows;

namespace TyEmuNuzhen.Views.Pages.Director.Employees
{
    /// <summary>
    /// Логика взаимодействия для DirectorsPage.xaml
    /// </summary>
    public partial class DirectorsPage : Page
    {
        public DirectorsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDirectors("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            AddUserWindow addUserWindow = new AddUserWindow(1);
            if (addUserWindow.ShowDialog() == false)
                return;
            LoadDirectors(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            DirectorClass.GetDirectorData(deleteBtn.Tag.ToString());
            string idUser = DirectorClass.dtDirectorDataList.Rows[0]["idUser"].ToString();
            if (!DirectorClass.DeleteDirector(deleteBtn.Tag.ToString()) || !UserClass.DeleteUser(idUser))
                return;
            LoadDirectors(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            DirectorClass.GetDirectorData(changeBtn.Tag.ToString());
            string idUser = DirectorClass.dtDirectorDataList.Rows[0]["idUser"].ToString();
            string login = DirectorClass.dtDirectorDataList.Rows[0]["login"].ToString();
            string surname = DirectorClass.dtDirectorDataList.Rows[0]["surname"].ToString();
            string name = DirectorClass.dtDirectorDataList.Rows[0]["name"].ToString();
            string middleName = DirectorClass.dtDirectorDataList.Rows[0]["middleName"].ToString();
            string phoneNumber = DirectorClass.dtDirectorDataList.Rows[0]["phoneNumber"].ToString();
            string email = DirectorClass.dtDirectorDataList.Rows[0]["email"].ToString();
            AddUserWindow addUserWindow = new AddUserWindow(changeBtn.Tag.ToString(), login, surname, name, middleName, phoneNumber, email, "", idUser, "", 1);
            if (addUserWindow.ShowDialog() == false)
                return;
            LoadDirectors(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDirectors(querySearch);
            CountRecords();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDirectors(querySearch);
            CountRecords();
        }

        private void LoadDirectors(string querySearch)
        {
            DirectorClass.GetDirectorsList(querySearch, SortValue());
            directorsGrid.ItemsSource = DirectorClass.dtDirectorsList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = DirectorClass.GetCountAllDirecrors();
            string countResRecords = DirectorClass.dtDirectorsList.Rows.Count.ToString();
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
                    sortValue = "directors.surname";
                    break;
                case 1: 
                    sortValue = "directors.name";
                    break;
                case 2: 
                    sortValue = "directors.middleName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F7)
            {
                ShowHelp();
                e.Handled = true;
            }
        }

        private void ShowHelp()
        {
            ReferenceInformationWindow helpWindow = new ReferenceInformationWindow("DirectorDirsPage");
            helpWindow.ShowDialog();
        }
    }
}
