using MySql.Data.MySqlClient;
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
    /// Класс для работы с должностями врачей
    /// </summary>
    internal class DoctorPostsClass
    {
        public static DataTable dtDoctorPostsList;
        public static DataTable dtDoctorPostsSList;

        /// <summary>
        /// Получение списка должностей врачей
        /// </summary>
        public static void GetDoctorPostsList()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT ID, postName FROM doctor_posts ORDER BY postName";
                dtDoctorPostsList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorPostsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка должностей врачей с возможностью поиска по имени должности
        /// </summary>
        /// <param name="querySearch"></param>
        public static void GetDoctorPostsList(string querySearch)
        {
            try
            {
                string whereClause = querySearch != "" ? $"WHERE postName LIKE @querySearch" : "";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT ID, postName FROM doctor_posts {whereClause}";
                if (whereClause != "")
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtDoctorPostsSList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorPostsSList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение имени должности врача по ID
        /// </summary>
        /// <param name="idPost"></param>
        /// <returns></returns>
        public static string GetDoctorPostName(string idPost)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT postName FROM doctor_posts WHERE ID = '{idPost}'";
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

        /// <summary>
        /// Получение количества всех должностей врачей
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllDoctorPosts()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM doctor_posts";
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

        /// <summary>
        /// Добавление новой должности врача
        /// </summary>
        /// <param name="postName"></param>
        /// <returns></returns>
        public static bool AddDoctorPost(string postName)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO doctor_posts VALUES (null, @postName)";
                DBConnection.myCommand.Parameters.AddWithValue("@postName", postName);
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show($"Запись с таким значением уже есть в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Обновление должности врача
        /// </summary>
        /// <param name="idPost"></param>
        /// <param name="postName"></param>
        /// <returns></returns>
        public static bool UpdateDoctorPost(string idPost, string postName)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE doctor_posts SET postName = @postName WHERE ID = '{idPost}'";
                DBConnection.myCommand.Parameters.AddWithValue("@postName", postName);
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show($"Запись с таким значением уже есть в системе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Удаление должности врача
        /// </summary>
        /// <param name="idPost"></param>
        /// <returns></returns>
        public static bool DeleteDoctorPost(string idPost)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM doctor_posts WHERE ID = '{idPost}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    MessageBox.Show($"Запись не может быть удалена, так как она используется в других таблицах.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
