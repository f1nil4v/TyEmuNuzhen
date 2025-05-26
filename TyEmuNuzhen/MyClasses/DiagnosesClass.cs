using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Documents;

namespace TyEmuNuzhen.MyClasses
{
    internal class DiagnosesClass
    {
        public static DataTable dtDiagnosesForChildrenComboBoxList;
        public static DataTable dtDiagnosesList;

        public static List<string> selectedIDDiagnoses = new List<string>();
        public static List<string> selectedDiagnoses = new List<string>();

        public static void GetDiagnosesForChildrenComboBoxList(string idChild)
        {
            try
            {
                string queryParam = String.Join(",", selectedIDDiagnoses);
                if (string.IsNullOrEmpty(queryParam))
                    DBConnection.myCommand.CommandText = $@"SELECT * FROM diagnoses";
                else
                    DBConnection.myCommand.CommandText = $@"SELECT * FROM diagnoses WHERE ID NOT IN ({queryParam})";
                dtDiagnosesForChildrenComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDiagnosesForChildrenComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetDiagnosesList(string querySearch)
        {
            try
            {
                string whereClause = querySearch != "" ? $"WHERE diagnosisName LIKE @querySearch" : "";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT * FROM diagnoses {whereClause}";
                if (whereClause != "")
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtDiagnosesList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDiagnosesList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetDiagnosisName(string idDiagnoses)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT diagnosisName FROM diagnoses WHERE ID = '{idDiagnoses}'";
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

        public static string GetCountAllDiagnoses()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM diagnoses";
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

        public static bool AddDiagnoses(string diagnosesName)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO diagnoses VALUES (null, @diagnosisName)";
                DBConnection.myCommand.Parameters.AddWithValue("@diagnosisName", diagnosesName);
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

        public static bool UpdateDiagnoses(string idDiagnoses, string diagnosesName)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE diagnoses SET diagnosisName = @diagnosisName WHERE ID = '{idDiagnoses}'";
                DBConnection.myCommand.Parameters.AddWithValue("@diagnosisName", diagnosesName);
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

        public static bool DeleteDiagnoses(string idDiagnoses)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM diagnoses WHERE ID = '{idDiagnoses}'";
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
