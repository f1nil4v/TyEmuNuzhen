using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class UserClass
    {
        public static string GetLastUserID()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT MAX(ID) FROM users";
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

        public static bool AddUser(string login, string password, string idRole)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO users VALUES (null, @login, MD5(@password), '{idRole}')";
                DBConnection.myCommand.Parameters.AddWithValue("@login", login);
                DBConnection.myCommand.Parameters.AddWithValue("@password", password);
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

        public static bool UpdateUser(string idUser, string password)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string queryUpdateValue = String.IsNullOrEmpty(password) ? "" : $@"password = MD5(@password)";
                if (String.IsNullOrEmpty(queryUpdateValue))
                    return true;
                DBConnection.myCommand.CommandText = $"UPDATE users SET {queryUpdateValue} WHERE ID = '{idUser}'";
                DBConnection.myCommand.Parameters.AddWithValue("@password", password);
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool UpdateUser(string idUser, string password, string idRole)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string queryUpdateValue = String.IsNullOrEmpty(password) ? "" : $@"password = MD5(@password),";
                DBConnection.myCommand.CommandText = $"UPDATE users SET {queryUpdateValue} idRole = '{idRole}' WHERE ID = '{idUser}'";
                DBConnection.myCommand.Parameters.AddWithValue("@password", password);
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool DeleteUser(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM users WHERE ID = '{idUser}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
