using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class RolesClass
    {
        public static DataTable dtCuratoRoles;

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
