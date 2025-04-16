using System;
using System.Collections.Generic;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ResultConsultationClass
    {
        public static List<string> filePaths = new List<string>();
        public static List<string> oldFilePaths = new List<string>();

        public static bool AddResaultConsultations()
        {
            try
            {
                string idConsultation = "";
                foreach (string filePath in filePaths) 
                {
                    DBConnection.myCommand.Parameters.Clear();
                    DBConnection.myCommand.CommandText = $"INSERT INTO results_consultation VALUES (null, '{idConsultation}', @path)";
                    DBConnection.myCommand.Parameters.AddWithValue("@filePaths", filePath);
                    if (DBConnection.myCommand.ExecuteNonQuery() <= 0)
                        return false;
                }
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
