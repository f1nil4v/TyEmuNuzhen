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
                DBConnection.myCommand.CommandText = $"INSERT INTO consultations VALUES (null, '{idDoctor}', '{idChild}', @path)";
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
    }
}
