using System;
using System.Windows;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.InWork
{
    /// <summary>
    /// Логика взаимодействия для DocumentTypeWindow.xaml
    /// </summary>
    public partial class DocumentTypeWindow : Window
    {

        public DocumentTypeWindow()
        {
            InitializeComponent();
            LoadDocumentTypes();
        }

        private void LoadDocumentTypes()
        {
            DocumentTypeClass.GetDocumentTypes();
            cbDocumentType.ItemsSource = DocumentTypeClass.dtDocumentTypes.DefaultView;
            if (DocumentTypeClass.dtDocumentTypes.Rows.Count > 0)
                cbDocumentType.SelectedIndex = 0;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (cbDocumentType.SelectedValue != null)
            {
                DocumentTypeClass.selectedIDTypeDocument = cbDocumentType.SelectedValue.ToString();
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите тип документа", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
} 