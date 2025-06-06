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

namespace TyEmuNuzhen.Views.Pages.Director.Orphanages
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
            HelpManagerClass.CurrentHelpKey = "DirectorOrphanagesAgreementsPage";
            OrphanageClass.GetOrphanageData(id);
            string orphanageName = OrphanageClass.dtOrphanageDataList.Rows[0]["nameOrphanage"].ToString();
            headerTxt.Text = orphanageName;
            LoadAgreements(id);
            _id = id;
        }

        private void btnAddAppealConsent_Click(object sender, RoutedEventArgs e)
        {
            if (!CreateDocumentsClass.CreateAgreementOrphanage(_id))
                return;
            LoadAgreements(_id);
        }

        private void LoadAgreements(string id)
        {
            agreementPanel.Children.Clear();

            AgreementOrphanagesClass.GetAgreementOrphanageData(id);
            if (AgreementOrphanagesClass.dtAgreementOrphanageData.Rows.Count > 0)
            {
                bool isFirst = true;
                foreach (DataRow row in AgreementOrphanagesClass.dtAgreementOrphanageData.Rows)
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
