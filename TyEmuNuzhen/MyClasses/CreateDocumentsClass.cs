using System;
using System.IO;
using System.Windows;
using wordAppealConsent = Microsoft.Office.Interop.Word;
using wordAgreementOrphanage = Microsoft.Office.Interop.Word;
using wordAgreementNanny = Microsoft.Office.Interop.Word;
using wordActOfCompletedNanny = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Data;

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
            if (!Directory.Exists(_documentSaveFolderPath + @"Children/AppealsConsents/"))
                Directory.CreateDirectory(_documentSaveFolderPath + @"Children/AppealsConsents/");
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
                app.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                doc.Close();
                app.Quit();
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
                if (!Directory.Exists(_documentSaveFolderPath + "Orphanages/Agreements/"))
                    Directory.CreateDirectory(_documentSaveFolderPath + "Orphanages/Agreements/");
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
                    app.Quit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    doc.Close();
                    app.Quit();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool CreateAgreementDoctor(string idDoctor)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("dd.MM.yyyy");
                string numLastAgreement = AgreementDoctorsClass.GetMaxNumAgreementDoctor() == "" ? "000000" : AgreementDoctorsClass.GetMaxNumAgreementDoctor();
                int numAgreementInt = Convert.ToInt32(numLastAgreement) + 1;
                string numAgreement = String.Format("{0:D6}", numAgreementInt);
                string fileName = $"Договор на сотрудничество № {numAgreement}";
                if (!AgreementDoctorsClass.AddAgreementDoctor(numAgreement, idDoctor, $"{_documentSaveFolderPath}Doctors/Agreements/{fileName}.docx"))
                    throw new Exception("Не удалось добавить информацию о Договоре на сотруднечество в базу данных");
                string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "agreementDoctor.docx";
                if (!Directory.Exists(_documentSaveFolderPath + "Doctors/Agreements/"))
                    Directory.CreateDirectory(_documentSaveFolderPath + "Doctors/Agreements/");
                string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + "Doctors/Agreements/" + fileName;
                DoctorsOnAgreementClass.GetDoctorDataForPrint(idDoctor);

                string doctorName = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["name"].ToString();
                string doctorSurname = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["surname"].ToString();
                string doctorMiddleName = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["middleName"].ToString() == ""
                    ? "" : DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["middleName"].ToString();
                string middleNameInitials = doctorMiddleName == "" ? "" : doctorMiddleName[0] + ".";

                string FIODoctor = doctorSurname + " " + doctorName + " " + doctorMiddleName;
                string surnameNMDoctor = doctorSurname + ". " + doctorName[0] + ". " + middleNameInitials;

                string postName = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["postName"].ToString();

                string medicalFacilityName = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["medicalFacilityName"].ToString();
                string medicalFacilityAddress = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["address"].ToString();

                string phone = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["phoneNumber"].ToString();
                string email = DoctorsOnAgreementClass.dtDoctorDataForPrint.Rows[0]["email"].ToString();
                var app = new wordAgreementOrphanage.Application();
                app.Visible = false;
                var doc = app.Documents.Open(documentSampleFolderPath);
                doc.Activate();
                doc.Bookmarks["agreementNum"].Range.Text = numAgreement;
                doc.Bookmarks["dateNow"].Range.Text = dateNow;
                doc.Bookmarks["postName"].Range.Text = postName;
                doc.Bookmarks["doctorFIO"].Range.Text = FIODoctor;
                doc.Bookmarks["medicalFacilityName"].Range.Text = medicalFacilityName;
                doc.Bookmarks["medicalFacilityAddress"].Range.Text = medicalFacilityAddress;
                doc.Bookmarks["doctorFIO1"].Range.Text = FIODoctor;
                doc.Bookmarks["phoneNumber"].Range.Text = phone;
                doc.Bookmarks["email"].Range.Text = email;
                doc.Bookmarks["doctorFamIO"].Range.Text = surnameNMDoctor;
                doc.Saved = true;
                try
                {
                    doc.SaveAs2($"{documentSaveFolderPath}.docx");
                    doc.Close();
                    app.Quit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    doc.Close();
                    app.Quit();
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
                if (!Directory.Exists(_documentSaveFolderPath + "Nannies/Agreements/"))
                    Directory.CreateDirectory(_documentSaveFolderPath + "Nannies/Agreements/");
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
                    app.Quit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    doc.Close();
                    app.Quit();
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
                if (!Directory.Exists(_documentSaveFolderPath + "Nannies/ActOfCompleetedWorks/"))
                    Directory.CreateDirectory(_documentSaveFolderPath + "Nannies/ActOfCompleetedWorks/");
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
                    app.Quit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    doc.Close();
                    app.Quit();
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool CreateReportToBeOnTimeChild(string idChild, string conclusion)
        {
            try
            {
                string dateNowQuery = DateTime.Now.ToString("yyyy-MM-dd");
                string dateNow = DateTime.Now.ToString("dd.MM.yyyy");
                string dateNowFileName = DateTime.Now.ToString("dd_MM_yyyy");
                string idActualProgram = ActualProgramClass.GetIDLastActualProgramChildren(idChild);

                string numReport = String.Format("{0:D6}", idActualProgram);

                string fileName = $"Отчёт по программе Чтобы успеть вовремя №{numReport} - {dateNowFileName}";
                if (!ActualProgramClass.UpdateChildrenActualProgramEndProgram(idActualProgram, dateNowQuery, $"{_documentSaveFolderPath}Children/Reports/ToBeOnTime/{fileName}.docx"))
                    throw new Exception("Не удалось обновить информацию об актуальной программе в базе данных");
                string documentSampleFolderPath = Path.GetFullPath(_documentSamplesFolderPath) + "reportToBeOnTimeChild.docx";
                if (!Directory.Exists(_documentSaveFolderPath + "Children/Reports/ToBeOnTime/"))
                    Directory.CreateDirectory(_documentSaveFolderPath + "Children/Reports/ToBeOnTime/");
                string documentSaveFolderPath = Path.GetFullPath(_documentSaveFolderPath) + "Children/Reports/ToBeOnTime/" + fileName;
                
                ChildrensClass.GetChildrenListByID(idChild);

                string childSurname = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
                string childName = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
                string childMiddleName = ChildrensClass.dtChildrensDetailedList.Rows[0]["middleName"].ToString();

                string childrenFIO = childSurname + " " + childName + " " + childMiddleName;
                string childrenBirthday = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
                string childRegionName = ChildrensClass.dtChildrensDetailedList.Rows[0]["regionName"].ToString();
                string childOrphanageName = ChildrensClass.dtChildrensDetailedList.Rows[0]["orphanageName"].ToString();

                ActualProgramClass.GetActualProgramDataToBeOnTimeForPrint(idActualProgram);

                string curatorFIO = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["curatorFullName"].ToString();
                string curatorPhoneNumber = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["phoneNumber"].ToString();
                string curatorEmail = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["email"].ToString();
                string dateProgramBegin = Convert.ToDateTime(ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["dateBegin"]).ToString("dd.MM.yyy");
                string dateProgramEnd = Convert.ToDateTime(ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["dateEnd"]).ToString("dd.MM.yyy");
                string hospitalName = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["medicalFacilityName"].ToString();
                string hospitalAddress = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["address"].ToString();
                string dateHospitalization = Convert.ToDateTime(ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["dateHospitalization"]).ToString("dd.MM.yyy");
                string dateDischarge = Convert.ToDateTime(ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["dateDischarge"]).ToString("dd.MM.yyy");
                string hospitalizationTotalCost = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["totalCost"].ToString();
                string countNanniesOnProgram = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["countNanniesOnProgram"].ToString();
                string idHospitalization = ActualProgramClass.dtActualProgramDataForPrint.Rows[0]["hospitalizationID"].ToString();

                TransferClass.GetTransferOnHozpitalizationSide1Data(idHospitalization);
                string idTransfer0 = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["ID"].ToString();
                string dateDeparture0 = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["dateDeparture"]).ToString("dd.MM.yyyy HH:mm");
                string dateArrival0 = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["dateArrival"]).ToString("dd.MM.yyyy HH:mm");
                string totalCost0 = TransferClass.dtTransferOnHozpitalizationSide1Data.Rows[0]["totalCost"].ToString();

                TransferClass.GetTransferOnHozpitalizationSide0Data(idHospitalization);
                string idTransfer1 = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["ID"].ToString();
                string dateDeparture1 = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["dateDeparture"]).ToString("dd.MM.yyyy HH:mm");
                string dateArrival1 = Convert.ToDateTime(TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["dateArrival"]).ToString("dd.MM.yyyy HH:mm");
                string totalCost1 = TransferClass.dtTransferOnHozpitalizationSide0Data.Rows[0]["totalCost"].ToString();

                HospitalizationDetailClass.GetHospitalizationDetailData(idHospitalization);
                NanniesOnProgramClass.GetHistoryNannyOnProgramDataForPrint(idActualProgram);
                TransferDetailClass.GetTransferDetailsData(idTransfer0, true);
                TransferDetailClass.GetTransferDetailsData(idTransfer1, false);

                var app = new wordActOfCompletedNanny.Application();
                app.Visible = false;
                var doc = app.Documents.Open(documentSampleFolderPath);
                doc.Activate();
                int indx1 = HospitalizationDetailClass.dtHospitalizationDetailData.Rows.Count - 1;
                int indx2 = NanniesOnProgramClass.dtHistoryNannyOnProgramDataForPrint.Rows.Count - 1;
                int indx3 = TransferDetailClass.dtTransferDetailedSide1Data.Rows.Count - 1;
                int indx4 = TransferDetailClass.dtTransferDetailedSide0Data.Rows.Count - 1;
                int rowCountTable1 = 2;
                int rowCountTable2 = 2;
                int rowCountTable3 = 2;
                int rowCountTable4 = 2;
                Object oMissing = System.Reflection.Missing.Value;
                doc.Bookmarks["reportNum"].Range.Text = numReport;
                doc.Bookmarks["curatorFIO"].Range.Text = curatorFIO;
                doc.Bookmarks["curatorPhoneNumber"].Range.Text = curatorPhoneNumber;
                doc.Bookmarks["curatorEmail"].Range.Text = curatorEmail;
                doc.Bookmarks["dateReport"].Range.Text = dateNow;
                doc.Bookmarks["programStart"].Range.Text = dateProgramBegin;
                doc.Bookmarks["programEnd"].Range.Text = dateProgramEnd;
                doc.Bookmarks["ChildFullName"].Range.Text = childrenFIO;
                doc.Bookmarks["ChildBirthdate"].Range.Text = childrenBirthday;
                doc.Bookmarks["RegionName"].Range.Text = childRegionName;
                doc.Bookmarks["OrphanageName"].Range.Text = childOrphanageName;
                doc.Bookmarks["ChildFullName1"].Range.Text = childrenFIO;
                doc.Bookmarks["ChildBirthdate1"].Range.Text = childrenBirthday;
                doc.Bookmarks["OrphanageName1"].Range.Text = childOrphanageName;
                doc.Bookmarks["RegionName1"].Range.Text = childRegionName;
                doc.Bookmarks["CuratorFullName"].Range.Text = curatorFIO;
                doc.Bookmarks["HospitalName"].Range.Text = hospitalName;
                doc.Bookmarks["HospitalAddress"].Range.Text = hospitalAddress;
                doc.Bookmarks["HospitalizationStartDate"].Range.Text = dateHospitalization;
                doc.Bookmarks["HospitalizationEndDate"].Range.Text = dateDischarge;
                doc.Bookmarks["HospitalTotalCost"].Range.Text = hospitalizationTotalCost;

                foreach (DataRow row in HospitalizationDetailClass.dtHospitalizationDetailData.Rows)
                {
                    int indRow = HospitalizationDetailClass.dtHospitalizationDetailData.Rows.IndexOf(row);
                    if (indRow != indx1)
                        doc.Tables[1].Rows.Add(ref oMissing);
                    doc.Tables[1].Rows[rowCountTable1].Cells[2].Range.Text = row["medicalCareType"].ToString();
                    doc.Tables[1].Rows[rowCountTable1].Cells[3].Range.Text = row["cost"].ToString();
                    rowCountTable1++;
                }

                doc.Bookmarks["countNanniesOnProgram"].Range.Text = countNanniesOnProgram;

                foreach (DataRow row in NanniesOnProgramClass.dtHistoryNannyOnProgramDataForPrint.Rows)
                {
                    int indRow = NanniesOnProgramClass.dtHistoryNannyOnProgramDataForPrint.Rows.IndexOf(row);
                    if (indRow != indx2)
                        doc.Tables[2].Rows.Add(ref oMissing);
                    doc.Tables[2].Rows[rowCountTable2].Cells[2].Range.Text = row["fullName"].ToString();
                    doc.Tables[2].Rows[rowCountTable2].Cells[3].Range.Text = row["costPerDay"].ToString();
                    doc.Tables[2].Rows[rowCountTable2].Cells[4].Range.Text = row["countWorkDays"].ToString();
                    doc.Tables[2].Rows[rowCountTable2].Cells[5].Range.Text = row["payment"].ToString();
                    rowCountTable2++;
                }

                doc.Bookmarks["TransferFrom0"].Range.Text = childOrphanageName;
                doc.Bookmarks["TransferTo0"].Range.Text = hospitalName;
                doc.Bookmarks["TransferDeparture0"].Range.Text = dateDeparture0;
                doc.Bookmarks["TransferArrival0"].Range.Text = dateArrival0;
                doc.Bookmarks["TransferTotalCost0"].Range.Text = totalCost0;
                doc.Bookmarks["TransferFrom0_1"].Range.Text = childOrphanageName;
                doc.Bookmarks["TransferTo0_1"].Range.Text = hospitalName;
                doc.Bookmarks["TransferFrom0_2"].Range.Text = childOrphanageName;
                doc.Bookmarks["TransferTo0_2"].Range.Text = hospitalName;

                foreach (DataRow row in TransferDetailClass.dtTransferDetailedSide1Data.Rows)
                {
                    int indRow = TransferDetailClass.dtTransferDetailedSide1Data.Rows.IndexOf(row);
                    if (indRow != indx3)
                        doc.Tables[3].Rows.Add(ref oMissing);
                    doc.Tables[3].Rows[rowCountTable3].Cells[2].Range.Text = row["transportType"].ToString();
                    doc.Tables[3].Rows[rowCountTable3].Cells[3].Range.Text = row["cost"].ToString();
                    rowCountTable3++;
                }

                doc.Bookmarks["TransferFrom1"].Range.Text = hospitalName;
                doc.Bookmarks["TransferTo1"].Range.Text = childOrphanageName;
                doc.Bookmarks["TransferDeparture1"].Range.Text = dateDeparture1;
                doc.Bookmarks["TransferArrival1"].Range.Text = dateArrival1;
                doc.Bookmarks["TransferTotalCost1"].Range.Text = totalCost1;
                doc.Bookmarks["TransferFrom1_1"].Range.Text = hospitalName;
                doc.Bookmarks["TransferTo1_1"].Range.Text = childOrphanageName;
                doc.Bookmarks["TransferFrom1_2"].Range.Text = hospitalName;
                doc.Bookmarks["TransferTo1_2"].Range.Text = childOrphanageName;

                foreach (DataRow row in TransferDetailClass.dtTransferDetailedSide0Data.Rows)
                {
                    int indRow = TransferDetailClass.dtTransferDetailedSide0Data.Rows.IndexOf(row);
                    if (indRow != indx4)
                        doc.Tables[4].Rows.Add(ref oMissing);
                    doc.Tables[4].Rows[rowCountTable4].Cells[2].Range.Text = row["transportType"].ToString();
                    doc.Tables[4].Rows[rowCountTable4].Cells[3].Range.Text = row["cost"].ToString();
                    rowCountTable4++;
                }

                doc.Bookmarks["Conclusion"].Range.Text = conclusion;
                doc.Saved = true;
                try
                {
                    doc.SaveAs2($"{documentSaveFolderPath}.docx");
                    doc.Close();
                    app.Quit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    doc.Close();
                    app.Quit();
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
