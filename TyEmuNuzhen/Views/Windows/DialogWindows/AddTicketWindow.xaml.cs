using Microsoft.Win32;
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
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTicketWindow.xaml
    /// </summary>
    public partial class AddTicketWindow : Window
    {
        private string _oldFilePath;
        private string _newFilePath;
        private string _idTransferDetail;
        private string _idTransfer;
        private string _idTransportType;
        private bool _isInsert = true;

        public AddTicketWindow(string idTransfer)
        {
            InitializeComponent();
            _idTransfer = idTransfer;
        }

        public AddTicketWindow(string idTransferDetail, string idTransfer)
        {
            InitializeComponent();
            btnTicket.Content = "Изменить билет";
            _idTransferDetail = idTransferDetail;
            _idTransfer = idTransfer;
            TransferDetailClass.GetTransferDetailData(idTransferDetail);
            _idTransportType = TransferDetailClass.dtTransferDetailData.Rows[0]["idTransportType"].ToString();
            tbCost.Text = TransferDetailClass.dtTransferDetailData.Rows[0]["cost"].ToString().Replace(",00", "");
            _oldFilePath = TransferDetailClass.dtTransferDetailData.Rows[0]["filePath"].ToString();
            ticket.Children.Clear();
            HospitalizationMedicalDirectionUserControl ticketUserControl = new HospitalizationMedicalDirectionUserControl(_oldFilePath, 2);
            ticket.Children.Add(ticketUserControl);
            _isInsert = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TransportTypesClass.GetTranportTypesList("");
            transportTypeCmbBox.ItemsSource = TransportTypesClass.dtTranposrtTypeS.DefaultView;
            transportTypeCmbBox.DisplayMemberPath = "transportType";
            transportTypeCmbBox.SelectedValuePath = "ID";
            if (TransportTypesClass.dtTranposrtTypeS.Rows.Count == 0)
            {
                warningTransportType.Visibility = Visibility.Visible;
                btnConfirm.IsEnabled = false;
                return;
            }
            if (_isInsert)
                transportTypeCmbBox.SelectedIndex = 0;
            else
                transportTypeCmbBox.SelectedValue = _idTransportType;
        }

        private void tbCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                if (_isInsert)
                    _oldFilePath = filePath;
                else
                    _newFilePath = filePath;
                ticket.Children.Clear();
                HospitalizationMedicalDirectionUserControl hospitalizationMedicalDirectionUserControl = new HospitalizationMedicalDirectionUserControl(filePath, 2);
                ticket.Children.Add(hospitalizationMedicalDirectionUserControl);
                btnTicket.Content = "Изменить билет";
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string filePath = null;
            string idTransportType = transportTypeCmbBox.SelectedValue.ToString();
            if (String.IsNullOrEmpty(tbCost.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_isInsert)
            {
                if (String.IsNullOrEmpty(_oldFilePath))
                {
                    MessageBox.Show("Пожалуйста, прикрепите билет", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                filePath = CopyFilesClass.CopyTicket(_oldFilePath);
            }
            else
                filePath = String.IsNullOrEmpty(_newFilePath) ? _oldFilePath : CopyFilesClass.CopyTicket(_newFilePath);

            if (String.IsNullOrEmpty(filePath))
                return;

            if (_isInsert)
            {
                if (!TransferDetailClass.AddTransferDetail(_idTransfer, idTransportType, tbCost.Text, filePath))
                {
                    CopyFilesClass.DeleteFile(filePath);
                    return;
                }
            }
            else
            {
                if (!TransferDetailClass.UpdateTransferDetail(_idTransferDetail, idTransportType, tbCost.Text, filePath))
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
    }
}
