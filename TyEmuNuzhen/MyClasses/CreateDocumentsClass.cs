using System;
using System.IO;
using System.Windows;
using wordAppealConsent = Microsoft.Office.Interop.Word;
using wordAgreementOrphanage = Microsoft.Office.Interop.Word;

namespace TyEmuNuzhen.MyClasses
{
    internal class CreateDocumentsClass
    {
        private static string _documentSamplesFolderPath = @"../../Documents/Samples/";
        private static string _documentSaveFolderPath = @"../../Documents/";

        public static void CreateAppealConsent(string idChildren, string idOrhanage, string fioChild, string birthdayChild, string orphanageName)
        {
            string numLastAppeal = ConsentsClass.GetMaxNumAppealOrphanage() == "" ? "000000" : ConsentsClass.GetMaxNumAppealOrphanage();
            int numAppealInt = Convert.ToInt32(numLastAppeal) + 1;
            string numAppeal = String.Format("{0:D6}", numAppealInt);
            string fileName = $"Обращение + согласие ДДИ №{numAppeal} - {orphanageName}";
            string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "appeal_consent_sample.docx";
            string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + @"Children/AppealsConsents/" + fileName;
            OrphanageClass.GetOrphanageDataForPrintDocuments(idOrhanage);
            AgreementOrphanagesClass.GetAgreementOrphanageDataForPrint(idOrhanage);
            string directorName = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorName"].ToString();
            string directorSurname = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorSurname"].ToString();
            string directorMiddleName = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorMiddleName"].ToString() == ""
                ? "" : OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorMiddleName"].ToString();
            string middleNameInitials = directorMiddleName == "" ? "" : directorMiddleName[0] + ".";

            string FIODirector = directorSurname + " " + directorName + " " + directorMiddleName;
            string surnameNMDirectror = directorSurname + ". " + directorName[0] + ". " + middleNameInitials;

            string numAgreement = String.Format("{0:D6}", AgreementOrphanagesClass.dtAgreementOrphanageData.Rows[0]["numAgreement"].ToString());

            string idActualProgram = ActualProgramClass.GetIDLastActualProgramChildren(idChildren);

            DateTime dateNow = DateTime.Now;
            string shortDateFormat = dateNow.ToString("dd.MM.yyyy");
            var app = new wordAppealConsent.Application();
            app.Visible = false;
            var doc = app.Documents.Open(documentSampleFolderPath);
            doc.Activate();

            doc.Bookmarks["appealNum"].Range.Text = numAppeal;
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
            doc.Bookmarks["agrementNumOrphanage"].Range.Text = numAgreement;
            doc.Bookmarks["dateAgreementOrphanage"].Range.Text = Convert.ToDateTime(AgreementOrphanagesClass.dtAgreementOrphanageData.Rows[0]["dateConclusion"]).ToString("dd.MM.yyyy");
            doc.Bookmarks["FIODirector1"].Range.Text = FIODirector;
            doc.Bookmarks["dateNow1"].Range.Text = shortDateFormat;
            doc.Bookmarks["programName"].Range.Text = ActualProgramClass.GetLastActualProgramChildren(idActualProgram);
            doc.Bookmarks["FIOEmployee"].Range.Text = CuratorClass.fullNameCurator;
            doc.Bookmarks["FIODirector2"].Range.Text = FIODirector;
            doc.Bookmarks["FIOChild1"].Range.Text = fioChild;
            doc.Bookmarks["birhdayChild1"].Range.Text = birthdayChild;
            doc.Bookmarks["FIODirector3"].Range.Text = FIODirector;
            doc.Bookmarks["dateNow2"].Range.Text = shortDateFormat;

            doc.Saved = true;
            try
            {
                if (!ChildrenDocumentClass.AddChildrenDocument(idChildren, "1", $"{_documentSaveFolderPath}Children/AppealsConsents/{fileName}.pdf") || !ConsentsClass.AddChildrenAppealConsent(numAppeal, idOrhanage, idActualProgram))
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
        
        public static bool CreateAgreementOrphanage(string idOrphanage)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("dd.MM.yyyy");
                string dateNowFileName = DateTime.Now.ToString("dd_MM_yyyy");
                string numLastAgreement = AgreementOrphanagesClass.GetMaxNumAgreementOrphanage() == "" ? "000000" : AgreementOrphanagesClass.GetMaxNumAgreementOrphanage();
                int numAgreementInt = Convert.ToInt32(numLastAgreement) + 1;
                string numAgreement = String.Format("{0:D6}", numAgreementInt);
                string fileName = $"Соглашение о социальном партнёрстве № {numAgreement} - {dateNowFileName}";
                if (!AgreementOrphanagesClass.AddAgreementOrphanage(numAgreement, idOrphanage, $"{_documentSaveFolderPath}Orphanages/Agreements/{fileName}.docx"))
                    throw new Exception("Не удалось добавить информацию о соглашении в базу данных");
                string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "agreementOrphanage.docx";
                string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + "Orphanages/Agreements/" + fileName;
                OrphanageClass.GetOrphanageDataForPrintDocuments(idOrphanage);

                string directorName = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorName"].ToString();
                string directorSurname = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorSurname"].ToString();
                string directorMiddleName = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorMiddleName"].ToString() == "" 
                    ? "" : OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["directorMiddleName"].ToString();
                string middleNameInitials = directorMiddleName == "" ? "" : directorMiddleName[0] + ".";

                string FIODirector = directorSurname + " " + directorName + " " + directorMiddleName;
                string surnameNMDirectror = directorSurname + ". " + directorName[0] + ". " + middleNameInitials;

                string orphanageName = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["nameOrphanage"].ToString();
                string address = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["address"].ToString();
                string email = OrphanageClass.dtOrphanageDataForPrintDocuments.Rows[0]["email"].ToString();
                var app = new wordAgreementOrphanage.Application();
                app.Visible = false;
                var doc = app.Documents.Open(documentSampleFolderPath);
                doc.Activate();
                doc.Bookmarks["agreementNum"].Range.Text = numAgreement;
                doc.Bookmarks["dateNow"].Range.Text = dateNow;
                doc.Bookmarks["nameOrphanage"].Range.Text = orphanageName;
                doc.Bookmarks["directorFIO"].Range.Text = FIODirector;
                doc.Bookmarks["nameOrphanage1"].Range.Text = orphanageName;
                doc.Bookmarks["addressOrphanage"].Range.Text = address;
                doc.Bookmarks["emailOrphanage"].Range.Text = email;
                doc.Bookmarks["directorFamIO"].Range.Text = surnameNMDirectror;
                doc.Saved = true;
                try
                {
                    doc.SaveAs2($"{documentSaveFolderPath}.docx");
                    doc.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    doc.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
}
    }
}
