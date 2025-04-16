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
        public ConsultationPage(string id, string FIOchild)
        {
            InitializeComponent();
            dpConsultationDate.SelectedDate = DateTime.Now;
            _id = id;
            fullNameChild.Text += FIOchild;
        }

        private void imgBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorPostsClass.GetDoctorPostsList();
            cbPost.ItemsSource = DoctorPostsClass.dtDoctorPostsList?.DefaultView;
            cbPost.DisplayMemberPath = "postName";
            cbPost.SelectedValuePath = "ID";
            DoctorsOnAgreementClass.GetDoctrosForComboBoxList("");
            cbSpecialist.ItemsSource = DoctorsOnAgreementClass.dtDoctorsForComboBoxList?.DefaultView;
            cbSpecialist.DisplayMemberPath = "fullName";
            cbSpecialist.SelectedValuePath = "ID";
        }

        private void cbPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string idPost = cbPost.SelectedValue == null ? null : cbPost.SelectedValue.ToString();
            DoctorsOnAgreementClass.GetDoctrosForComboBoxList(idPost);
            cbSpecialist.ItemsSource = DoctorsOnAgreementClass.dtDoctorsForComboBoxList?.DefaultView;
            cbSpecialist.DisplayMemberPath = "fullName";
            cbSpecialist.SelectedValuePath = "ID";
            cbSpecialist.SelectedIndex = -1;
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
                LoadMedicalConclusion(filePath);
            }
        }

        private void btnAddDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            DiagnosisWindow diagnosisWindow = new DiagnosisWindow();
            if (diagnosisWindow.ShowDialog() == true)
                LoadDiagnoses();
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
        }

        private void LoadMedicalConclusion(string filePath)
        {
            conclusionsPanel.Children.Clear();
            if (string.IsNullOrEmpty(filePath))
                return;
            ConsultationDocumentsUserControl consultationDocumentsUserControl = new ConsultationDocumentsUserControl(false, 0, filePath);
            consultationDocumentsUserControl.DeleteRequested += OnMedicalConclusionDeleteRequested;
            conclusionsPanel.Children.Add(consultationDocumentsUserControl);
        }

        private void LoadDiagnoses()
        {
            diagnosesPanel.Children.Clear();
            foreach (string diagnosis in DiagnosesClass.selectedDiagnoses)
            {
                int index = DiagnosesClass.selectedDiagnoses.IndexOf(diagnosis);
                ConsultationDiagnosisUserControl consultationDiagnosisUserControl = new ConsultationDiagnosisUserControl(diagnosis, index);
                consultationDiagnosisUserControl.DeleteRequested += OnMedicalResultsDeleteRequested;
                diagnosesPanel.Children.Add(consultationDiagnosisUserControl);
            }
        }

        private void OnMedicalResultsDeleteRequested()
        {
            LoadMedicalResults();
        }

        private void OnMedicalConclusionDeleteRequested()
        {
            LoadMedicalConclusion("");
        }

        private void OnDiagnosesDeleteReques()
        {
            LoadDiagnoses();
        }
    }
}
