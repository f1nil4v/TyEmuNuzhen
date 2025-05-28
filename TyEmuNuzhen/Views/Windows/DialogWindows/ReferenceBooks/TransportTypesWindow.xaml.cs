using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для TransportTypesWindow.xaml
    /// </summary>
    public partial class TransportTypesWindow : Window
    {
        private string _id;
        private bool _isInsert = true;

        public TransportTypesWindow()
        {
            InitializeComponent();
        }

        public TransportTypesWindow(string id)
        {
            InitializeComponent();
            this.Title = "Редактирование типа транспорта";
            _isInsert = false;
            _id = id;
            tbValue.Text = TransportTypesClass.GetTranposrtTypeName(id);
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
                if (!TransportTypesClass.AddTranposrtType(tbValue.Text))
                    return;
            }
            else
            {
                if (!TransportTypesClass.UpdateTranposrtType(_id, tbValue.Text))
                    return;
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
