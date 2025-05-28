using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с актами выполненных работ.
    /// </summary>
    internal class ActOfCompletedWorksClass
    {
        /// <summary>
        /// Получение максимального номера акта выполненных работ.
        /// </summary>
        /// <returns></returns>
        public static string GetMaxNumActOfCompletedWorksNanny()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(numOfAct) FROM act_of_completed_works";
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
        /// Добавление акта выполненных работ в базу данных.
        /// </summary>
        /// <param name="numOfAct"></param>
        /// <param name="idNannyOnProgram"></param>
        /// <param name="countWorkDays"></param>
        /// <param name="payment"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool AddActOfCompletedWorks(string numOfAct, string idNannyOnProgram, string countWorkDays, string payment, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText =
                    $"INSERT INTO act_of_completed_works VALUES (null, '{numOfAct}', '{idNannyOnProgram}', '{countWorkDays}', '{payment}', '{filePath}')";
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
