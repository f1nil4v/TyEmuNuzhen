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
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows.DialogWindows.ReferenceBooks
{
    /// <summary>
    /// Логика взаимодействия для DiagnosesWindow.xaml
    /// </summary>
    public partial class DiagnosesWindow : Window
    {
        private string _id;
        private bool _isInsert = true;

        public DiagnosesWindow()
        {
            InitializeComponent();
        }

        public DiagnosesWindow(string id)
        {
            InitializeComponent();
            this.Title = "Редактирование медицинского учреждения";
            _isInsert = false;
            _id = id;
            tbValue.Text = DiagnosesClass.GetDiagnosisName(id);
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
                if (!DiagnosesClass.AddDiagnoses(tbValue.Text))
                    return;
            }
            else
            {
                if (!DiagnosesClass.UpdateDiagnoses(_id, tbValue.Text))
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
