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
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using System.IO;
using Microsoft.Win32;
using TyEmuNuzhen.Views.Windows;
using System.Security.Cryptography;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime
{
    /// <summary>
    /// Логика взаимодействия для NanniesChildrenPage.xaml
    /// </summary>
    public partial class NanniesChildrenPage : Page
    {
        private string _idActualProgram;
        private string _idChild;
        private string _idNanny;
        private string _idNannyOnProgram;

        private bool _haveActiveNannyOnProgram = true;
        private bool _changedStatus = false;

        public NanniesChildrenPage(string idChild, string fioChild)
        {
            InitializeComponent();
            fullNameChild.Text = fioChild;
            _idChild = idChild;
            LoadNannies();
        }

        private void btnAddNanny_Click(object sender, RoutedEventArgs e)
        {
            AddNannyOnProgramWindow addNannyOnProgramWindow = new AddNannyOnProgramWindow(_idActualProgram, _idChild);
            if (addNannyOnProgramWindow.ShowDialog() == true)
            {
                if (_haveActiveNannyOnProgram == true)
                {
                    if (!CreateDocumentsClass.CreateActOfCompleetedWorksNanny(_idNannyOnProgram, _idNanny, _idChild))
                        return;
                    if (!NanniesOnProgramClass.UpdateStatusNannyOnProgram(_idNannyOnProgram))
                        return;
                }
                LoadNannies();
                ChildrensClass.GetChildrenListByID(_idChild);
                string idStatusProgram = ChildrensClass.dtChildrensDetailedList.Rows[0]["idStatusProgram"].ToString();
                if (idStatusProgram == "2")
                {
                    if (!ChildrensClass.UpdateStatusProgramChildren(_idChild, "3"))
                        return;
                    MessageBox.Show("Статус ребёнка изменён на \"Ожидает госпитализации\".", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    _changedStatus = true;
                }
            }
        }

        private void LoadNannies()
        {
            agreementPanel.Children.Clear();
            _idActualProgram = ActualProgramClass.GetIDLastActualProgramChildren(_idChild);
            NanniesOnProgramClass.GetActiveNannyOnProgramData(_idActualProgram);
            if (NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows.Count == 0)
            {
                noRecord.Visibility = Visibility.Visible;
                nannyData.Visibility = Visibility.Collapsed;
                _haveActiveNannyOnProgram = false;
                return;
            }
            noRecord.Visibility = Visibility.Collapsed;
            nannyData.Visibility = Visibility.Visible;
            _haveActiveNannyOnProgram = true;
            btnAddNanny.Content = "Изменить няню";
            _idNannyOnProgram = NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["ID"].ToString();
            _idNanny = NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["idNanny"].ToString();
            FIOTxt.Text = "ФИО: " + NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["fullName"].ToString();
            phoneTxt.Text = "Номер телефона: " +  NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["phoneNumber"].ToString();
            emailTxt.Text = "Email: " + NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["email"].ToString();
            dateBeginTxt.Text = "Дата заключения договора: " + Convert.ToDateTime(NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["dateConclusion"]).ToString("dd.MM.yyyy");
            costPerDayTxt.Text = "Стоимость в сутки: " + NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["costPerDay"].ToString();
            string filePath = NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["filePath"].ToString();
            string dateBegin = Convert.ToDateTime(NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows[0]["dateConclusion"]).ToString("dd.MM.yyyy");
            ImageUserControl agreementActiveNanny = new ImageUserControl(3, false, filePath, dateBegin, "");
            agreementPanel.Children.Add(agreementActiveNanny);

            NanniesOnProgramClass.GetHistoryNannyOnProgramData(_idActualProgram);
            nanniesGrid.ItemsSource = NanniesOnProgramClass.dtHistoryNannyOnProgramData.DefaultView;
        }

        private void downloadAgreementBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadBtn = sender as Button;
            string originalFileName = Path.GetFileName(downloadBtn.Tag.ToString());
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = "Документы Word(*.docx) | *.docx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(downloadBtn.Tag.ToString(), selectedPath);
            }
        }

        private void downloadActOfCompletedWorksBtn_Click(object sender, RoutedEventArgs e)
        {
            var downloadBtn = sender as Button;
            string originalFileName = Path.GetFileName(downloadBtn.Tag.ToString());
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = "Документы Word(*.docx) | *.docx"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(downloadBtn.Tag.ToString(), selectedPath);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_changedStatus == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new ChildrensPage(2));
            NavigationService.RemoveBackEntry();
        }
    }
}
