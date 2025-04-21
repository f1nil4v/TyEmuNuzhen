using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class TransportTypesClass
    {
        public static DataTable dtTranposrtTypeS;

        public static void GetTranposrtTypesList(string querySearch)
        {
            try
            {
                string whereClause = querySearch != "" ? $"WHERE transportType LIKE @querySearch" : "";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT ID, transportType FROM transport_type {whereClause}";
                if (whereClause != "")
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtTranposrtTypeS = new DataTable();
                DBConnection.myDataAdapter.Fill(dtTranposrtTypeS);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetTranposrtTypeName(string idTransportType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT transportType FROM transport_type WHERE ID = '{idTransportType}'";
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

        public static string GetCountAllTranposrtTypes()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM transport_type";
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

        public static bool GetSameTranposrtType(string idTransportType, string transportType)
        {
            try
            {
                string whereClause = idTransportType == null ? "" : $"AND ID <> '{idTransportType}'";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM transport_type WHERE transportType = @transportType {whereClause}";
                DBConnection.myCommand.Parameters.AddWithValue("@transportType", transportType);
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

        public static bool AddTranposrtType(string transportType)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO transport_type VALUES (null, @transportType)";
                DBConnection.myCommand.Parameters.AddWithValue("@transportType", transportType);
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

        public static bool UpdateTranposrtType(string idTransportType, string transportType)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE transport_type SET transportType = @transportType WHERE ID = '{idTransportType}'";
                DBConnection.myCommand.Parameters.AddWithValue("@transportType", transportType);
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

        public static bool DeleteTranposrtType(string idTransportType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM transport_type WHERE ID = '{idTransportType}'";
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
