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

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для AddMedicalFacilityWindow.xaml
    /// </summary>
    public partial class AddMedicalFacilityWindow : Window
    {
        private string _id;
        private bool _isInsert = true;

        public AddMedicalFacilityWindow()
        {
            InitializeComponent();
        }

        public AddMedicalFacilityWindow(string id)
        {
            InitializeComponent();
            this.Title = "Редактирование медицинского учреждения";
            _isInsert = false;
            _id = id;
            MedicalFacilityClass.GetMedicalFacilityData(id);
            tbMedicalFacilityName.Text = MedicalFacilityClass.dtMedicalFacilityData.Rows[0]["medicalFacilityName"].ToString();
            tbAddress.Text = MedicalFacilityClass.dtMedicalFacilityData.Rows[0]["address"].ToString();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbMedicalFacilityName.Text) || String.IsNullOrEmpty(tbAddress.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_isInsert)
            {
                if (!MedicalFacilityClass.GetSameMedicalFacility(tbMedicalFacilityName.Text, tbAddress.Text))
                {
                    return;
                }
                if (!MedicalFacilityClass.AddMedicalFacility(tbMedicalFacilityName.Text, tbAddress.Text))
                    return;
            }
            else
            {
                if (!MedicalFacilityClass.GetSameMedicalFacility(_id, tbMedicalFacilityName.Text, tbAddress.Text))
                {
                    return;
                }
                if (!MedicalFacilityClass.UpdateMedicalFacility(_id, tbMedicalFacilityName.Text, tbAddress.Text))
                    return;
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbMedicalFacilityName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
