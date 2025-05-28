using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с ролями пользователей в системе
    /// </summary>
    internal class RolesClass
    {
        public static DataTable dtCuratoRoles;

        /// <summary>
        /// Метод для получения ролей кураторов из базы данных
        /// </summary>
        public static void GetCuratorRoles()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT * FROM roles WHERE ID NOT IN (1, 3)";
                dtCuratoRoles = new DataTable();
                DBConnection.myDataAdapter.Fill(dtCuratoRoles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Метод для получения названия роли
        /// </summary>
        /// <returns></returns>
        public static string GetRole()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT roleName FROM roles 
                                                        WHERE ID = '{AuthorizationClass.idRole}'";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
