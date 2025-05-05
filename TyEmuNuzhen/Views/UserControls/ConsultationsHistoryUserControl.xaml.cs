using System.Data;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ConsultationsHistoryUserControl.xaml
    /// </summary>
    public partial class ConsultationsHistoryUserControl : UserControl
    {
        public ConsultationsHistoryUserControl(string idConsultation, string fioDoctorInitials, string doctorPost, string filePathMedicalConclusion, string dateСonclusion)
        {
            InitializeComponent();
            LoadConsultation(idConsultation, fioDoctorInitials, doctorPost, filePathMedicalConclusion, dateСonclusion);
        }

        private void LoadConsultation(string idConsultation, string fioDoctorInitials, string doctorPost, string filePathMedicalConclusion, string dateСonclusion)
        {
            dateConsultation.Text = dateСonclusion + " - " + doctorPost + " " + fioDoctorInitials;
            diagnosisPanel.Children.Clear();

            ChildrenDiagnosisClass.GetChildrenDiagnoses(idConsultation);
            DataView view = ChildrenDiagnosisClass.dtChildrenDiagnoses.DefaultView;
            foreach (DataRowView row in view)
            {
                string diagnosisName = row["diagnosisName"].ToString();
                string dateAdded = "";
                DescriptionUserControl descriptionUserControl = new DescriptionUserControl(false, dateAdded, diagnosisName);
                diagnosisPanel.Children.Add(descriptionUserControl);
            }

            resultConsultationPanel.Children.Clear();
            ResultConsultationClass.GetResultConsultation(idConsultation);
            DataView viewResult = ResultConsultationClass.dtResultConsultationList.DefaultView;
            foreach (DataRowView row in viewResult)
            {
                string filePath = row["filePath"].ToString();
                ImageUserControl imageUserControl = new ImageUserControl(1, false, filePath, "", "");
                resultConsultationPanel.Children.Add(imageUserControl);
            }

            conclusionConsultationPanel.Children.Clear();

            ImageUserControl imageUserControlMedicalConclusion = new ImageUserControl(2, false, filePathMedicalConclusion, dateСonclusion, "");
            conclusionConsultationPanel.Children.Add(imageUserControlMedicalConclusion);
        }
    }
}
