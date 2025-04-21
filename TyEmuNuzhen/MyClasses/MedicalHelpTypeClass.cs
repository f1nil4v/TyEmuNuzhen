using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class MedicalHelpTypeClass
    {
        public static DataTable dtMedicalHelpTypeS;

        public static void GetMedicalHelpTypesList(string querySearch)
        {
            try
            {
                string whereClause = querySearch != "" ? $"WHERE medicalCareType LIKE @querySearch" : "";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT ID, medicalCareType FROM medical_care_type {whereClause}";
                if (whereClause != "")
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtMedicalHelpTypeS = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMedicalHelpTypeS);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetMedicalHelpTypeName(string idMedicalHelpType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT medicalCareType FROM medical_care_type WHERE ID = '{idMedicalHelpType}'";
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

        public static string GetCountAllMedicalHelpTypes()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_care_type";
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

        public static bool GetSameMedicalHelpType(string idMedicalHelpType, string medicalHelpType)
        {
            try
            {
                string whereClause = idMedicalHelpType == null ? "" : $"AND ID <> '{idMedicalHelpType}'";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_care_type WHERE medicalCareType = @medicalHelpType {whereClause}";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalHelpType", medicalHelpType);
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

        public static bool AddMedicalHelpType(string medicalHelpType)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO medical_care_type VALUES (null, @medicalHelpType)";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalHelpType", medicalHelpType);
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

        public static bool UpdateMedicalHelpType(string idMedicalHelpType, string medicalHelpType)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE medical_care_type SET medicalCareType = @medicalHelpType WHERE ID = '{idMedicalHelpType}'";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalHelpType", medicalHelpType);
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

        public static bool DeleteMedicalHelpType(string idMedicalHelpType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM medical_care_type WHERE ID = '{idMedicalHelpType}'";
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
