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

        public AddTransferWindow(string idHospitalization, bool isSide0)
        {
            InitializeComponent();
            _idHospitalization = idHospitalization;
            _side = isSide0 ? "1" : "0";
        }

        public AddTransferWindow(string idTransfer)
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
    }
}
