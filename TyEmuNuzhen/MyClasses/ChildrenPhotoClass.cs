using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с фотографиями детей
    /// </summary>
    internal class ChildrenPhotoClass
    {
        public static DataTable dtMonitoringPhoto;

        /// <summary>
        /// Получение фотографий детей по ID ребенка
        /// </summary>
        /// <param name="idChild"></param>
        public static void GetMonitoringPhotoChildren(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM childphoto 
                    WHERE childphoto.idChild = '{idChild}'
                    ORDER BY childphoto.ID DESC";
                dtMonitoringPhoto = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMonitoringPhoto);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавление фотографии ребенка в базу данных
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="urlPhoto"></param>
        /// <returns></returns>
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
