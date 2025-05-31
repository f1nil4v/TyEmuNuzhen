using System;
using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;
using TyEmuNuzhen.Views.Windows.DialogWindows.ReferenceBooks;

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
            HelpManagerClass.CurrentHelpKey = "DirectorReferPostsPage";
            LoadDoctorPosts("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            DoctorPostsWindow doctorPostsWindow = new DoctorPostsWindow();
            if (doctorPostsWindow.ShowDialog() == false)
                return;
            LoadDoctorPosts(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
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
            DoctorPostsWindow doctorPostsWindow = new DoctorPostsWindow(changeBtn.Tag.ToString());
            if (doctorPostsWindow.ShowDialog() == false)
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
