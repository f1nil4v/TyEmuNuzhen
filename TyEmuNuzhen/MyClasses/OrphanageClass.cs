using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с данными детских домов
    /// </summary>
    internal class OrphanageClass
    {
        public static DataTable dtOrphanagesForComboBoxList;
        public static DataTable dtOrphanageDataForPrintDocuments;
        public static DataTable dtOrphanagesList;
        public static DataTable dtOrphanageDataList;

        /// <summary>
        /// Получение списка детских домов для выпадающего списка
        /// </summary>
        /// <param name="idRegion"></param>
        public static void GetOrphanagesForComboBoxList(string idRegion)
        {
            try
            {
                string whereClause = string.IsNullOrEmpty(idRegion) ? "" : $" WHERE idRegion = '{idRegion}'";
                DBConnection.myCommand.CommandText = $@"SELECT ID, nameOrphanage FROM orphanages 
                                                        {whereClause}
                                                        ORDER BY nameOrphanage";
                dtOrphanagesForComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtOrphanagesForComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных детского дома для печати
        /// </summary>
        /// <param name="idOrphanage"></param>
        public static void GetOrphanageDataForPrintDocuments(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM orphanages 
                                                        WHERE ID = '{idOrphanage}'";
                dtOrphanageDataForPrintDocuments = new DataTable();
                DBConnection.myDataAdapter.Fill(dtOrphanageDataForPrintDocuments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка детских домов с возможностью поиска и сортировки
        /// </summary>
        /// <param name="querySearch"></param>
        /// <param name="idRegion"></param>
        /// <param name="orderByValue"></param>
        public static void GetOrphanagesList(string querySearch, string idRegion, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (orphanages.nameOrphanage LIKE @searchQuery OR orphanages.address LIKE @searchQuery OR orphanages.email LIKE @searchQuery 
                        OR orphanages.directorSurname LIKE @searchQuery OR orphanages.directorName LIKE @searchQuery OR orphanages.directorMiddleName LIKE @searchQuery
                        OR CONCAT_WS(' ', orphanages.directorSurname, orphanages.directorName, IFNULL(orphanages.directorMiddleName, '')) LIKE @searchQuery)";
                whereClause += string.IsNullOrEmpty(idRegion) ? "" : $" AND orphanages.idRegion = '{idRegion}'";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT orphanages.ID, orphanages.nameOrphanage, CONCAT_WS(' ', orphanages.directorSurname, orphanages.directorName, IFNULL(orphanages.directorMiddleName, '')) as 'directorFullName',
                                            regions.regionName, orphanages.address, orphanages.email
                                        FROM orphanages, regions
                                        WHERE regions.ID = orphanages.idRegion {whereClause}
                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtOrphanagesList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtOrphanagesList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных детского дома по его ID
        /// </summary>
        /// <param name="idOrphanage"></param>
        public static void GetOrphanageData(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT orphanages.ID, orphanages.nameOrphanage, orphanages.directorSurname, orphanages.directorName, IFNULL(orphanages.directorMiddleName, '-') as 'middleName',
                                            orphanages.idRegion, orphanages.email, orphanages.address
                                        FROM orphanages 
                                        WHERE ID = '{idOrphanage}'";
                dtOrphanageDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtOrphanageDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение количества всех детских домов
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllOrphanages()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM orphanages";
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
        /// Получение максимального ID детского дома
        /// </summary>
        /// <returns></returns>
        public static string GetMaxIdOrphanage()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(ID) FROM orphanages";
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
        /// Проверка на уникальность адреса детского дома
        /// </summary>
        /// <param name="idRegion"></param>
        /// <param name="address"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool GetSameDataOrphanage(string idRegion, string address, string id = null)
        {
            try
            {
                string whereClause = String.IsNullOrEmpty(id) ? "" : $"AND ID <> '{id}'";
                DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM orphanages WHERE idRegion = '{idRegion}' AND address = '{address}' {whereClause}";
                if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Детский дом по данному адресу уже есть в системе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Добавление нового детского дома
        /// </summary>
        /// <param name="nameOrphanage"></param>
        /// <param name="directorSurname"></param>
        /// <param name="directorName"></param>
        /// <param name="directorMiddleName"></param>
        /// <param name="idRegion"></param>
        /// <param name="address"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool AddOrphanage(string nameOrphanage, string directorSurname, string directorName, string directorMiddleName, string idRegion, string address, string email)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO orphanages VALUES (null, @nameOrphanage, @directorSurname, @directorName, @directorMiddleName, '{idRegion}', @address, @email)";
                DBConnection.myCommand.Parameters.AddWithValue("@nameOrphanage", nameOrphanage);
                DBConnection.myCommand.Parameters.AddWithValue("@directorSurname", directorSurname);
                DBConnection.myCommand.Parameters.AddWithValue("@directorName", directorName);
                DBConnection.myCommand.Parameters.AddWithValue("@directorMiddleName", directorMiddleName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
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
        /// Обновление данных детского дома
        /// </summary>
        /// <param name="idOrphanage"></param>
        /// <param name="nameOrphanage"></param>
        /// <param name="directorSurname"></param>
        /// <param name="directorName"></param>
        /// <param name="directorMiddleName"></param>
        /// <param name="idRegion"></param>
        /// <param name="address"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool UpdateOrphanage(string idOrphanage, string nameOrphanage, string directorSurname, string directorName, string directorMiddleName, string idRegion, string address, string email)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE orphanages SET nameOrphanage = @nameOrphanage, directorSurname = @directorSurname, directorName = @directorName, 
                                                            directorMiddleName = @directorMiddleName, idRegion = '{idRegion}', address = @address, email = @email
                                                        WHERE ID = '{idOrphanage}'";
                DBConnection.myCommand.Parameters.AddWithValue("@nameOrphanage", nameOrphanage);
                DBConnection.myCommand.Parameters.AddWithValue("@directorSurname", directorSurname);
                DBConnection.myCommand.Parameters.AddWithValue("@directorName", directorName);
                DBConnection.myCommand.Parameters.AddWithValue("@directorMiddleName", directorMiddleName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
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
        /// Удаление детского дома
        /// </summary>
        /// <param name="idOrphanage"></param>
        /// <returns></returns>
        public static bool DeleteOrphanage(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM orphanages WHERE ID = '{idOrphanage}'";
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
