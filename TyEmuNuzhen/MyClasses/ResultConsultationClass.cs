using System;
using System.Collections.Generic;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ResultConsultationClass
    {
        public static List<string> oldFilePaths = new List<string>();

        public static bool AddResaultConsultations(string idChild)
        {
            try
            {
                string idConsultation = ConsultationClass.GetLastIdConsultation();
                foreach (string filePath in oldFilePaths) 
                {
                    string newFilePath = CopyFilesClass.CopyChildMedicalResults(filePath, idChild);
                    DBConnection.myCommand.Parameters.Clear();
                    DBConnection.myCommand.CommandText = $"INSERT INTO results_consultation VALUES (null, '{idConsultation}', @filePaths)";
                    DBConnection.myCommand.Parameters.AddWithValue("@filePaths", newFilePath);
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
