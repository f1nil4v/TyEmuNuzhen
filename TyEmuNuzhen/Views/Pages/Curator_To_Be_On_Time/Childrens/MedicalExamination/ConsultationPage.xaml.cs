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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.MedicalExamination
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
            HelpManagerClass.CurrentHelpKey = "CuratorConsultationPage";
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
                    NavigationService.GoBack();
                    NavigationService.RemoveBackEntry();
                }
            }
            else
            {
                NavigationService.GoBack();
                NavigationService.RemoveBackEntry();
            }
            HelpManagerClass.CurrentHelpKey = "CuratorMedicalExaminationChildrensPage";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorPostsClass.GetDoctorPostsList();
            cbPost.ItemsSource = DoctorPostsClass.dtDoctorPostsList?.DefaultView;
            cbPost.DisplayMemberPath = "postName";
            cbPost.SelectedValuePath = "ID";
            cbPost.SelectedIndex = 0;
            DoctorsOnAgreementClass.GetDoctrosForComboBoxList(cbPost.SelectedValue.ToString());
            cbSpecialist.ItemsSource = DoctorsOnAgreementClass.dtDoctorsForComboBoxList?.DefaultView;
            cbSpecialist.DisplayMemberPath = "fullName";
            cbSpecialist.SelectedValuePath = "ID";
            cbSpecialist.SelectedIndex = 0;
        }

        private void cbPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string idPost = cbPost.SelectedValue == null ? null : cbPost.SelectedValue.ToString();
            DoctorsOnAgreementClass.GetDoctrosForComboBoxList(idPost);
            cbSpecialist.ItemsSource = DoctorsOnAgreementClass.dtDoctorsForComboBoxList?.DefaultView;
            cbSpecialist.DisplayMemberPath = "fullName";
            cbSpecialist.SelectedValuePath = "ID";
            cbSpecialist.SelectedIndex = 0;
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
                    NavigationService.GoBack();
                    NavigationService.RemoveBackEntry();
                }
            }
            else
            {
                NavigationService.GoBack();
                NavigationService.RemoveBackEntry();
            }
            HelpManagerClass.CurrentHelpKey = "CuratorMedicalExaminationChildrensPage";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ResultConsultationClass.oldFilePaths.Count == 0 || _haveMedicalConclusion == false || dpConsultationDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int countDiagnoses = DiagnosesClass.selectedDiagnoses.Count;
            string idDoctor = cbSpecialist.SelectedValue.ToString();
            string selectedDate = dpConsultationDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string newPathMedicalConclusion = CopyFilesClass.CopyChildMedicalConclusion(_oldPathMedicalConclusion, _id);
            if (String.IsNullOrEmpty(newPathMedicalConclusion))
                return;
            if (countDiagnoses == 0)
            {
                if (MessageBox.Show(@"Вы не добавили диагнозов. После подтверждения ребёнку будет присвоен статус Проблем не выявлено. Хотите продолжить?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (!ConsultationClass.AddChildrenConsultation(idDoctor, _id, newPathMedicalConclusion, selectedDate)
                        || !ResultConsultationClass.AddResaultConsultations(_id)
                        || !ChildrensClass.UpdateStatusChildren(_id, "11"))
                        return;
                }
                else
                    return;
            }
            else if (countDiagnoses == 1)
            {
                if (MessageBox.Show(@"Вы уверены, что добавили всю информацию медицинского освидетельствования? После подтверждения ребёнку попадёт в программу под ваше кураторство. Хотите продолжить?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (!ConsultationClass.AddChildrenConsultation(idDoctor, _id, newPathMedicalConclusion, selectedDate)
                        || !ResultConsultationClass.AddResaultConsultations(_id)
                        || !ChildrenDiagnosisClass.AddChildrenDiagnosis()
                        || !ActualDiagnosesClass.AddChildrenDiagnosis(_id)
                        || !ChildrensClass.UpdateStatusChildren(_id, "4")
                        || !ChildrensClass.UpdateStatusProgramChildren(_id, "1")
                        || !ActualProgramClass.AddChildrenActualProgram(_id, "1", CuratorClass.idCurator))
                        return;
                }
                else
                    return;
            }
            else if (countDiagnoses > 1)
            {
                if (MessageBox.Show(@"Вы уверены, что добавили всю информацию медицинского освидетельствования? После подтверждения ребёнку попадёт в программу под ваше кураторство. Хотите продолжить?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    if (!ConsultationClass.AddChildrenConsultation(idDoctor, _id, newPathMedicalConclusion, selectedDate)
                        || !ResultConsultationClass.AddResaultConsultations(_id)
                        || !ChildrenDiagnosisClass.AddChildrenDiagnosis()
                        || !ActualDiagnosesClass.AddChildrenDiagnosis(_id)
                        || !ChildrensClass.UpdateStatusChildren(_id, "4")
                        || !ChildrensClass.UpdateStatusProgramChildren(_id, "1")
                        || !ActualProgramClass.AddChildrenActualProgram(_id, "1", CuratorClass.idCurator))
                        return;
                }
            }
            NavigationService.Navigate(new ChildrensPage(1));
            NavigationService.RemoveBackEntry();
            HelpManagerClass.CurrentHelpKey = "CuratorMedicalExaminationChildrensPage";

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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DiagnosesClass.selectedDiagnoses.Clear();
            DiagnosesClass.selectedIDDiagnoses.Clear();
            ResultConsultationClass.oldFilePaths.Clear();
        }
    }
}
