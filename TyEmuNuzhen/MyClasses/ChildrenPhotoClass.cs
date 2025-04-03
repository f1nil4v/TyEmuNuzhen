using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrenPhotoClass
    {
        public static bool AddMonitoringPhotoChildren(string idChild, string urlPhoto)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = @"INSERT INTO childphoto 
                    VALUES (null, @idChild, @urlPhoto, @dateNow)";
                
                DBConnection.myCommand.Parameters.AddWithValue("@idChild", idChild);
                DBConnection.myCommand.Parameters.AddWithValue("@urlPhoto", urlPhoto);
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
