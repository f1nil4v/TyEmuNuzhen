using System.Windows.Controls;
using System.Windows;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;
using Microsoft.Win32;
using System.IO;

namespace TyEmuNuzhen.Views.Pages.Director.Orphanages
{
    public partial class OrphanagesPage : Page
    {
        public OrphanagesPage()
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
            LoadOrphanages("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            AddOrphanage addOrphanage = new AddOrphanage();
            if (addOrphanage.ShowDialog() != true)
                return;
            LoadOrphanages(querySearch);
            CountRecords();
        }

        private void downloadArgreementOrphanageBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var downloadBtn = sender as Button;
            NavigationService.Navigate(new AgreementsPage(downloadBtn.Tag.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var changeBtn = sender as Button;
            string idOrphanage = changeBtn.Tag.ToString();
            OrphanageClass.GetOrphanageData(idOrphanage);
            string nameOrphanage = OrphanageClass.dtOrphanageDataList.Rows[0]["nameOrphanage"].ToString();
            string directorSurname = OrphanageClass.dtOrphanageDataList.Rows[0]["directorSurname"].ToString();
            string directorName = OrphanageClass.dtOrphanageDataList.Rows[0]["directorName"].ToString();
            string directorMiddleName = OrphanageClass.dtOrphanageDataList.Rows[0]["middleName"].ToString();
            string idRegion = OrphanageClass.dtOrphanageDataList.Rows[0]["idRegion"].ToString();
            string address = OrphanageClass.dtOrphanageDataList.Rows[0]["address"].ToString();
            string email = OrphanageClass.dtOrphanageDataList.Rows[0]["email"].ToString();
            AddOrphanage addOrphanage = new AddOrphanage(idOrphanage, nameOrphanage, directorSurname, directorName, directorMiddleName, idRegion, address, email);
            if (addOrphanage.ShowDialog() != true)
                return;
            LoadOrphanages(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
                return;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            string idOrphanage = deleteBtn.Tag.ToString();
            AgreementOrphanagesClass.GetAgreementOrphanageData(idOrphanage);
            string filePath = AgreementOrphanagesClass.dtAgreementOrphanageData.Rows[0]["filePath"].ToString();
            if (!OrphanageClass.DeleteOrphanage(deleteBtn.Tag.ToString()) || !CopyFilesClass.DeleteFile(filePath))
                return;
            LoadOrphanages(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadOrphanages(querySearch);
            CountRecords();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadOrphanages(querySearch);
            CountRecords();
        }

        private void regionsCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadOrphanages(querySearch);
            CountRecords();
        }

        private void LoadOrphanages(string querySearch)
        {
            string idRegion = regionsCmbBox.SelectedValue == null ? null : regionsCmbBox.SelectedValue.ToString();
            OrphanageClass.GetOrphanagesList(querySearch, idRegion, SortValue());
            orphanagesGrid.ItemsSource = OrphanageClass.dtOrphanagesList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = OrphanageClass.GetCountAllOrphanages();
            string countResRecords = OrphanageClass.dtOrphanagesList.Rows.Count.ToString();
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
                    sortValue = "orphanages.nameOrphanage";
                    break;
                case 1: 
                    sortValue = "orphanages.directorSurname";
                    break;
                case 2: 
                    sortValue = "orphanages.directorName";
                    break;
                case 3: 
                    sortValue = "orphanages.directorMiddelName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }
    }
}