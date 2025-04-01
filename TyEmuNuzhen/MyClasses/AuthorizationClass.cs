using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class AuthorizationClass
    {
        public static string Authorization(string login, string password)
        {
            try
            {
                string query = "SELECT idRole FROM users WHERE login = @login AND password = @password";
                DBConnection.myCommand.CommandText = query;
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.Parameters.AddWithValue("@login", login);
                DBConnection.myCommand.Parameters.AddWithValue("@password", password);
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                    return result.ToString();
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при авторизации. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
