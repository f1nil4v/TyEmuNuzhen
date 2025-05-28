using System;
using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;
using TyEmuNuzhen.Views.Windows.DialogWindows.ReferenceBooks;

namespace TyEmuNuzhen.Views.Pages.Director.Reference_Books
{
    /// <summary>
    /// Логика взаимодействия для RegionsPage.xaml
    /// </summary>
    public partial class RegionsPage : Page
    {
        public RegionsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRegions("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            RegionsWindow regionsWindow = new RegionsWindow();
            if (regionsWindow.ShowDialog() == false)
                return;
            LoadRegions(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!RegionsClass.DeleteRegion(deleteBtn.Tag.ToString()))
                return;
            LoadRegions(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            RegionsWindow regionsWindow = new RegionsWindow(changeBtn.Tag.ToString());
            if (regionsWindow.ShowDialog() == false)
                return;
            LoadRegions(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadRegions(querySearch);
            CountRecords();
        }

        private void LoadRegions(string querySearch)
        {
            RegionsClass.GetRegionsList(querySearch);
            regionsGrid.ItemsSource = RegionsClass.dtRegionsS.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = RegionsClass.GetCountAllRegions();
            string countResRecords = RegionsClass.dtRegionsS.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
