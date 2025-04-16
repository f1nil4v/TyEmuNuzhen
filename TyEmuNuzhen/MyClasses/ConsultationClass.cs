using System;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ConsultationClass
    {
        public static bool AddChildrenConsultation(string idDoctor, string idChild, string filePath)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO consultation VALUES (null, '{idDoctor}', '{idChild}', @path)";
                DBConnection.myCommand.Parameters.AddWithValue("@path", filePath);
                if (DBConnection.myCommand.ExecuteNonQuery() < 0)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static string GetLastIdConsultation()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(ID) FROM consultation";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
