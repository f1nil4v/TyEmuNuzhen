using System;
using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Director.Reference_Books
{
    /// <summary>
    /// Логика взаимодействия для DoctorPostsPage.xaml
    /// </summary>
    public partial class DoctorPostsPage : Page
    {
        public DoctorPostsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDoctorPosts("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow();
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!DoctorPostsClass.GetSameDoctorPostName(null, referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DoctorPostsClass.AddDoctorPost(referenceBookValuesWindow.tbValue.Text))
                return;
            LoadDoctorPosts(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!DoctorPostsClass.DeleteDoctorPost(deleteBtn.Tag.ToString()))
                return;
            LoadDoctorPosts(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            string valueDataRow = DoctorPostsClass.GetDoctorPostName(changeBtn.Tag.ToString());
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow(valueDataRow);
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!DoctorPostsClass.GetSameDoctorPostName(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DoctorPostsClass.UpdateDoctorPost(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
                return;
            LoadDoctorPosts(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDoctorPosts(querySearch);
            CountRecords();
        }

        private void LoadDoctorPosts(string querySearch)
        {
            DoctorPostsClass.GetDoctorPostsList(querySearch);
            doctorPostsGrid.ItemsSource = DoctorPostsClass.dtDoctorPostsSList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = DoctorPostsClass.GetCountAllDoctorPosts();
            string countResRecords = DoctorPostsClass.dtDoctorPostsSList.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
