using System;
using System.IO;
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
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using TyEmuNuzhen.Views.Windows.DialogWindows;
using TyEmuNuzhen.Views.Windows;
using Microsoft.Win32;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks.HistoryProgram
{
    /// <summary>
    /// Логика взаимодействия для HospitalizationPage.xaml
    /// </summary>
    public partial class HospitalizationPage : Page
    {
        private string _idActualProgram;
        private string _idHospitalization;

        private string _dateHospitalization;
        private string _dateDischarge;
        private string _idTransferToMedical;
        private string _idTransferFromMedical;


        public HospitalizationPage(string idActualProgram)
        {
            InitializeComponent();
            _idActualProgram = idActualProgram;
            HelpManagerClass.CurrentHelpKey = "HistoryHospitalizationPage";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            HospitalizationClass.GetPeriodsHospitalizationList(_idActualProgram);
            periodHospitalizationCmbBox.ItemsSource = HospitalizationClass.dtPeriodsHospitalizationList?.DefaultView;
            periodHospitalizationCmbBox.DisplayMemberPath = "periodHospitalization";
            periodHospitalizationCmbBox.SelectedValuePath = "ID";
            periodHospitalizationCmbBox.SelectedIndex = 0;
            LoadHospitalization();
            LoadTransferToMedical();
            LoadTransferFromMedical();
        }

        private void periodHospitalizationCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadHospitalization();
            LoadTransferToMedical();
            LoadTransferFromMedical();
        }

        private void downloadTicketBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadBtn = sender as Button;
            string originalFileName = Path.GetFileName(downloadBtn.Tag.ToString());
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(downloadBtn.Tag.ToString(), selectedPath);
            }
        }

        private void LoadHospitalization()
        {
            _idHospitalization = periodHospitalizationCmbBox.SelectedValue.ToString();
            HospitalizationClass.GetHospitalizationData(_idHospitalization, _idActualProgram);
            medicalFacilityTxt.Text = "Медицинское учреждение: " + HospitalizationClass.dtHospitalizationData.Rows[0]["medicalFacility"].ToString();
            dateHospitalizationTxt.Text = "Дата госпитализации: " + Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateHospitalization"]).ToString("dd.MM.yyyy");
            string dateDischarge = HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"] == DBNull.Value ? "Неопределено" : Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"]).ToString("dd.MM.yyyy");
            dateDischargeTxt.Text = "Дата выписки: " + dateDischarge;
            totalCostTxt.Text = "Стоимость: " + HospitalizationClass.dtHospitalizationData.Rows[0]["totalCost"].ToString() + " ₽";
            string filePathMedicalDirection = HospitalizationClass.dtHospitalizationData.Rows[0]["filePath"].ToString();
            medicalDirection.Children.Clear();
            ImageUserControl hospitalizationMedicalDirectionUserControl = new ImageUserControl(4, true, filePathMedicalDirection, _dateHospitalization, _dateDischarge);
            medicalDirection.Children.Add(hospitalizationMedicalDirectionUserControl);
            _dateHospitalization = Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateHospitalization"]).ToString("dd.MM.yyyy");
            _dateDischarge = HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"] == DBNull.Value ? null : Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"]).ToString("dd.MM.yyyy");
            LoadDetailsHospitalization();
        }

        private void LoadDetailsHospitalization()
        {
            HospitalizationDetailClass.GetHospitalizationDetailData(_idHospitalization);
            hospitalizationDetailed.ItemsSource = HospitalizationDetailClass.dtHospitalizationDetailData.DefaultView;
            hospitalizationDetailed.DataContext = this;
        }

        private void LoadTransferToMedical()
        {
            TransferClass.GetTransferOnHozpitalizationSide1Data(_idHospitalization);
            _idTransferToMedical = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["ID"].ToString();
            dateDepartureSide1.Text = "Дата и время отправления: " + Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["dateDeparture"]).ToString("dd.MM.yyyy HH:mm");
            dateArrivalSide1.Text = "Дата и время прибытия: " + Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["dateArrival"]).ToString("dd.MM.yyyy HH:mm");
            ToMedicalFacilityTotalCost.Text = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["totalCost"].ToString().Replace(",00", "") + " ₽";
            LoadTranserToMedicalDetails();
        }

        private void LoadTranserToMedicalDetails()
        {
            TransferDetailClass.GetTransferDetailsData(_idTransferToMedical, true);
            transferSide1Grid.ItemsSource = TransferDetailClass.dtTransferDetailedSide1Data.DefaultView;
        }

        private void LoadTransferFromMedical()
        {
            TransferClass.GetTransferOnHozpitalizationSide0Data(_idHospitalization);
            _idTransferFromMedical = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["ID"].ToString();
            dateDepartureSide0.Text = "Дата и время отправления: " + Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["dateDeparture"]).ToString("dd.MM.yyyy HH:mm");
            dateArrivalSide0.Text = "Дата и время прибытия: " + Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["dateArrival"]).ToString("dd.MM.yyyy HH:mm");
            FromMedicalFacilityTotalCost.Text = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["totalCost"].ToString().Replace(",00", "") + " ₽";
            LoadTranserFromMedicalDetails();
        }

        private void LoadTranserFromMedicalDetails()
        {
            TransferDetailClass.GetTransferDetailsData(_idTransferFromMedical, false);
            transferSide0Grid.ItemsSource = TransferDetailClass.dtTransferDetailedSide0Data.DefaultView;
        }
    }
}
