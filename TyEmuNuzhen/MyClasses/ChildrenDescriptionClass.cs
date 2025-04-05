using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrenDescriptionClass
    {
        public static DataTable dtMonitoringDescription = new DataTable();

        public static void GetMonitoringDescriptionChildren(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM childrens_description 
                    WHERE childrens_description.idChild = '{idChild}' ORDER BY ID DESC";
                dtMonitoringDescription.Clear();
                DBConnection.myDataAdapter.Fill(dtMonitoringDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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
