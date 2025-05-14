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
using System.Windows.Shapes;

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ReferenceBookValuesWindow.xaml
    /// </summary>
    public partial class ReferenceBookValuesWindow : Window
    {
        public ReferenceBookValuesWindow()
        {
            InitializeComponent();
            this.Title = "Добавление записи";
        }

        public ReferenceBookValuesWindow(string value)
        {
            InitializeComponent();
            this.Title = "Редактирование записи";
            tbValue.Text = value;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbValue.Text))
            {
                MessageBox.Show("Пожалуйста, заполните поле", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }
    }
}
