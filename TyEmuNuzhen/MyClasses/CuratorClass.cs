using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class CuratorClass
    {
        public static string fullNameCurator;

        public static string GetCuratorID(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT ID FROM curators WHERE idUser = '{idUser}'";
                Object resultID = DBConnection.myCommand.ExecuteScalar();
                if (resultID != null)
                {
                    return resultID.ToString();
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

        public static string GetCuratorFullName(string ID)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) FROM curators WHERE ID = '{ID}'";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                {
                    fullNameCurator = result.ToString();
                    return result.ToString();
                }
                else
                {
                    fullNameCurator = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                fullNameCurator = null;
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
