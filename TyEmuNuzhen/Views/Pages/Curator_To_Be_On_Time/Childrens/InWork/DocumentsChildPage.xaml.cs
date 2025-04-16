using Microsoft.Win32;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.InWork
{
    /// <summary>
    /// Логика взаимодействия для DocumentsChildPage.xaml
    /// </summary>
    public partial class DocumentsChildPage : Page
    {
        private string _id;
        private bool _hasAppealsConsents = false;

        public DocumentsChildPage(string id, string FIOChild)
        {
            InitializeComponent();
            _id = id;
            fullNameChild.Text += FIOChild;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
            NavigationService.RemoveBackEntry();
        }

        private void BtnAddDocument_Click(object sender, RoutedEventArgs e)
        {
            DocumentTypeWindow documentTypeWindow = new DocumentTypeWindow();
            if (documentTypeWindow.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(DocumentTypeClass.selectedIDTypeDocument))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
                    };
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string filePath = openFileDialog.FileName;
                        string newFilePath = CopyFilesClass.CopyChildDocument(filePath, _id);
                        ChildrenDocumentClass.GetSameDocumentScanByDocumentID(_id, DocumentTypeClass.selectedIDTypeDocument);
                        if (ChildrenDocumentClass.dtTemporaryDocumentChildrenDocuments.Rows.Count > 0)
                        {
                            string oldFilePath = ChildrenDocumentClass.dtTemporaryDocumentChildrenDocuments.Rows[0]["filePath"].ToString();
                            File.Delete(oldFilePath);
                            if (!ChildrenDocumentClass.UpdateChildrenDocument(_id, DocumentTypeClass.selectedIDTypeDocument, newFilePath))
                                return;
                        }
                        else
                        {
                            if (!ChildrenDocumentClass.AddChildrenDocument(_id, DocumentTypeClass.selectedIDTypeDocument, newFilePath))
                                return;
                        }
                        LoadDocuments(_id);
                    }
                }
            }
        }

        private void btnAddAppealConsent_Click(object sender, RoutedEventArgs e)
        {
            string idOrphanage = ChildrensClass.dtChildrensDetailedList.Rows[0]["idOrphanage"].ToString();
            string FIOChild = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString()
                + " " + ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString()
                + " " + ChildrensClass.dtChildrensDetailedList.Rows[0]["middleName"].ToString();
            string birthdayChild = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
            string orphanageName = ChildrensClass.dtChildrensDetailedList.Rows[0]["orphanageName"].ToString();
            CreateDocumentsClass.CreateAppealConsent(_id, idOrphanage, FIOChild, birthdayChild, orphanageName);
            LoadAppealsConsents(_id);
            LoadDocuments(_id);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string needAgreement = "Необходимо заключить договор на социальное партнёрство с детским домом-интернатом. Обратитесь к директору.";
            string needConsultation = "Необходимо установить диагнозы. Проведите консультацию.";
            ChildrensClass.GetChildrenListByID(_id);
            ChildrenDiagnosisClass.GetChildrenDiagnoses(_id);
            string idOrphanage = ChildrensClass.dtChildrensDetailedList.Rows[0]["idOrphanage"].ToString();
            AgreementOrphanagesClass.GetAgreementOrphanageData(idOrphanage);
            if (AgreementOrphanagesClass.dtAgreementOrphanageData.Rows.Count == 0)
            {
                hasAppealsConsentsTextBlock.Text = needAgreement;
                hasDocumentsTextBlock.Text = needAgreement;
                hasAppealsConsentsTextBlock.Visibility = Visibility.Visible;
                hasDocumentsTextBlock.Visibility = Visibility.Visible;
                btnAddAppealConsent.IsEnabled = false;
                btnAddDocument.IsEnabled = false;
                return;
            }
            if (ChildrenDiagnosisClass.dtChildrenDiagnoses.Rows.Count == 0)
            {
                hasAppealsConsentsTextBlock.Text = needConsultation;
                hasDocumentsTextBlock.Text = needConsultation;
                hasAppealsConsentsTextBlock.Visibility = Visibility.Visible;
                hasDocumentsTextBlock.Visibility = Visibility.Visible;
                btnAddAppealConsent.IsEnabled = false;
                btnAddDocument.IsEnabled = false;
                return;
            }
            LoadAppealsConsents(_id);
            LoadDocuments(_id);
        }

        private void LoadAppealsConsents(string childId)
        {
            ChildrensClass.GetChildrenListByID(childId);
            string idOrphanage = ChildrensClass.dtChildrensDetailedList.Rows[0]["idOrphanage"].ToString();
            appealsConsentsPanel.Children.Clear();

            ChildrenDocumentClass.GetChildrenDocuments(childId, false, idOrphanage);
            if (ChildrenDocumentClass.dtChildrenAppealsConsents.Rows.Count > 0)
            {
                bool isFirst = true;
                foreach (DataRow row in ChildrenDocumentClass.dtChildrenAppealsConsents.Rows)
                {
                    string filePath = row["filePath"].ToString();
                    string dateСonclusion = Convert.ToDateTime(row["dateСonclusion"]).ToString("dd.MM.yyyy");
                    ImageUserControl photoControl = new ImageUserControl(2, isFirst, filePath, dateСonclusion, "");
                    appealsConsentsPanel.Children.Add(photoControl);
                    isFirst = false;
                }
                hasAppealsConsentsTextBlock.Visibility = Visibility.Collapsed;
                _hasAppealsConsents = true;
                btnAddAppealConsent.IsEnabled = false;
                btnAddDocument.IsEnabled = true;
            }
            else
            {
                hasAppealsConsentsTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void LoadDocuments(string childId)
        {
            documentsPanel.Children.Clear();

            if (_hasAppealsConsents == false)
            {
                hasDocumentsTextBlock.Text = "Необходимо обращение на благотворительную помощь и согласие на обработку персональных данных/на фото- видеосъёмку";
                hasDocumentsTextBlock.Visibility = Visibility.Visible;
                btnAddDocument.IsEnabled = false;
                return;
            }

            ChildrenDocumentClass.GetChildrenDocuments(childId, true, "");
            if (ChildrenDocumentClass.dtChildrenDocumentsScan.Rows.Count > 0)
            {
                bool isFirst = true;
                foreach (DataRow row in ChildrenDocumentClass.dtChildrenDocumentsScan.Rows)
                {
                    string filePath = row["filePath"].ToString();
                    string documentType = row["documentType"].ToString();
                    ImageUserControl photoControl = new ImageUserControl(1, isFirst, filePath, "",documentType);
                    documentsPanel.Children.Add(photoControl);
                    isFirst = false;
                }
            }
            else
            {
                hasDocumentsTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
