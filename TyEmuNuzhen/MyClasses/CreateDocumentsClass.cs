using System;
using System.IO;
using System.Windows;
using wordAppealConsent = Microsoft.Office.Interop.Word;

namespace TyEmuNuzhen.MyClasses
{
    internal class CreateDocumentsClass
    {
        private static string _documentSamplesFolderPath = @"../../Documents/Samples/";
        private static string _documentSaveFolderPath = @"../../Documents/Children/AppealsConsents/";

        public static void CreateAppealConsent(string idChildren, string idOrhanage, string fioChild, string birthdayChild, string orphanageName)
        {
            string fileName = $"Обращение + согласие ДДИ - {fioChild} {orphanageName}";
            string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "appeal_consent_sample.docx";
            string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + fileName;
            OrphanageClass.GetOrphanageDataForPrintDocuments(idOrhanage);
            AgreementOrphanagesClass.GetAgreementOrphanageData(idOrhanage);
            string FIODirector = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorSurname"].ToString()
                + " " + OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorName"].ToString() 
                + " " + OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorMiddleName"].ToString();

            string surnameNMDirectror = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorName"].ToString()[0]
                + ". " + OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorMiddleName"].ToString()[0]
                + ". " + OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorSurname"].ToString();

            DateTime dateNow = DateTime.Now;
            string shortDateFormat = dateNow.ToString("dd.MM.yyyy");
            var app = new wordAppealConsent.Application();
            app.Visible = false;
            var doc = app.Documents.Open(documentSampleFolderPath);
            doc.Activate();

            doc.Bookmarks["appealNum"].Range.Text = AgreementOrphanagesClass.dtAgreementOrphanageData.Rows[0]["ID"].ToString();
            doc.Bookmarks["dateNow"].Range.Text = shortDateFormat;
            doc.Bookmarks["orphanageName"].Range.Text = orphanageName;
            doc.Bookmarks["surnameNMDirectror"].Range.Text = surnameNMDirectror;
            doc.Bookmarks["FIODirector"].Range.Text = FIODirector;
            doc.Bookmarks["orphanageName1"].Range.Text = orphanageName;
            doc.Bookmarks["FIOChild"].Range.Text = fioChild;
            doc.Bookmarks["birthdayChild"].Range.Text = birthdayChild;
            for (int i = 0; i < ChildrenDiagnosisClass.dtChildrenDiagnoses.Rows.Count; i++)
            {
                doc.Bookmarks["diagnosesList"].Range.Text +=
                    ChildrenDiagnosisClass.dtChildrenDiagnoses.Rows[i]["diagnosisName"].ToString();
                if (i < ChildrenDiagnosisClass.dtChildrenDiagnoses.Rows.Count - 1)
                {
                    doc.Bookmarks["diagnosesList"].Range.Text += ", ";
                }
            }
            doc.Bookmarks["agrementNumOrphanage"].Range.Text = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["ID"].ToString();
            doc.Bookmarks["dateAgreementOrphanage"].Range.Text = Convert.ToDateTime(AgreementOrphanagesClass.dtAgreementOrphanageData.Rows[0]["dateConclusion"]).ToString("dd.MM.yyyy");
            doc.Bookmarks["FIODirector1"].Range.Text = FIODirector;
            doc.Bookmarks["dateNow1"].Range.Text = shortDateFormat;
            doc.Bookmarks["programName"].Range.Text = RolesClass.GetRole();
            doc.Bookmarks["FIOEmployee"].Range.Text = CuratorClass.fullNameCurator;
            doc.Bookmarks["FIODirector2"].Range.Text = FIODirector;
            doc.Bookmarks["FIOChild1"].Range.Text = fioChild;
            doc.Bookmarks["birhdayChild1"].Range.Text = birthdayChild;
            doc.Bookmarks["FIODirector3"].Range.Text = FIODirector;
            doc.Bookmarks["dateNow2"].Range.Text = shortDateFormat;

            doc.Saved = true;
            try
            {
                if (!ChildrenDocumentClass.AddChildrenDocument(idChildren, "1", $"{_documentSaveFolderPath}{fileName}.pdf") || !ConsentsClass.AddChildrenAppealConsent(idOrhanage))
                    throw new Exception("Не удалось добавить информацию о документе в базу данных");
                doc.SaveAs2($"{documentSaveFolderPath}.pdf", wordAppealConsent.WdSaveFormat.wdFormatPDF);
                doc.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                doc.Close();
            }
        }
    }
}
