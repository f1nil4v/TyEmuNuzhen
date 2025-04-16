using Microsoft.Win32;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.InWork
{
    /// <summary>
    /// Логика взаимодействия для ConsultationPage.xaml
    /// </summary>
    public partial class ConsultationPage : Page
    {
        private string _id; 
        private bool _haveChanges = false;
        private bool _haveMedicalConclusion = false;
        private string _oldPathMedicalConclusion = "";

        public ConsultationPage(string id, string FIOchild)
        {
            InitializeComponent();
            dpConsultationDate.SelectedDate = DateTime.Now;
            _id = id;
            fullNameChild.Text += FIOchild;
        }

        private void imgBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_haveChanges)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти? После выхода с данной страницы изменения будут отменены", "Выход", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    DiagnosesClass.selectedDiagnoses.Clear();
                    DiagnosesClass.selectedIDDiagnoses.Clear();
                    ResultConsultationClass.oldFilePaths.Clear();
                    NavigationService.GoBack();
                    NavigationService.RemoveBackEntry();
                }
            }
            else
            {
                NavigationService.GoBack();
                NavigationService.RemoveBackEntry();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorPostsClass.GetDoctorPostsList();
            cbPost.ItemsSource = DoctorPostsClass.dtDoctorPostsList?.DefaultView;
            cbPost.DisplayMemberPath = "postName";
            cbPost.SelectedValuePath = "ID";
            cbPost.Text = "Нейрохирург";
            DoctorsOnAgreementClass.GetDoctrosForComboBoxList(cbPost.SelectedValue.ToString());
            cbSpecialist.ItemsSource = DoctorsOnAgreementClass.dtDoctorsForComboBoxList?.DefaultView;
            cbSpecialist.DisplayMemberPath = "fullName";
            cbSpecialist.SelectedValuePath = "ID";
            cbSpecialist.SelectedIndex = 1;
        }

        private void cbPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string idPost = cbPost.SelectedValue == null ? null : cbPost.SelectedValue.ToString();
            DoctorsOnAgreementClass.GetDoctrosForComboBoxList(idPost);
            cbSpecialist.ItemsSource = DoctorsOnAgreementClass.dtDoctorsForComboBoxList?.DefaultView;
            cbSpecialist.DisplayMemberPath = "fullName";
            cbSpecialist.SelectedValuePath = "ID";
            cbSpecialist.SelectedIndex = 1;
        }

        private void btnAddResult_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                ResultConsultationClass.oldFilePaths.Add(filePath);
                LoadMedicalResults();
            }
        }

        private void btnAddConclusion_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF File (*.pdf)|*.pdf"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                _oldPathMedicalConclusion = filePath;
                LoadMedicalConclusion(filePath);
            }
        }

        private void btnAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            DiagnosisWindow diagnosisWindow = new DiagnosisWindow(_id);
            if (diagnosisWindow.ShowDialog() == true)
                LoadDiagnoses();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_haveChanges)
            {
                if (MessageBox.Show("Вы уверены, что хотите выйти? После выхода с данной страницы изменения будут отменены", "Выход", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    DiagnosesClass.selectedDiagnoses.Clear();
                    DiagnosesClass.selectedIDDiagnoses.Clear();
                    ResultConsultationClass.oldFilePaths.Clear();
                    NavigationService.GoBack();
                    NavigationService.RemoveBackEntry();
                }
            }
            else
            {
                NavigationService.GoBack();
                NavigationService.RemoveBackEntry();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (DiagnosesClass.selectedDiagnoses.Count == 0 || ResultConsultationClass.oldFilePaths.Count == 0 || _haveMedicalConclusion == false || dpConsultationDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string idDoctor = cbSpecialist.SelectedValue.ToString();
            string selectedDate = dpConsultationDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string newPathMedicalConclusion = CopyFilesClass.CopyChildMedicalConclusion(_oldPathMedicalConclusion, _id);
            if (!ConsultationClass.AddChildrenConsultation(idDoctor, _id, newPathMedicalConclusion) || !ResultConsultationClass.AddResaultConsultations(_id) || !ChildrenDiagnosisClass.AddChildrenDiagnosis(selectedDate))
                return;
            DiagnosesClass.selectedDiagnoses.Clear();
            DiagnosesClass.selectedIDDiagnoses.Clear();
            ResultConsultationClass.oldFilePaths.Clear();
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();

        }

        private void LoadMedicalResults()
        {
            medicalResultsPanel.Children.Clear();
            foreach (string oldFilePath in ResultConsultationClass.oldFilePaths)
            {
                ConsultationDocumentsUserControl consultationDocumentsUserControl = new ConsultationDocumentsUserControl(true, ResultConsultationClass.oldFilePaths.IndexOf(oldFilePath), oldFilePath);
                consultationDocumentsUserControl.DeleteRequested += OnMedicalResultsDeleteRequested;
                medicalResultsPanel.Children.Add(consultationDocumentsUserControl);
            }
            if (DiagnosesClass.selectedDiagnoses.Count > 0 || ResultConsultationClass.oldFilePaths.Count > 0 || _haveMedicalConclusion)
                _haveChanges = true;
            else
                _haveChanges = false;
        }

        private void LoadMedicalConclusion(string filePath)
        {
            conclusionsPanel.Children.Clear();
            if (string.IsNullOrEmpty(filePath))
            {
                _haveMedicalConclusion = false;
                if (DiagnosesClass.selectedDiagnoses.Count > 0 || ResultConsultationClass.oldFilePaths.Count > 0 || _haveMedicalConclusion)
                    _haveChanges = true;
                else
                    _haveChanges = false;
                _oldPathMedicalConclusion = "";
                return;
            }
            _haveMedicalConclusion = true;
            ConsultationDocumentsUserControl consultationDocumentsUserControl = new ConsultationDocumentsUserControl(false, 0, filePath);
            consultationDocumentsUserControl.DeleteRequested += OnMedicalConclusionDeleteRequested;
            conclusionsPanel.Children.Add(consultationDocumentsUserControl);
            if (DiagnosesClass.selectedDiagnoses.Count > 0 || ResultConsultationClass.oldFilePaths.Count > 0 || _haveMedicalConclusion)
                _haveChanges = true;
            else
                _haveChanges = false;
        }

        private void LoadDiagnoses()
        {
            diagnosesPanel.Children.Clear();
            foreach (string diagnosis in DiagnosesClass.selectedDiagnoses)
            {
                int index = DiagnosesClass.selectedDiagnoses.IndexOf(diagnosis);
                ConsultationDiagnosisUserControl consultationDiagnosisUserControl = new ConsultationDiagnosisUserControl(diagnosis, index);
                consultationDiagnosisUserControl.DeleteRequested += OnDiagnosesDeleteRequest;
                diagnosesPanel.Children.Add(consultationDiagnosisUserControl);
            }
            if (DiagnosesClass.selectedDiagnoses.Count > 0 || ResultConsultationClass.oldFilePaths.Count > 0 || _haveMedicalConclusion)
                _haveChanges = true;
            else 
                _haveChanges = false;
        }

        private void OnMedicalResultsDeleteRequested()
        {
            LoadMedicalResults();
        }

        private void OnMedicalConclusionDeleteRequested()
        {
            LoadMedicalConclusion("");
        }

        private void OnDiagnosesDeleteRequest()
        {
            LoadDiagnoses();
        }
    }
}
