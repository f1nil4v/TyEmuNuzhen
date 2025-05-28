using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с соглашениями о социальном партнёрстве с детскими домами.
    /// </summary>
    internal class AgreementOrphanagesClass
    {
        public static DataTable dtAgreementOrphanageData;

        /// <summary>
        /// Получение данных соглашений о социальном партнёрстве с детскими домами.
        /// </summary>
        /// <param name="idOrphanage"></param>
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

        /// <summary>
        /// Получение данных соглашения о социальном партнёрстве с детским домом для печати.
        /// </summary>
        /// <param name="idOrphanage"></param>
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

        /// <summary>
        /// Получение максимального номера соглашения о социальном партнёрстве с детским домом.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Добавление соглашения о социальном партнёрстве с детским домом в базу данных.
        /// </summary>
        /// <param name="numAgreement"></param>
        /// <param name="idOrphanage"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
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
