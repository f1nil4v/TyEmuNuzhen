using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с результатами консультаций
    /// </summary>
    internal class ResultConsultationClass
    {
        public static List<string> oldFilePaths = new List<string>();
        public static DataTable dtResultConsultationList;
        /// <summary>
        /// Получение списка результатов консультаций по ID консультации
        /// </summary>
        /// <param name="idConsultation"></param>
        public static void GetResultConsultation(string idConsultation)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT filePath
                    FROM results_consultation
                    WHERE idConsiltation = '{idConsultation}'";
                dtResultConsultationList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtResultConsultationList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавление результатов консультации для ребенка
        /// </summary>
        /// <param name="idChild"></param>
        /// <returns></returns>
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
