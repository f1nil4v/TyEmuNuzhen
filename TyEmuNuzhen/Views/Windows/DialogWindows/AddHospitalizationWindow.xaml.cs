using Microsoft.Win32;
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
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для AddHospitalizationWindow.xaml
    /// </summary>
    public partial class AddHospitalizationWindow : Window
    {
        private string _oldFilePath;
        private string _newFilePath;
        private string _idActualProgram;
        private string _idHospitalization;
        private string _idMedicalFacility;
        private bool _isInsert = true;

        private DateTime? _dateDDischarge;
        private DateTime? _dateArrivalAtHospital;
        private DateTime? _dateDepartureAtDDI;

        public AddHospitalizationWindow(string idActualProgram, DateTime? dateDDischarge, DateTime? dateArrivalAtHospital, DateTime? dateDepartureAtDDI)
        {
            InitializeComponent();
            _idActualProgram = idActualProgram;
            _dateDDischarge = dateDDischarge;
            _dateArrivalAtHospital = dateArrivalAtHospital;
            _dateDepartureAtDDI = dateDepartureAtDDI;
        }

        public AddHospitalizationWindow(string idActualProgram, string idHospitalization, string dateHospitalization, string dateDischarge, string idMedicalFacility, string filePath, 
            DateTime? dateDDischarge, DateTime? dateArrivalAtHospital, DateTime? dateDepartureAtDDI)
        {
            InitializeComponent();
            _idActualProgram = idActualProgram;
            _idHospitalization = idHospitalization;
            dpDateHospitalization.Text = dateHospitalization;
            dpDateDischarge.Text = dateDischarge;
            _idMedicalFacility = idMedicalFacility;
            _oldFilePath = filePath;
            medicalDirection.Children.Clear();
            HospitalizationMedicalDirectionUserControl hospitalizationMedicalDirectionUserControl = new HospitalizationMedicalDirectionUserControl(filePath, 1);
            medicalDirection.Children.Add(hospitalizationMedicalDirectionUserControl);
            this.Title = "Редактирование госпитализации";
            btnMedicalDirection.Content = "Изменить медицинское направление";
            _isInsert = false;
            _dateDDischarge = dateDDischarge;
            _dateArrivalAtHospital = dateArrivalAtHospital;
            _dateDepartureAtDDI = dateDepartureAtDDI;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MedicalFacilityClass.GetMedicalFacilityForComboBoxList();
            medicalFacilityCmbBox.ItemsSource = MedicalFacilityClass.dtMedicalFacilityForComboBoxList.DefaultView;
            medicalFacilityCmbBox.DisplayMemberPath = "medicalFacilityName";
            medicalFacilityCmbBox.SelectedValuePath = "ID";
            if (MedicalFacilityClass.dtMedicalFacilityForComboBoxList.Rows.Count == 0)
            {
                warningMedicalFacility.Visibility = Visibility.Visible;
                btnConfirm.IsEnabled = false;
                return;
            }
            if (String.IsNullOrEmpty(_idMedicalFacility))
                medicalFacilityCmbBox.SelectedIndex = 0;
            else
                medicalFacilityCmbBox.SelectedValue = _idMedicalFacility;

            if (_dateArrivalAtHospital != null)
                dpDateHospitalization.DisplayDateStart = _dateArrivalAtHospital;
            else
            {
                ActualProgramClass.GetDatesActualProgram(_idActualProgram);
                DateTime dateHospitalization = Convert.ToDateTime(ActualProgramClass.dtDatesActualProgram.Rows[0]["dateBegin"]);
                dpDateHospitalization.DisplayDateStart = dateHospitalization;
            }
            if (_dateDDischarge != null)
                dpDateHospitalization.DisplayDateEnd = _dateDDischarge;
            else if (_dateDepartureAtDDI != null)
                dpDateHospitalization.DisplayDateEnd = _dateDepartureAtDDI;
            if (String.IsNullOrEmpty(dpDateHospitalization.Text))
            {
                dpDateDischarge.IsEnabled = false;
                return;
            }
            else
                dpDateDischarge.DisplayDateStart = dpDateHospitalization.SelectedDate.Value.AddDays(1);
            if (_dateDepartureAtDDI != null)
                dpDateDischarge.DisplayDateEnd = _dateDepartureAtDDI;
        }

        private void btnMedicalDirection_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF File (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (_isInsert)
                    _oldFilePath = filePath;
                else
                    _newFilePath = filePath;
                medicalDirection.Children.Clear();
                HospitalizationMedicalDirectionUserControl hospitalizationMedicalDirectionUserControl = new HospitalizationMedicalDirectionUserControl(filePath, 1);
                medicalDirection.Children.Add(hospitalizationMedicalDirectionUserControl);
                btnMedicalDirection.Content = "Изменить медицинское направление";
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string filePath = null;
            string hospitalizationDate = dpDateHospitalization.SelectedDate.Value.ToString("yyyy-MM-dd");
            string dateDischarge = String.IsNullOrEmpty(dpDateDischarge.SelectedDate.ToString()) ? "" : dpDateDischarge.SelectedDate.Value.ToString("yyyy-MM-dd");
            string idMedicalFacility = medicalFacilityCmbBox.SelectedValue.ToString();
            if (String.IsNullOrEmpty(hospitalizationDate) || String.IsNullOrEmpty(dateDischarge))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_isInsert)
            {
                if (String.IsNullOrEmpty(_oldFilePath))
                {
                    MessageBox.Show("Пожалуйста, прикрепите медицинское направление", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                filePath = CopyFilesClass.CopyHospitalizationMedicalDirection(_oldFilePath);
            }
            else
                filePath = String.IsNullOrEmpty(_newFilePath) ? _oldFilePath : CopyFilesClass.CopyHospitalizationMedicalDirection(_newFilePath);

            if (String.IsNullOrEmpty(filePath))
                return;

            if (_isInsert)
            {
                if (!HospitalizationClass.AddHospitalization(idMedicalFacility, _idActualProgram, hospitalizationDate, dateDischarge, filePath))
                {
                    CopyFilesClass.DeleteFile(filePath);
                    return;
                }
            }
            else
            {
                if (!HospitalizationClass.UpdateHospitalization(_idHospitalization, idMedicalFacility, hospitalizationDate, dateDischarge, filePath))
                {
                    if (!String.IsNullOrEmpty(_newFilePath))
                        CopyFilesClass.DeleteFile(filePath);
                }
                if (!String.IsNullOrEmpty(_newFilePath))
                    CopyFilesClass.DeleteFile(_oldFilePath);
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void dpDateHospitalization_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpDateDischarge.SelectedDate = null;
            if (String.IsNullOrEmpty(dpDateHospitalization.Text))
            {
                dpDateDischarge.IsEnabled = false;
                return;
            }
            dpDateDischarge.DisplayDateStart = dpDateHospitalization.SelectedDate.Value.AddDays(1);
            dpDateDischarge.IsEnabled = true;
            if (_dateDepartureAtDDI != null)
                dpDateHospitalization.DisplayDateEnd = _dateDepartureAtDDI;
            else
                dpDateHospitalization.DisplayDateEnd = null;
        }

        private void dpDateDischarge_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateDischarge.SelectedDate == null)
                return;
            dpDateHospitalization.DisplayDateEnd = dpDateDischarge.SelectedDate.Value.AddDays(-1);
        }
    }
}
