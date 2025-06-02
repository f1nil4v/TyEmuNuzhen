using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks
{
    /// <summary>
    /// Логика взаимодействия для DocumentsPage.xaml
    /// </summary>
    public partial class DocumentsPage : Page
    {
        private string _id;

        public DocumentsPage(string id, string FIOChild)
        {
            InitializeComponent();
            _id = id;
            fullNameChild.Text += FIOChild;
            HelpManagerClass.CurrentHelpKey = "CuratorDocumentsCPage";

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
            HelpManagerClass.CurrentHelpKey = "CuratorCompletedWorksPage";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ChildrensClass.GetChildrenListByID(_id);
            ChildrenDiagnosisClass.GetChildrenDiagnoses(_id);
            string idOrphanage = ChildrensClass.dtChildrensDetailedList.Rows[0]["idOrphanage"].ToString();
            AgreementOrphanagesClass.GetAgreementOrphanageData(idOrphanage);
            LoadDocuments(_id);
        }

        private void LoadDocuments(string childId)
        {
            LoadHeaderCount();
            documentsPanel.Children.Clear();
            ChildrenDocumentClass.GetChildrenDocuments(childId, true, "");
            if (ChildrenDocumentClass.dtChildrenDocumentsScan.Rows.Count > 0)
            {
                bool isFirst = true;
                foreach (DataRow row in ChildrenDocumentClass.dtChildrenDocumentsScan.Rows)
                {
                    string filePath = row["filePath"].ToString();
                    string documentType = row["documentType"].ToString();
                    ImageUserControl photoControl = new ImageUserControl(1, isFirst, filePath, "", documentType);
                    documentsPanel.Children.Add(photoControl);
                    isFirst = false;
                }
            }
            else
            {
                hasDocumentsTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void LoadHeaderCount()
        {
            string countAllDocs = DocumentTypeClass.GetCountAllDocumentTypes();
            string countChildDocs = ChildrenDocumentClass.GetCountChildrenDocuments(_id);
            documentsGrid.Header = "Документы (" + countChildDocs + "/" + countAllDocs + ")";
        }

    }
}
