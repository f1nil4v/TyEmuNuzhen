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
    /// Логика взаимодействия для DocumentsTypePage.xaml
    /// </summary>
    public partial class DocumentsTypePage : Page
    {
        public DocumentsTypePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDocumentTypes("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow();
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!DocumentTypeClass.GetSameDocumentType(null, referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DocumentTypeClass.AddDocumentType(referenceBookValuesWindow.tbValue.Text))
                return;
            LoadDocumentTypes(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            if (!DocumentTypeClass.DeleteDocumentType(deleteBtn.Tag.ToString()))
                return;
            LoadDocumentTypes(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            string valueDataRow = DocumentTypeClass.GetDocumentTypeName(changeBtn.Tag.ToString());
            ReferenceBookValuesWindow referenceBookValuesWindow = new ReferenceBookValuesWindow(valueDataRow);
            if (!referenceBookValuesWindow.ShowDialog() == true)
                return;
            if (!DocumentTypeClass.GetSameDocumentType(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
            {
                MessageBox.Show("Данная должность уже есть в системе! Введите другое название должности.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DocumentTypeClass.UpdateDocumentType(changeBtn.Tag.ToString(), referenceBookValuesWindow.tbValue.Text))
                return;
            LoadDocumentTypes(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDocumentTypes(querySearch);
            CountRecords();
        }

        private void LoadDocumentTypes(string querySearch)
        {
            DocumentTypeClass.GetDocumentTypesList(querySearch);
            documnetTypesGrid.ItemsSource = DocumentTypeClass.dtDocumentTypesS.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = DocumentTypeClass.GetCountAllDocumentTypes();
            string countResRecords = DocumentTypeClass.dtDocumentTypesS.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
