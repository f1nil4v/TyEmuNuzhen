using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для DoctorPostsWindow.xaml
    /// </summary>
    public partial class DoctorPostsWindow : Window
    {
        private string _id;
        private bool _isInsert = true;

        public DoctorPostsWindow()
        {
            InitializeComponent();
        }

        public DoctorPostsWindow(string id)
        {
            InitializeComponent();
            this.Title = "Редактирование должности врача";
            _isInsert = false;
            _id = id;
            tbValue.Text = DoctorPostsClass.GetDoctorPostName(id); ;
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
                if (!DoctorPostsClass.AddDoctorPost(tbValue.Text))
                    return;
            }
            else
            {
                if (!DoctorPostsClass.UpdateDoctorPost(_id, tbValue.Text))
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
            Regex regex = new Regex(@"[^0-9]");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
