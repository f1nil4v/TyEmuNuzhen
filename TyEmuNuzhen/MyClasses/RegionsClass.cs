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
    /// Класс для работы с регионами
    /// </summary>
    internal class RegionsClass
    {
        public static DataTable dtRegions;
        public static DataTable dtRegionsForEditInfoChildren;
        public static DataTable dtRegionsS;

        /// <summary>
        /// Получение списка регионов
        /// </summary>
        public static void GetRegionsList()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT ID, regionName FROM regions ORDER BY regionName";
                dtRegions = new DataTable();
                DBConnection.myDataAdapter.Fill(dtRegions);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка регионов для редактирования информации о детях
        /// </summary>
        public static void GetRegionsListForEditInfoChildren()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT ID, regionName FROM regions WHERE ID IN (SELECT DISTINCT idRegion FROM orphanages) ORDER BY regionName";
                dtRegionsForEditInfoChildren = new DataTable();
                DBConnection.myDataAdapter.Fill(dtRegionsForEditInfoChildren);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка регионов с возможностью поиска по имени региона
        /// </summary>
        /// <param name="querySearch"></param>
        public static void GetRegionsList(string querySearch)
        {
            try
            {
                string whereClause = querySearch != "" ? $"WHERE regionName LIKE @querySearch" : "";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT ID, regionName FROM regions {whereClause}";
                if (whereClause != "")
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtRegionsS = new DataTable();
                DBConnection.myDataAdapter.Fill(dtRegionsS);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение имени региона по его ID
        /// </summary>
        /// <param name="idRegion"></param>
        /// <returns></returns>
        public static string GetRegionName(string idRegion)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT regionName FROM regions WHERE ID = '{idRegion}'";
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
        /// Получение количества всех регионов в базе данных
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllRegions()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM regions";
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
        /// Добавление нового региона в базу данных
        /// </summary>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public static bool AddRegion(string regionName)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO regions VALUES (null, @regionName)";
                DBConnection.myCommand.Parameters.AddWithValue("@regionName", regionName);
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
        /// Обновление информации о регионе
        /// </summary>
        /// <param name="idRegion"></param>
        /// <param name="regionName"></param>
        /// <returns></returns>
        public static bool UpdateRegion(string idRegion, string regionName)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE regions SET regionName = @regionName WHERE ID = '{idRegion}'";
                DBConnection.myCommand.Parameters.AddWithValue("@regionName", regionName);
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
                    MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// Удаление региона
        /// </summary>
        /// <param name="idRegion"></param>
        /// <returns></returns>
        public static bool DeleteRegion(string idRegion)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM regions WHERE ID = '{idRegion}'";
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
