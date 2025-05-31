using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с волонтёрами в базе данных.
    /// </summary>
    internal class VolonteerClass
    {
        public static string idRegion;
        public static DataTable dtVolonteersList;
        public static DataTable dtVolonteerDataList;

        /// <summary>
        /// Получение ID волонтёра по ID пользователя.
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение полного имени волонтёра по его ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetVolonteerFullName(string ID)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) FROM volunteers WHERE ID = '{ID}'";
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
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Получение региона волонтёра по ID пользователя.
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение списка волонтёров из базы данных с возможностью фильтрации и сортировки
        /// </summary>
        /// <param name="querySearch"></param>
        /// <param name="idRegion"></param>
        /// <param name="orderByValue"></param>
        public static void GetVolonteersList(string querySearch, string idRegion, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (volunteers.surname LIKE @searchQuery OR volunteers.name LIKE @searchQuery OR volunteers.middleName LIKE @searchQuery 
                        OR volunteers.phoneNumber LIKE @searchQuery OR volunteers.email LIKE @searchQuery OR users.login LIKE @searchQuery
                        OR CONCAT_WS(' ', volunteers.surname, volunteers.name, IFNULL(volunteers.middleName, '') LIKE @searchQuery))";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT volunteers.ID, users.login, volunteers.surname, volunteers.name, IFNULL(volunteers.middleName, '-') as 'middleName',
                                            volunteers.phoneNumber, volunteers.email, regions.regionName, volunteers.idUser
                                        FROM volunteers, users, regions
                                        WHERE users.ID = volunteers.idUser AND regions.ID = volunteers.idRegion AND volunteers.idRegion = '{idRegion}' {whereClause}
                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtVolonteersList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtVolonteersList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных волонтёра по его ID для отображения в форме редактирования или просмотра.
        /// </summary>
        /// <param name="idVolonteer"></param>
        public static void GetVolonteerData(string idVolonteer)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT volunteers.ID, users.login, volunteers.surname, volunteers.name, IFNULL(volunteers.middleName, '-') as 'middleName',
                                            volunteers.phoneNumber, volunteers.email, regions.regionName, volunteers.idUser
                                        FROM volunteers, regions, users
                                        WHERE users.ID = volunteers.idUser AND regions.ID = volunteers.idRegion AND volunteers.ID = '{idVolonteer}'";
                dtVolonteerDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtVolonteerDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение общего количества волонтёров в базе данных.
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllVolonteers()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM volunteers";
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
        /// Добавление нового волонтёра в базу данных с указанием его данных и региона.
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="idRegion"></param>
        /// <returns></returns>
        public static bool AddVolonteer(string surname, string name, string middleName, string phoneNumber, string email, string idRegion)
        {
            try
            {
                string idUser = UserClass.GetLastUserID();
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO volunteers VALUES (null, @surname, @name, @middleName, @phoneNumber, @email, '{idRegion}', '{idUser}')";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                DBConnection.myCommand.Parameters.AddWithValue("@email", email);
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

        /// <summary>
        /// Обновление данных волонтёра в базе данных по его ID с указанием новых данных и региона.
        /// </summary>
        /// <param name="idVolonteer"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="idRegion"></param>
        /// <returns></returns>
        public static bool UpdateVolonteer(string idVolonteer, string surname, string name, string middleName, string phoneNumber, string email, string idRegion)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE volunteers SET surname = @surname, name = @name, middleName = @middleName, 
                                                            phoneNumber = @phoneNumber, email = @email, idregion = '{idRegion}'
                                                        WHERE ID = '{idVolonteer}'";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                DBConnection.myCommand.Parameters.AddWithValue("@email", email);
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

        /// <summary>
        /// Удаление волонтёра из базы данных
        /// </summary>
        /// <param name="idVolonteer"></param>
        /// <returns></returns>
        public static bool DeleteVolonteer(string idVolonteer)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM volunteers WHERE ID = '{idVolonteer}'";
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
