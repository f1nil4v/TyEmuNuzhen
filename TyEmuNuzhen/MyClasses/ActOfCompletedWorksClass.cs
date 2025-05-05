using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ActOfCompletedWorksClass
    {
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
