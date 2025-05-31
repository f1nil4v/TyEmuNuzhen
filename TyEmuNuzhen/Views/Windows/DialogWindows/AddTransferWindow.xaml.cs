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

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTransferWindow.xaml
    /// </summary>
    public partial class AddTransferWindow : Window
    {
        private string _idTransfer;
        private string _idHospitalization;
        private string _side;
        private bool _isInsert = true;

        private string _idActualProgram;

        private DateTime _dateDHospitalization;
        private DateTime? _dateDDischarge;

        private DateTime? _dateDepartureAtHospital;
        private DateTime? _dateArrivalAtHospital;

        private DateTime? _dateDepartureAtDDI;
        private DateTime? _dateArrivalAtDDI;

        public AddTransferWindow(string idActualProgram, string idHospitalization, bool isSide0, DateTime dateDHospitalization, DateTime? dateDDischarge, 
            DateTime? dateDepartureAtHospital, DateTime? dateArrivalAtHospital, DateTime? dateDepartureAtDDI, DateTime? dateArrivalAtDDI)
        {
            InitializeComponent();
            _idHospitalization = idHospitalization;
            _side = isSide0 ? "1" : "0";
            _idActualProgram = idActualProgram;

            _dateDHospitalization = dateDHospitalization;
            _dateDDischarge = dateDDischarge;
            _dateDepartureAtHospital = dateDepartureAtHospital;
            _dateArrivalAtHospital = dateArrivalAtHospital;
            _dateDepartureAtDDI = dateDepartureAtDDI;
            _dateArrivalAtDDI = dateArrivalAtDDI;
        }

        public AddTransferWindow(string idActualProgram, string idTransfer, bool isSide0, DateTime dateDHospitalization, DateTime? dateDDischarge,
            DateTime? dateDepartureAtHospital, DateTime? dateArrivalAtHospital, DateTime? dateDepartureAtDDI, DateTime? dateArrivalAtDDI, char c)
        {
            InitializeComponent();
            this.Title = "Редактирование трансфера";
            _idTransfer = idTransfer;
            TransferClass.GetTransferOnHozpitalizationData(idTransfer);
            dpDateDeparture.Text = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationData.Rows[0]["dateDeparture"]).ToString("dd.MM.yyyy");
            tpTimeDeparture.Text = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationData.Rows[0]["dateDeparture"]).ToString("HH:mm");
            dpDateArrival.Text = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationData.Rows[0]["dateArrival"]).ToString("dd.MM.yyyy");
            tpTimeArrival.Text = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationData.Rows[0]["dateArrival"]).ToString("HH:mm");
            _isInsert = false;
            _side = isSide0 ? "1" : "0";

            _idActualProgram = idActualProgram;

            _dateDHospitalization = dateDHospitalization;
            _dateDDischarge = dateDDischarge;
            _dateDepartureAtHospital = dateDepartureAtHospital;
            _dateArrivalAtHospital = dateArrivalAtHospital;
            _dateDepartureAtDDI = dateDepartureAtDDI;
            _dateArrivalAtDDI = dateArrivalAtDDI;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_side == "1")
            {
                ActualProgramClass.GetDatesActualProgram(_idActualProgram);
                DateTime dateTransfer = Convert.ToDateTime(ActualProgramClass.dtDatesActualProgram.Rows[0]["dateBegin"]);
                dpDateDeparture.DisplayDateStart = dateTransfer;
                if (_dateArrivalAtHospital == null)
                    dpDateDeparture.DisplayDateEnd = _dateDHospitalization;
                else
                    dpDateDeparture.DisplayDateEnd = _dateArrivalAtHospital;
                if (String.IsNullOrEmpty(dpDateDeparture.Text))
                {
                    dpDateArrival.IsEnabled = false;
                    return;
                }
                dpDateArrival.DisplayDateStart = dpDateDeparture.SelectedDate;
                dpDateArrival.DisplayDateEnd = _dateDHospitalization;
            }
            else
            {
                if (_dateDDischarge != null)
                    dpDateDeparture.DisplayDateStart = _dateDDischarge;
                else
                    dpDateDeparture.DisplayDateStart = _dateDHospitalization;
                if (_dateArrivalAtDDI != null)
                    dpDateDeparture.DisplayDateEnd = _dateArrivalAtDDI;
                if (String.IsNullOrEmpty(dpDateDeparture.Text))
                {
                    dpDateArrival.IsEnabled = false;
                    return;
                }
                dpDateArrival.DisplayDateStart = dpDateDeparture.SelectedDate;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(dpDateDeparture.Text) || String.IsNullOrWhiteSpace(tpTimeDeparture.Text) || String.IsNullOrWhiteSpace(dpDateArrival.Text) || String.IsNullOrWhiteSpace(tpTimeArrival.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string dateDeparture = dpDateDeparture.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + tpTimeDeparture.Text;
            string dateArrival = dpDateArrival.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + tpTimeArrival.Text;
            if (_isInsert)
            {
                if (!TransferClass.AddTransfer(_idHospitalization, dateDeparture, dateArrival, _side))
                {
                    MessageBox.Show("Ошибка при добавлении трансфера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                if (!TransferClass.UpdateTransfer(_idTransfer, dateDeparture, dateArrival))
                {
                    MessageBox.Show("Ошибка при редактировании трансфера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void dpDateDeparture_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpDateArrival.SelectedDate = null;
            if (String.IsNullOrEmpty(dpDateDeparture.Text))
            {
                dpDateArrival.IsEnabled = false;
                return;
            }
            dpDateArrival.DisplayDateStart = dpDateDeparture.SelectedDate;
            dpDateArrival.IsEnabled = true;
            if (_side == "1")
                dpDateArrival.DisplayDateEnd = _dateDHospitalization;
        }

        private void dpDateArrival_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDateArrival.SelectedDate == null)
                return;
            dpDateDeparture.DisplayDateEnd = dpDateArrival.SelectedDate;
        }
    }
}
