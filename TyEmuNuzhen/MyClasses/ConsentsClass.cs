using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы обращениями детского дома
    /// </summary>
    internal class ConsentsClass
    {
        /// <summary>
        /// Получение максимального номера обращения детского дома
        /// </summary>
        /// <returns></returns>
        public static string GetMaxNumAppealOrphanage()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(numAppeal) FROM consents";
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
        /// Получение пути к файлу обращения детского дома по ID программы
        /// </summary>
        /// <param name="idActualProgram"></param>
        /// <returns></returns>
        public static string GetFilePathAppealConsent(string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT children_documents.filePath FROM children_documents, consents WHERE children_documents.ID = consents.idDocument AND consents.idActualProgram = '{idActualProgram}'";
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
        /// Добавление обращения детского дома
        /// </summary>
        /// <param name="numAppeal"></param>
        /// <param name="idOrphanage"></param>
        /// <param name="idActualProgram"></param>
        /// <returns></returns>
        public static bool AddChildrenAppealConsent(string numAppeal, string idOrphanage, string idActualProgram)
        {
            try
            {
                string idDocument = ChildrenDocumentClass.GetLastDocumentID();
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                DBConnection.myCommand.CommandText = $@"INSERT INTO consents 
                    VALUES (null, '{numAppeal}', '{dateNow}', '{idOrphanage}', '{idDocument}', '{idActualProgram}')";

                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
