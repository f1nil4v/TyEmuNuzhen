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
        public static DataTable dtRegions = new DataTable();
        public static DataTable dtRegionsForEditInfoChildren = new DataTable();

        public static void GetRegionsList()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT * FROM regions ORDER BY regionName";
                dtRegions.Clear();
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
                DBConnection.myCommand.CommandText = "SELECT * FROM regions WHERE ID IN (SELECT idRegion FROM orphanages) ORDER BY regionName";
                dtRegionsForEditInfoChildren.Clear();
                DBConnection.myDataAdapter.Fill(dtRegionsForEditInfoChildren);
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
    }
}
