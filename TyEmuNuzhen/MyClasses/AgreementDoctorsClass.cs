using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с договорамми врачей
    /// </summary>
    internal class AgreementDoctorsClass
    {
        public static DataTable dtAgreementDoctorData;
        /// <summary>
        /// Получение списка договоров врачей по ID врача
        /// </summary>
        /// <param name="idDoctor"></param>
        public static void GetAgreementDoctorData(string idDoctor)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM agreement_doctors 
                                                        WHERE idDoctor = '{idDoctor}' ORDER BY ID DESC ";
                dtAgreementDoctorData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtAgreementDoctorData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение максимального номера договора врача
        /// </summary>
        /// <returns></returns>
        public static string GetMaxNumAgreementDoctor()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(numOfAgreement) FROM agreement_doctors";
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
        /// Добавление нового договора врача в БД
        /// </summary>
        /// <param name="numAgreement"></param>
        /// <param name="idDoctor"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool AddAgreementDoctor(string numAgreement, string idDoctor, string filePath)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                DBConnection.myCommand.CommandText =
                    $"INSERT INTO agreement_doctors VALUES (null, '{numAgreement}', '{dateNow}', '{idDoctor}', '{filePath}')";
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
