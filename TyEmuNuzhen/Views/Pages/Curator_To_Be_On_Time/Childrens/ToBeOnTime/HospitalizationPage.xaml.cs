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
using System.Windows.Navigation;
using System.IO;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using TyEmuNuzhen.Views.Windows;
using TyEmuNuzhen.Views.Windows.DialogWindows;
using Microsoft.Win32;
using System.Data;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime
{
    /// <summary>
    /// Логика взаимодействия для HospitalizationPage.xaml
    /// </summary>
    public partial class HospitalizationPage : Page
    {
        private string _idChild;
        private string _idActualProgram;
        private string _idHospitalization;

        private string _dateHospitalization;
        private string _dateDischarge;

        private string _idMedicalFacility;
        private string _filePathMedicalDirection;
        private string _idTransferToMedical;
        private string _idTransferFromMedical;

        private DateTime _dateDHospitalization;
        private DateTime? _dateDDischarge;

        private DateTime? _dateDepartureAtHospital;
        private DateTime? _dateArrivalAtHospital;

        private DateTime? _dateDepartureAtDDI;
        private DateTime? _dateArrivalAtDDI;

        private bool _haveTicketsToMedical = false;
        private bool _haveTicketsFromMedical = false;

        private bool _changedStatus = false;

        public bool AreActionsEnabled { get; set; } = true;

        public HospitalizationPage(string idHospitalization, string idChild, string fioChild)
        {
            InitializeComponent();
            _idChild = idChild;
            fullNameChild.Text = fioChild;
            _idHospitalization = idHospitalization;
            HelpManagerClass.CurrentHelpKey = "CuratorHospitalizationPage";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadPage();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_changedStatus == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new ChildrensPage(2));
            NavigationService.RemoveBackEntry();
            HelpManagerClass.CurrentHelpKey = "CuratorToBeOnTimePage";
        }

        private void btnAddHospitalization_Click(object sender, RoutedEventArgs e)
        {
            AddHospitalizationWindow addHospitalizationWindow = new AddHospitalizationWindow(_idActualProgram, _dateDDischarge, _dateArrivalAtHospital, _dateDepartureAtDDI);
            if (addHospitalizationWindow.ShowDialog() == true)
            {
                LoadHospitalization();
                ChildrensClass.GetChildrenListByID(_idChild);
                string idStatusProgram = ChildrensClass.dtChildrensDetailedList.Rows[0]["idStatusProgram"].ToString();
                if (idStatusProgram == "3")
                {
                    if (!ChildrensClass.UpdateStatusProgramChildren(_idChild, "4"))
                        return;
                    MessageBox.Show("Статус ребёнка изменён на \"Госпитализация\".", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    _changedStatus = true;
                }
            }
        }

        private void btnEditHospitalization_Click(object sender, RoutedEventArgs e)
        {
            AddHospitalizationWindow addHospitalizationWindow = new AddHospitalizationWindow(_idActualProgram, _idHospitalization, _dateHospitalization, _dateDischarge, _idMedicalFacility, _filePathMedicalDirection,
                _dateDDischarge, _dateArrivalAtHospital, _dateDepartureAtDDI);
            if (addHospitalizationWindow.ShowDialog() == true)
                LoadHospitalization();
        }

        private void btnAddDetailHospitalization_Click(object sender, RoutedEventArgs e)
        {
            AddHospitalizationDetailWindow addHospitalizationDetail = new AddHospitalizationDetailWindow(_idHospitalization, _dateDHospitalization, _dateDDischarge);
            if (addHospitalizationDetail.ShowDialog() == true)
                LoadHospitalization();
        }

        private void changeDetailHospitalizationBtn_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = (Button)sender;
            string id = changeBtn.Tag.ToString();
            AddHospitalizationDetailWindow addHospitalizationDetail = new AddHospitalizationDetailWindow(id, _idHospitalization, _dateDHospitalization, _dateDDischarge);
            if (addHospitalizationDetail.ShowDialog() == true)
                LoadHospitalization();
        }

        private void deleteDetailHospitalizationBtn_Click(object sender, RoutedEventArgs e)
        {
            var deleteBtn = (Button)sender;
            string id = deleteBtn.Tag.ToString();
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            if (!HospitalizationDetailClass.DeleteHospitalizationDetail(id))
                return;
            LoadHospitalization();
        }

        private void btnEndHospitalization_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
             "Вы уверены, что хотите завершить госпитализацию?\n\n",
             "Завершение госпитализации",
             MessageBoxButton.OKCancel,
             MessageBoxImage.Question);

            if (result == MessageBoxResult.Cancel)
                return;
            ChildrensClass.GetChildrenListByID(_idChild);
            string idStatusProgram = ChildrensClass.dtChildrensDetailedList.Rows[0]["idStatusProgram"].ToString();
            if (idStatusProgram == "4")
            {
                if (!ChildrensClass.UpdateStatusProgramChildren(_idChild, "5"))
                    return;
                MessageBox.Show("Статус ребёнка изменён на \"Программа пройдена\".", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                _changedStatus = true;
                ReloadPage();
            }
        }

        private void btnAddTransferToMedical_Click(object sender, RoutedEventArgs e)
        {
            AddTransferWindow addTransferWindow = null;
            if (btnAddTransferToMedical.Tag == null)
                addTransferWindow = new AddTransferWindow(_idActualProgram, _idHospitalization, true, _dateDHospitalization, _dateDDischarge,
                    _dateDepartureAtHospital, _dateArrivalAtHospital, _dateDepartureAtDDI, _dateArrivalAtDDI);
            else
                addTransferWindow = new AddTransferWindow(_idActualProgram, btnAddTransferToMedical.Tag.ToString(), true, _dateDHospitalization, _dateDDischarge,
                    _dateDepartureAtHospital, _dateArrivalAtHospital, _dateDepartureAtDDI, _dateArrivalAtDDI, 'c');
            if (addTransferWindow.ShowDialog() == true)
                LoadTransferToMedical();
        }

        private void btnAddTransferToMedicalTicket_Click(object sender, RoutedEventArgs e)
        {
            AddTicketWindow addTicketWindow = new AddTicketWindow(_idTransferToMedical);
            if (addTicketWindow.ShowDialog() == true)
                LoadTransferToMedical();
        }

        private void changeTicketSide0Btn_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = (Button)sender;
            string id = changeBtn.Tag.ToString();
            AddTicketWindow addTicketWindow = new AddTicketWindow(id, _idTransferToMedical);
            if (addTicketWindow.ShowDialog() == true)
                LoadTransferToMedical();
        }

        private void deleteTicketSide0Btn_Click(object sender, RoutedEventArgs e)
        {
            var deleteBtn = (Button)sender;
            string id = deleteBtn.Tag.ToString();
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            if (!TransferDetailClass.DeleteTransferDetail(id))
                return;
            LoadTransferToMedical();
        }

        private void btnAddTransferFromMedical_Click(object sender, RoutedEventArgs e)
        {
            AddTransferWindow addTransferWindow = null;
            if (btnAddTransferFromMedical.Tag == null)
                addTransferWindow = new AddTransferWindow(_idActualProgram, _idHospitalization, false, _dateDHospitalization, _dateDDischarge,
                    _dateDepartureAtHospital, _dateArrivalAtHospital, _dateDepartureAtDDI, _dateArrivalAtDDI);
            else
                addTransferWindow = new AddTransferWindow(_idActualProgram, btnAddTransferFromMedical.Tag.ToString(), false, _dateDHospitalization, _dateDDischarge,
                    _dateDepartureAtHospital, _dateArrivalAtHospital, _dateDepartureAtDDI, _dateArrivalAtDDI, 'c');
            if (addTransferWindow.ShowDialog() == true)
                LoadTransferFromMedical();
        }

        private void btnAddTransferFromMedicalTicket_Click(object sender, RoutedEventArgs e)
        {
            AddTicketWindow addTicketWindow = new AddTicketWindow(_idTransferFromMedical);
            if (addTicketWindow.ShowDialog() == true)
                LoadTransferFromMedical();
        }

        private void changeTicketSide1Btn_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = (Button)sender;
            string id = changeBtn.Tag.ToString();
            AddTicketWindow addTicketWindow = new AddTicketWindow(id, _idTransferFromMedical);
            if (addTicketWindow.ShowDialog() == true)
                LoadTransferFromMedical();
        }

        private void deleteTicketSide1Btn_Click(object sender, RoutedEventArgs e)
        {
            var deleteBtn = (Button)sender;
            string id = deleteBtn.Tag.ToString();
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            if (!TransferDetailClass.DeleteTransferDetail(id))
                return;
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

        private void ReloadPage()
        {
            _idActualProgram = ActualProgramClass.GetIDLastActualProgramChildren(_idChild);
            LoadHospitalization();
            LoadTransferToMedical();
            LoadTransferFromMedical();
            if (_haveTicketsFromMedical == false || _haveTicketsToMedical == false || HospitalizationClass.dtHospitalizationData.Rows.Count == 0)
                return;
            if (DateTime.Now < _dateArrivalAtDDI)
                return;
            ChildrensClass.GetChildrenListByID(_idChild);
            string idStatusProgram = ChildrensClass.dtChildrensDetailedList.Rows[0]["idStatusProgram"].ToString();
            if (idStatusProgram != "4")
            {
                btnEditHospitalization.IsEnabled = false;
                btnEndHospitalization.IsEnabled = false;
                btnAddDetailHospitalization.IsEnabled = false;
                AreActionsEnabled = false;
                btnAddTransferFromMedical.IsEnabled = false;
                btnAddTransferToMedical.IsEnabled = false;
                btnAddTransferFromMedicalTicket.IsEnabled = false;
                btnAddTransferToMedicalTicket.IsEnabled = false;
                return;
            }
            btnEndHospitalization.IsEnabled = true;
        }

        private void LoadHospitalization()
        {
            HospitalizationClass.GetHospitalizationData(_idHospitalization, _idActualProgram);
            if (HospitalizationClass.dtHospitalizationData.Rows.Count == 0)
            {
                noRecord.Visibility = Visibility.Visible;
                hospitalizationDetailedGrid.IsEnabled = false;
                transferMainGrid.IsEnabled = false;
                btnAddHospitalization.Visibility = Visibility.Visible;
                btnEditHospitalization.Visibility = Visibility.Collapsed;
                btnEndHospitalization.Visibility = Visibility.Collapsed;
                return;
            }
            noRecord.Visibility = Visibility.Collapsed;
            btnAddHospitalization.Visibility = Visibility.Collapsed;
            btnEditHospitalization.Visibility = Visibility.Visible;
            btnEndHospitalization.Visibility = Visibility.Visible;
            hospitalizationDetailedGrid.IsEnabled = true;
            transferMainGrid.IsEnabled = true;
            if (_idHospitalization == "")
                _idHospitalization = HospitalizationClass.dtHospitalizationData.Rows[0]["ID"].ToString();
            medicalFacilityTxt.Text = "Медицинское учреждение: " + HospitalizationClass.dtHospitalizationData.Rows[0]["medicalFacility"].ToString();
            _dateDHospitalization = Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateHospitalization"]);
            dateHospitalizationTxt.Text = "Дата госпитализации: " + _dateDHospitalization.ToString("dd.MM.yyyy");
            _dateDDischarge = HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"]);
            string dateDischarge = _dateDDischarge == null ? "Неопределено" : _dateDDischarge.Value.ToString("dd.MM.yyyy");
            dateDischargeTxt.Text = "Дата выписки: " + dateDischarge;
            totalCostTxt.Text = "Стоимость: " + HospitalizationClass.dtHospitalizationData.Rows[0]["totalCost"].ToString() + " ₽";
            string filePathMedicalDirection = HospitalizationClass.dtHospitalizationData.Rows[0]["filePath"].ToString();
            medicalDirection.Children.Clear();
            ImageUserControl hospitalizationMedicalDirectionUserControl = new ImageUserControl(4, true, filePathMedicalDirection, _dateHospitalization, _dateDischarge);
            medicalDirection.Children.Add(hospitalizationMedicalDirectionUserControl);
            _dateHospitalization = Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateHospitalization"]).ToString("dd.MM.yyyy");
            _dateDischarge = HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"] == DBNull.Value ? null : Convert.ToDateTime(HospitalizationClass.dtHospitalizationData.Rows[0]["dateDischarge"]).ToString("dd.MM.yyyy");
            _filePathMedicalDirection = filePathMedicalDirection;
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
            if (TransferClass.dtTransferOnHozpitalizationSide1Data.Rows.Count == 0)
            {
                btnAddTransferToMedicalTicket.IsEnabled = false;
                return;
            }
            btnAddTransferToMedicalTicket.IsEnabled = true;
            btnAddTransferToMedical.Content = "Редиктировать трансфер";
            btnAddTransferToMedical.Tag = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["ID"].ToString();
            _idTransferToMedical = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["ID"].ToString();
            _dateDepartureAtHospital = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["dateDeparture"]);
            _dateArrivalAtHospital = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["dateArrival"]);
            dateDepartureSide1.Text = "Дата и время отправления: " + _dateDepartureAtHospital.Value.ToString("dd.MM.yyyy HH:mm");
            dateArrivalSide1.Text = "Дата и время прибытия: " + _dateArrivalAtHospital.Value.ToString("dd.MM.yyyy HH:mm");
            ToMedicalFacilityTotalCost.Text = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["totalCost"].ToString().Replace(",00", "") + " ₽";
            LoadTranserToMedicalDetails();
        }

        private void LoadTranserToMedicalDetails()
        {
            TransferDetailClass.GetTransferDetailsData(_idTransferToMedical, true);
            transferSide1Grid.ItemsSource = TransferDetailClass.dtTransferDetailedSide1Data.DefaultView;
            if (TransferDetailClass.dtTransferDetailedSide1Data.Rows.Count > 0)
                _haveTicketsToMedical = true;
        }

        private void LoadTransferFromMedical()
        {
            TransferClass.GetTransferOnHozpitalizationSide0Data(_idHospitalization);
            if (TransferClass.dtTransferOnHozpitalizationSide0Data.Rows.Count == 0)
            {
                btnAddTransferFromMedicalTicket.IsEnabled = false;
                return;
            }
            btnAddTransferFromMedicalTicket.IsEnabled = true;
            btnAddTransferFromMedical.Content = "Редиктировать трансфер";
            btnAddTransferFromMedical.Tag = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["ID"].ToString();
            _idTransferFromMedical = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["ID"].ToString();
            _dateDepartureAtDDI = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["dateDeparture"]);
            _dateArrivalAtDDI = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["dateArrival"]);
            dateDepartureSide0.Text = "Дата и время отправления: " + _dateDepartureAtDDI.Value.ToString("dd.MM.yyyy HH:mm");
            dateArrivalSide0.Text = "Дата и время прибытия: " + _dateArrivalAtDDI.Value.ToString("dd.MM.yyyy HH:mm");
            FromMedicalFacilityTotalCost.Text = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["totalCost"].ToString().Replace(",00", "") + " ₽";
            LoadTranserFromMedicalDetails();
        }

        private void LoadTranserFromMedicalDetails()
        {
            TransferDetailClass.GetTransferDetailsData(_idTransferFromMedical, false);
            transferSide0Grid.ItemsSource = TransferDetailClass.dtTransferDetailedSide0Data.DefaultView;
            if (TransferDetailClass.dtTransferDetailedSide0Data.Rows.Count > 0)
                _haveTicketsFromMedical = true;
        }

    }
}
