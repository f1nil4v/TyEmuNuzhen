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

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для AddHospitalizationDetailWindow.xaml
    /// </summary>
    public partial class AddHospitalizationDetailWindow : Window
    {
        private string _idHospitalization;
        private string _idHospitalizationDetail;
        private string _idMedicalTypeHelp;
        private bool _isInsert = true;
        private DateTime _dateDHospitalization;
        private DateTime? _dateDDischarge;

        public AddHospitalizationDetailWindow(string idHospitalization, DateTime dateDHospitalization, DateTime? dateDDischarge)
        {
            InitializeComponent();
            _idHospitalization = idHospitalization;
            _dateDHospitalization = dateDHospitalization;
            _dateDDischarge = dateDDischarge;
        }

        public AddHospitalizationDetailWindow(string idHospitalizationDetail, string idHospitalization, DateTime dateDHospitalization, DateTime? dateDDischarge)
        {
            InitializeComponent();
            _idHospitalizationDetail = idHospitalizationDetail;
            _idHospitalization = idHospitalization;
            HospitalizationDetailClass.GetHospitalizationDetailDataChange(idHospitalizationDetail);
            _idMedicalTypeHelp = HospitalizationDetailClass.dtHospitalizationDetailDataChange.Rows[0]["idTypeMedicalHelp"].ToString();
            tbCost.Text = HospitalizationDetailClass.dtHospitalizationDetailDataChange.Rows[0]["cost"].ToString().Replace(",00","");
            dpDateMedicalHelp.Text = Convert.ToDateTime(HospitalizationDetailClass.dtHospitalizationDetailDataChange.Rows[0]["dateMedicalHelp"]).ToString("dd.MM.yyyy");
            _isInsert = false;
            this.Title = "Редактирование медицинской услуги";   
            _dateDHospitalization = dateDHospitalization;
            _dateDDischarge = dateDDischarge;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MedicalHelpTypeClass.GetMedicalHelpTypesList("");
            medicalCareTypeCmbBox.ItemsSource = MedicalHelpTypeClass.dtMedicalHelpTypeS.DefaultView;
            medicalCareTypeCmbBox.DisplayMemberPath = "medicalCareType";
            medicalCareTypeCmbBox.SelectedValuePath = "ID";
            if (MedicalHelpTypeClass.dtMedicalHelpTypeS.Rows.Count == 0)
            {
                warningMedicalCareType.Visibility = Visibility.Visible;
                btnConfirm.IsEnabled = false;
                return;
            }
            if (_isInsert)
                medicalCareTypeCmbBox.SelectedIndex = 0;
            else
                medicalCareTypeCmbBox.SelectedValue = _idMedicalTypeHelp;
            dpDateMedicalHelp.DisplayDateStart = _dateDHospitalization;
            dpDateMedicalHelp.DisplayDateEnd = _dateDDischarge;
        }

        private void tbCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbCost.Text) || String.IsNullOrWhiteSpace(dpDateMedicalHelp.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string dateMedicalHelp = dpDateMedicalHelp.SelectedDate.Value.ToString("yyyy-MM-dd");
            string medicalCareType = medicalCareTypeCmbBox.SelectedValue.ToString();
            if (_isInsert)
            {
                if (!HospitalizationDetailClass.GetSameHospitalizationDetail(_idHospitalization, medicalCareType, dateMedicalHelp))
                {
                    MessageBox.Show("Запись с такими данными уже существует! Введите другие значения.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!HospitalizationDetailClass.AddHospitalizationDetail(_idHospitalization, medicalCareType, tbCost.Text, dateMedicalHelp))
                    return;
            }
            else
            {
                if (!HospitalizationDetailClass.GetSameHospitalizationDetail(_idHospitalizationDetail, _idHospitalization, medicalCareType, dateMedicalHelp))
                {
                    MessageBox.Show("Запись с такими данными уже существует! Введите другие значения.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!HospitalizationDetailClass.UpdateHospitalizationDetail(_idHospitalizationDetail, medicalCareType, tbCost.Text, dateMedicalHelp))
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
