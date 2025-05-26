using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows.DialogWindows.ReferenceBooks
{
    /// <summary>
    /// Логика взаимодействия для DocumentsTypeWindow.xaml
    /// </summary>
    public partial class DocumentsTypeWindow : Window
    {
        private string _id;
        private bool _isInsert = true;

        public DocumentsTypeWindow()
        {
            InitializeComponent();
        }

        public DocumentsTypeWindow(string id)
        {
            InitializeComponent();
            this.Title = "Редактирование типа документа";
            _isInsert = false;
            _id = id;
            tbValue.Text = DocumentTypeClass.GetDocumentTypeName(id);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbValue.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_isInsert)
            {
                if (!DocumentTypeClass.AddDocumentType(tbValue.Text))
                    return;
            }
            else
            {
                if (!DocumentTypeClass.UpdateDocumentType(_id, tbValue.Text))
                    return;
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
