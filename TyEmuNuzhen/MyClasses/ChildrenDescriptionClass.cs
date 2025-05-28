using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с описаниями детей
    /// </summary>
    internal class ChildrenDescriptionClass
    {
        public static DataTable dtMonitoringDescription;

        /// <summary>
        /// Получение описаний детей по ID ребенка
        /// </summary>
        /// <param name="idChild"></param>
        public static void GetMonitoringDescriptionChildren(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM childrens_description 
                    WHERE childrens_description.idChild = '{idChild}' ORDER BY ID DESC";
                dtMonitoringDescription = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMonitoringDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавление описания ребенка в базу данных
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool AddMonitoringDescriptionChildren(string idChild, string description)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = @"INSERT INTO childrens_description 
                    VALUES (null, @idChild, @description, @dateNow)";
                
                DBConnection.myCommand.Parameters.AddWithValue("@idChild", idChild);
                DBConnection.myCommand.Parameters.AddWithValue("@description", description);
                DBConnection.myCommand.Parameters.AddWithValue("@dateNow", dateNow);
                
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
