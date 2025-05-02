using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class AgreementOrphanagesClass
    {
        public static DataTable dtAgreementOrphanageData;

        public static void GetAgreementOrphanageData(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM agreement_orphanage 
                                                        WHERE idOrphanage = '{idOrphanage}' ORDER BY ID DESC ";
                dtAgreementOrphanageData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtAgreementOrphanageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetAgreementOrphanageDataForPrint(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM agreement_orphanage 
                                                        WHERE idOrphanage = '{idOrphanage}' ORDER BY ID DESC LIMIT 1";
                dtAgreementOrphanageData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtAgreementOrphanageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetMaxNumAgreementOrphanage()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(numAgreement) FROM agreement_orphanage";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static bool AddAgreementOrphanage(string numAgreement, string idOrphanage, string filePath)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                DBConnection.myCommand.CommandText =
                    $"INSERT INTO agreement_orphanage VALUES (null, '{numAgreement}', '{dateNow}', '{idOrphanage}', '{filePath}')";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

    }
}
