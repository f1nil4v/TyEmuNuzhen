using System;
using System.IO;
using System.Windows;
using wordAppealConsent = Microsoft.Office.Interop.Word;
using wordAgreementOrphanage = Microsoft.Office.Interop.Word;
using wordAgreementNanny = Microsoft.Office.Interop.Word;
using wordActOfCompletedNanny = Microsoft.Office.Interop.Word;

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
                string numLastAgreement = AgreementOrphanagesClass.GetMaxNumAgreementOrphanage() == "" ? "000000" : AgreementOrphanagesClass.GetMaxNumAgreementOrphanage();
                int numAgreementInt = Convert.ToInt32(numLastAgreement) + 1;
                string numAgreement = String.Format("{0:D6}", numAgreementInt);
                string fileName = $"Соглашение о социальном партнёрстве № {numAgreement}";
                if (!AgreementOrphanagesClass.AddAgreementOrphanage(numAgreement, idOrphanage, $"{_documentSaveFolderPath}Orphanages/Agreements/{fileName}.docx"))
                    throw new Exception("Не удалось добавить информацию о соглашении с ДДИ в базу данных");
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

        public static bool CreateAgreementNanny(string idNanny, string idChild, string costPerDay)
        {
            try
            {
                string idNannyOnProgram = NanniesOnProgramClass.GetLastNannyOnProgramID(idNanny);
                string dateNow = DateTime.Now.ToString("dd.MM.yyyy");
                string numLastAgreement = AgreementNannyOnProgramClass.GetMaxNumAgreementNanny() == "" ? "000000" : AgreementNannyOnProgramClass.GetMaxNumAgreementNanny();
                int numAgreementInt = Convert.ToInt32(numLastAgreement) + 1;
                string numAgreement = String.Format("{0:D6}", numAgreementInt);
                string fileName = $"Договор с няней на сопровождение ребёнка № {numAgreement}";
                if (!AgreementNannyOnProgramClass.AddAgreement(numAgreement, idNannyOnProgram, costPerDay, $"{_documentSaveFolderPath}Nannies/Agreements/{fileName}.docx"))
                    throw new Exception("Не удалось добавить информацию о договоре с няней в базу данных");
                if (!NanniesClass.UpdateNannyOnProgramStatus(idNanny, "1"))
                    throw new Exception("Не удалось обновить статус няни в базе данных");
                string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "agreementNanny.docx";
                string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + "Nannies/Agreements/" + fileName;
                
                ChildrensClass.GetChildrenListByID(idChild);
                string childSurname = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
                string childName = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
                string childMiddleName = ChildrensClass.dtChildrensDetailedList.Rows[0]["middleName"].ToString();
                string childrenFIO = childSurname + " " + childName + " " + childMiddleName;
                string childrenBirthday = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
                
                NanniesClass.GetNannyData(idNanny);

                string nannyName = NanniesClass.dtNanniesDataList.Rows[0]["name"].ToString();
                string nannySurname = NanniesClass.dtNanniesDataList.Rows[0]["surname"].ToString();
                string nannyMiddleName = NanniesClass.dtNanniesDataList.Rows[0]["middleName"].ToString() == ""
                    ? "" : NanniesClass.dtNanniesDataList.Rows[0]["middleName"].ToString();
                string middleNameInitials = nannyMiddleName == "" ? "" : nannyMiddleName[0] + ".";

                string FIONanny = nannySurname + " " + nannyName + " " + nannyMiddleName;
                string surnameNMNanny = nannySurname + ". " + nannyName[0] + ". " + middleNameInitials;

                string passSeries = NanniesClass.dtNanniesDataList.Rows[0]["passSeries"].ToString();
                string passNum = NanniesClass.dtNanniesDataList.Rows[0]["passNum"].ToString();
                string passDateOfIssue = Convert.ToDateTime(NanniesClass.dtNanniesDataList.Rows[0]["passDateOfIssue"]).ToString("dd.MM.yyyy");
                string passOrganizationOfIssue = NanniesClass.dtNanniesDataList.Rows[0]["passOrganizationOfIssue"].ToString();
                string passCode = NanniesClass.dtNanniesDataList.Rows[0]["passCode"].ToString();
                string addressRegister = NanniesClass.dtNanniesDataList.Rows[0]["addressRegister"].ToString();

                string phoneNumber = NanniesClass.dtNanniesDataList.Rows[0]["phoneNumber"].ToString();
                string email = NanniesClass.dtNanniesDataList.Rows[0]["email"].ToString();

                var app = new wordAgreementNanny.Application();
                app.Visible = false;
                var doc = app.Documents.Open(documentSampleFolderPath);
                doc.Activate();
                doc.Bookmarks["agreementNum"].Range.Text = numAgreement;
                doc.Bookmarks["dateAgreement"].Range.Text = dateNow;
                doc.Bookmarks["nannyFIO"].Range.Text = FIONanny;
                doc.Bookmarks["childFIO"].Range.Text = childrenFIO;
                doc.Bookmarks["childBirthday"].Range.Text = childrenBirthday;
                doc.Bookmarks["dateWorkBegin"].Range.Text = dateNow;
                doc.Bookmarks["costPerDay"].Range.Text = costPerDay;
                doc.Bookmarks["nannyFIO1"].Range.Text = FIONanny;
                doc.Bookmarks["phoneNumber"].Range.Text = phoneNumber;
                doc.Bookmarks["email"].Range.Text = email;
                doc.Bookmarks["passSeries"].Range.Text = passSeries;
                doc.Bookmarks["passNum"].Range.Text = passNum;
                doc.Bookmarks["passOrganizationOfIssue"].Range.Text = passOrganizationOfIssue;
                doc.Bookmarks["passDateOfIssue"].Range.Text = passDateOfIssue;
                doc.Bookmarks["passCode"].Range.Text = passCode;
                doc.Bookmarks["addressRegister"].Range.Text = addressRegister;
                doc.Bookmarks["nannySurnameInitials"].Range.Text = surnameNMNanny;
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

        public static bool CreateActOfCompleetedWorksNanny(string idNannyOnProgram, string idNanny, string idChild)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("dd.MM.yyyy");
                string dateNowFileName = DateTime.Now.ToString("dd_MM_yyyy");
                string numLastAgreement = ActOfCompletedWorksClass.GetMaxNumActOfCompletedWorksNanny() == "" ? "000000" : ActOfCompletedWorksClass.GetMaxNumActOfCompletedWorksNanny();
                int numActInt = Convert.ToInt32(numLastAgreement) + 1;
                string numAct = String.Format("{0:D6}", numActInt);
                string fileName = $"Акт выполненных работ № {numAct} - {dateNowFileName}";
                AgreementNannyOnProgramClass.GetAgreementNannyDataForPrint(idNannyOnProgram);
                TimeSpan difference = Convert.ToDateTime(dateNow) - Convert.ToDateTime(AgreementNannyOnProgramClass.dtAgreementNannyData.Rows[0]["dateConclusion"]);

                string countDays = difference.Days.ToString() == "0" ? "1" : difference.Days.ToString();
                string payment = (Convert.ToInt32(AgreementNannyOnProgramClass.dtAgreementNannyData.Rows[0]["costPerDay"]) * Convert.ToInt32(countDays)).ToString();

                if (!ActOfCompletedWorksClass.AddActOfCompletedWorks(numAct, idNannyOnProgram, countDays, payment, $"{_documentSaveFolderPath}Nannies/ActOfCompleetedWorks/{fileName}.docx"))
                    throw new Exception("Не удалось добавить информацию об акте выполненных работ в базу данных");
                if (!NanniesClass.UpdateNannyOnProgramStatus(idNanny, "0"))
                    throw new Exception("Не удалось обновить статус няни в базе данных");
                string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "actOfCompletedWorksNanny.docx";
                string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + "Nannies/ActOfCompleetedWorks/" + fileName;

                string agreementNum = String.Format("{0:D6}", Convert.ToInt32(AgreementNannyOnProgramClass.dtAgreementNannyData.Rows[0]["numOfAgreement"]));
                string costPerDay = AgreementNannyOnProgramClass.dtAgreementNannyData.Rows[0]["costPerDay"].ToString();
                string dateBegin = Convert.ToDateTime(AgreementNannyOnProgramClass.dtAgreementNannyData.Rows[0]["dateConclusion"]).ToString("dd.MM.yyyy");

                ChildrensClass.GetChildrenListByID(idChild);
                string childSurname = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
                string childName = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
                string childMiddleName = ChildrensClass.dtChildrensDetailedList.Rows[0]["middleName"].ToString();
                string childrenFIO = childSurname + " " + childName + " " + childMiddleName;
                string middleNameChildInitials = childMiddleName == "" ? "" : childMiddleName[0] + ".";
                string childrenInitials = childSurname + " " + childName[0] + ". " + middleNameChildInitials;
                string childrenBirthday = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");

                NanniesClass.GetNannyData(idNanny);

                string nannyName = NanniesClass.dtNanniesDataList.Rows[0]["name"].ToString();
                string nannySurname = NanniesClass.dtNanniesDataList.Rows[0]["surname"].ToString();
                string nannyMiddleName = NanniesClass.dtNanniesDataList.Rows[0]["middleName"].ToString() == ""
                    ? "" : NanniesClass.dtNanniesDataList.Rows[0]["middleName"].ToString();
                string middleNameInitials = nannyMiddleName == "" ? "" : nannyMiddleName[0] + ".";

                string FIONanny = nannySurname + " " + nannyName + " " + nannyMiddleName;
                string surnameNMNanny = nannySurname + ". " + nannyName[0] + ". " + middleNameInitials;

                string passSeries = NanniesClass.dtNanniesDataList.Rows[0]["passSeries"].ToString();
                string passNum = NanniesClass.dtNanniesDataList.Rows[0]["passNum"].ToString();
                string passDateOfIssue = Convert.ToDateTime(NanniesClass.dtNanniesDataList.Rows[0]["passDateOfIssue"]).ToString("dd.MM.yyyy");
                string passOrganizationOfIssue = NanniesClass.dtNanniesDataList.Rows[0]["passOrganizationOfIssue"].ToString();
                string passCode = NanniesClass.dtNanniesDataList.Rows[0]["passCode"].ToString();
                string addressRegister = NanniesClass.dtNanniesDataList.Rows[0]["addressRegister"].ToString();

                string phoneNumber = NanniesClass.dtNanniesDataList.Rows[0]["phoneNumber"].ToString();
                string email = NanniesClass.dtNanniesDataList.Rows[0]["email"].ToString();
                var app = new wordActOfCompletedNanny.Application();
                app.Visible = false;
                var doc = app.Documents.Open(documentSampleFolderPath);
                doc.Activate();
                doc.Bookmarks["actNum"].Range.Text = numAct;
                doc.Bookmarks["agreementNum"].Range.Text = agreementNum;
                doc.Bookmarks["agreementDate"].Range.Text = dateBegin;
                doc.Bookmarks["dateAct"].Range.Text = dateNow;
                doc.Bookmarks["nannyFIO"].Range.Text = FIONanny;
                doc.Bookmarks["childFIO"].Range.Text = childrenFIO;
                doc.Bookmarks["childBirthDay"].Range.Text = childrenBirthday;
                doc.Bookmarks["agreementDate1"].Range.Text = dateBegin;
                doc.Bookmarks["dateAct1"].Range.Text = dateNow;
                doc.Bookmarks["agreementNum2"].Range.Text = agreementNum;
                doc.Bookmarks["agreementDate2"].Range.Text = dateBegin;
                doc.Bookmarks["costWorkDays"].Range.Text = payment;
                doc.Bookmarks["childFIOInitials"].Range.Text = childrenInitials;
                doc.Bookmarks["costPerDay"].Range.Text = costPerDay;
                doc.Bookmarks["countDays"].Range.Text = countDays;
                doc.Bookmarks["costWorkDays1"].Range.Text = payment;
                doc.Bookmarks["nannyFIO1"].Range.Text = FIONanny;
                doc.Bookmarks["phoneNumber"].Range.Text = phoneNumber;
                doc.Bookmarks["email"].Range.Text = email;
                doc.Bookmarks["passSeries"].Range.Text = passSeries;
                doc.Bookmarks["passNum"].Range.Text = passNum;
                doc.Bookmarks["passOrganizationOfIssue"].Range.Text = passOrganizationOfIssue;
                doc.Bookmarks["passDateOfIssue"].Range.Text = passDateOfIssue;
                doc.Bookmarks["passCode"].Range.Text = passCode;
                doc.Bookmarks["addressRegister"].Range.Text = addressRegister;
                doc.Bookmarks["nannySurnameInitials"].Range.Text = surnameNMNanny;
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
