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

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Nannies
{
    /// <summary>
    /// Логика взаимодействия для NanniesPage.xaml
    /// </summary>
    public partial class NanniesPage : Page
    {
        public NanniesPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNannies("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            AddNannyWindow addNannyWindow = new AddNannyWindow();
            if (!addNannyWindow.ShowDialog() == true)
                return;
            LoadNannies(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!NanniesClass.DeleteNanny(deleteBtn.Tag.ToString()))
                return;
            LoadNannies(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            NanniesClass.GetNannyData(changeBtn.Tag.ToString());
            string surname = NanniesClass.dtNanniesDataList.Rows[0]["surname"].ToString();
            string name = NanniesClass.dtNanniesDataList.Rows[0]["name"].ToString();
            string middleName = NanniesClass.dtNanniesDataList.Rows[0]["middleName"].ToString();
            string passSeries = NanniesClass.dtNanniesDataList.Rows[0]["passSeries"].ToString();
            string passNum = NanniesClass.dtNanniesDataList.Rows[0]["passNum"].ToString();
            string passDateOfIssue = NanniesClass.dtNanniesDataList.Rows[0]["passDateOfIssue"].ToString();
            string passOrganizationOfIssue = NanniesClass.dtNanniesDataList.Rows[0]["passOrganizationOfIssue"].ToString();
            string passCode = NanniesClass.dtNanniesDataList.Rows[0]["passCode"].ToString();
            string addressRegister = NanniesClass.dtNanniesDataList.Rows[0]["addressRegister"].ToString();
            string phoneNumber = NanniesClass.dtNanniesDataList.Rows[0]["phoneNumber"].ToString();
            string email = NanniesClass.dtNanniesDataList.Rows[0]["email"].ToString();
            AddNannyWindow addNannyWindow = new AddNannyWindow(changeBtn.Tag.ToString(), surname, name, middleName, passSeries, passNum, passDateOfIssue, passOrganizationOfIssue, passCode, addressRegister, phoneNumber, email);
            if (!addNannyWindow.ShowDialog() == true)
                return;
            LoadNannies(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadNannies(querySearch);
            CountRecords();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadNannies(querySearch);
            CountRecords();
        }

        private void LoadNannies(string querySearch)
        {
            NanniesClass.GetNanniesList(querySearch, SortValue());
            nanniesGrid.ItemsSource = NanniesClass.dtNanniesList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = NanniesClass.GetCountAllNannies();
            string countResRecords = NanniesClass.dtNanniesList.Rows.Count.ToString();
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
                    sortValue = "nannies.surname";
                    break;
                case 1:
                    sortValue = "nannies.name";
                    break;
                case 2:
                    sortValue = "nannies.middleName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }
    }
}
