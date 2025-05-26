using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.RightsManagement;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class MedicalFacilityClass
    {
        public static DataTable dtMedicalFacilityForComboBoxList;
        public static DataTable dtMedicalFacilityList;
        public static DataTable dtMedicalFacilityData;

        public static void GetMedicalFacilityForComboBoxList()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT ID, medicalFacilityName FROM medical_facility ORDER BY medicalFacilityName";
                dtMedicalFacilityForComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMedicalFacilityForComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetMedicalFacilityList(string querySearch)
        {
            try
            {
                string whereClause = String.IsNullOrEmpty(querySearch) ? "" : "WHERE medicalFacilityName LIKE @querySearch OR address LIKE @querySearch";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT * FROM medical_facility {whereClause}";
                if (!String.IsNullOrEmpty(querySearch))
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtMedicalFacilityList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMedicalFacilityList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetMedicalFacilityData(string id)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM medical_facility WHERE ID = '{id}'";
                dtMedicalFacilityData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMedicalFacilityData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool GetSameMedicalFacility(string medicalFacilityName, string address)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_facility WHERE medicalFacilityName = @medicalFacilityName AND address = @address";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalFacilityName", medicalFacilityName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) > 0)
                {
                    MessageBox.Show("Данное медицинское учреждение уже есть в системе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_facility WHERE address = @address";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalFacilityName", medicalFacilityName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
                result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) > 0)
                {
                    MessageBox.Show("Данный адрес занят другим медицинским учреждением!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public static bool GetSameMedicalFacility(string id, string medicalFacilityName, string address)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_facility WHERE medicalFacilityName = @medicalFacilityName AND address = @address AND ID <> '{id}'";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalFacilityName", medicalFacilityName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) > 0)
                {
                    MessageBox.Show("Данное медицинское учреждение уже есть в системе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_facility WHERE address = @address AND ID <> '{id}'";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalFacilityName", medicalFacilityName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
                result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) > 0)
                {
                    MessageBox.Show("Данный адрес занят другим медицинским учреждением!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        public static string GetCountAllMedicalFacility()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM medical_facility";
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

        public static bool AddMedicalFacility(string medicalFacilityName, string address)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO medical_facility VALUES (null, @medicalFacilityName, @address)";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalFacilityName", medicalFacilityName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
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

        public static bool UpdateMedicalFacility(string id, string medicalFacilityName, string address)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE medical_facility SET medicalFacilityName = @medicalFacilityName, address = @address WHERE ID = '{id}'";
                DBConnection.myCommand.Parameters.AddWithValue("@medicalFacilityName", medicalFacilityName);
                DBConnection.myCommand.Parameters.AddWithValue("@address", address);
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

        public static bool DeleteMedicalFacility(string id)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM medical_facility WHERE ID = '{id}'";
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
