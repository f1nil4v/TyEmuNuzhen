using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class VolonteerClass
    {
        public static string idRegion;

        public static string GetVolonteerID(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT ID FROM volunteers WHERE idUser = '{idUser}'";
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

        public static string GetVolonteerRegion(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT idRegion FROM volunteers WHERE idUser = '{idUser}'";
                Object resultIDRegion = DBConnection.myCommand.ExecuteScalar();
                if (resultIDRegion != null)
                {
                    idRegion = resultIDRegion.ToString();
                    return resultIDRegion.ToString();
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
    }
}
