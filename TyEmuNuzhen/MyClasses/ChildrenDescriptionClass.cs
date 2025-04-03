using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrenDescriptionClass
    {
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
