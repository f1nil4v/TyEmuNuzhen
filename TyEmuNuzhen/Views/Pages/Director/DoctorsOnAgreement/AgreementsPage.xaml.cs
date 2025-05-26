using System;
using System.Collections.Generic;
using System.Data;
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

namespace TyEmuNuzhen.Views.Pages.Director.DoctorsOnAgreement
{
    /// <summary>
    /// Логика взаимодействия для AgreementsPage.xaml
    /// </summary>
    public partial class AgreementsPage : Page
    {
        private string _id;

        public AgreementsPage(string id)
        {
            InitializeComponent();
            DoctorsOnAgreementClass.GetDoctorDataForPrint(id);
            string doctorName = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["name"].ToString();
            string doctorSurname = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["surname"].ToString();
            string doctorMiddleName = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["middleName"].ToString() == ""
                ? "" : DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["middleName"].ToString();
            string fullName = doctorName + " " + doctorSurname + " " + doctorMiddleName;
            headerTxt.Text += fullName;
            LoadAgreements(id);
            _id = id;
        }

        private void btnAddAppealConsent_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateDocumentsClass.CreateAgreementDoctor(_id))
                return;
            LoadAgreements(_id);
        }

        private void LoadAgreements(string id)
        {
            agreementPanel.Children.Clear();

            AgreementDoctorsClass.GetAgreementDoctorData(id);
            if (AgreementDoctorsClass.dtAgreementDoctorData.Rows.Count > 0)
            {
                bool isFirst = true;
                foreach (DataRow row in AgreementDoctorsClass.dtAgreementDoctorData.Rows)
                {
                    string filePath = row["filePath"].ToString();
                    string dateСonclusion = Convert.ToDateTime(row["dateConclusion"]).ToString("dd.MM.yyyy");
                    ImageUserControl photoControl = new ImageUserControl(3, isFirst, filePath, dateСonclusion, "");
                    agreementPanel.Children.Add(photoControl);
                    isFirst = false;
                }
                hasAgreementsTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                hasAgreementsTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }
    }
}
