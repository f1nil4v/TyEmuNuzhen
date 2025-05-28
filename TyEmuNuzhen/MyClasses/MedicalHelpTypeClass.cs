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
    /// Класс для работы с типами медицинской помощи
    /// </summary>
    internal class MedicalHelpTypeClass
    {
        public static DataTable dtMedicalHelpTypeS;

        /// <summary>
        /// Получение списка типов медицинской помощи
        /// </summary>
        /// <param name="querySearch"></param>
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

        /// <summary>
        /// Получение названия типа медицинской помощи по его ID
        /// </summary>
        /// <param name="idMedicalHelpType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение количества всех типов медицинской помощи
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Добавление нового типа медицинской помощи
        /// </summary>
        /// <param name="medicalHelpType"></param>
        /// <returns></returns>
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
        /// Обновление типа медицинской помощи
        /// </summary>
        /// <param name="idMedicalHelpType"></param>
        /// <param name="medicalHelpType"></param>
        /// <returns></returns>
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
        /// Удаление типа медицинской помощи
        /// </summary>
        /// <param name="idMedicalHelpType"></param>
        /// <returns></returns>
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
