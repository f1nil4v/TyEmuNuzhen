using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class RegionsClass
    {
        public static DataTable dtRegions;
        public static DataTable dtRegionsForEditInfoChildren;
        public static DataTable dtRegionsS;

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

        public static bool GetSameRegion(string idRegion, string regionName)
        {
            try
            {
                string whereClause = idRegion == null ? "" : $"AND ID <> '{idRegion}'";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM regions WHERE regionName = @regionName {whereClause}";
                DBConnection.myCommand.Parameters.AddWithValue("@regionName", regionName);
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
