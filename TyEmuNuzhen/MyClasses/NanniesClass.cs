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
    /// Класс для работы с нянями
    /// </summary>
    internal class NanniesClass
    {
        public static DataTable dtNanniesList;
        public static DataTable dtNanniesForSelectProgramList;
        public static DataTable dtNanniesDataList;

        /// <summary>
        /// Получение списка нянь
        /// </summary>
        /// <param name="querySearch"></param>
        /// <param name="orderByValue"></param>
        public static void GetNanniesList(string querySearch, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"WHERE (nannies.surname LIKE @searchQuery OR nannies.name LIKE @searchQuery OR nannies.middleName LIKE @searchQuery 
                        OR nannies.phoneNumber LIKE @searchQuery OR nannies.email LIKE @searchQuery
                        OR CONCAT_WS(' ', nannies.surname, nannies.name, IFNULL(nannies.middleName, '') LIKE @searchQuery)
                        OR passSeries LIKE @searchQuery OR passNum LIKE @searchQuery OR CONCAT_WS(' ', passSeries, passNum) LIKE @searchQuery)";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT ID, CONCAT_WS(' ', surname, name, IFNULL(nannies.middleName, '')) as 'FIO',
                                                        CONCAT_WS(' ', passSeries, passNum) as 'passSeriesNum', passDateOfIssue, passOrganizationOfIssue,
                                                        passCode, addressRegister, phoneNumber, email
                                                        FROM nannies
                                                        {whereClause}
                                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtNanniesList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtNanniesList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка нянь для выбора в программу
        /// </summary>
        /// <param name="querySearch"></param>
        /// <param name="orderByValue"></param>
        public static void GetNanniesListForSelectOnProgram(string querySearch, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (nannies.surname LIKE @searchQuery OR nannies.name LIKE @searchQuery OR nannies.middleName LIKE @searchQuery 
                        OR nannies.phoneNumber LIKE @searchQuery OR nannies.email LIKE @searchQuery
                        OR CONCAT_WS(' ', nannies.surname, nannies.name, IFNULL(nannies.middleName, '') LIKE @searchQuery)
                        OR nannies.passSeries LIKE @searchQuery OR nannies.passNum LIKE @searchQuery OR CONCAT_WS(' ', nannies.passSeries, nannies.passNum) LIKE @searchQuery) AND";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT nannies.ID, CONCAT_WS(' ', nannies.surname, nannies.name, IFNULL(nannies.middleName, '')) as 'FIO',
                                                        CONCAT_WS(' ', nannies.passSeries, nannies.passNum) as 'passSeriesNum', nannies.passDateOfIssue, nannies.passOrganizationOfIssue,
                                                        nannies.passCode, nannies.addressRegister, nannies.phoneNumber, nannies.email
                                                        FROM nannies
                                                        WHERE nannies.status = 0
                                                        {whereClause}
                                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtNanniesForSelectProgramList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtNanniesForSelectProgramList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных няни по ID
        /// </summary>
        /// <param name="idNanny"></param>
        public static void GetNannyData(string idNanny)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM nannies
                                        WHERE ID = '{idNanny}'";
                dtNanniesDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtNanniesDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение количества нянь
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllNannies()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM nannies";
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
        /// Проверка на уникальность паспортных данных няни 
        /// </summary>
        /// <param name="passSeries"></param>
        /// <param name="passNum"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool GetSamePassData(string passSeries, string passNum, string id = null)
        {
            try
            {
                string whereClause = String.IsNullOrEmpty(id) ? "" : $"AND ID <> '{id}'";
                DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM nannies WHERE passSeries = '{passSeries}' AND passNum = '{passNum}' {whereClause}";
                if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Наня с такими паспортными данными уже есть в системе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                else
                    return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Добавление няни
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="passSeries"></param>
        /// <param name="passNum"></param>
        /// <param name="passDateOfIssue"></param>
        /// <param name="passOrganizationOfIssue"></param>
        /// <param name="passCode"></param>
        /// <param name="addressRegister"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool AddNanny(string surname, string name, string middleName, string passSeries, string passNum, string passDateOfIssue, string passOrganizationOfIssue, string passCode, string addressRegister, string phoneNumber, string email)
        {
            try
            {
                string idUser = UserClass.GetLastUserID();
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO nannies VALUES (null, @surname, @name, @middleName, @passSeries, @passNum, @passDateOfIssue, 
                                                    @passOrganizationOfIssue, @passCode, @addressRegister, @phoneNumber, @email, 0)";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@passSeries", passSeries);
                DBConnection.myCommand.Parameters.AddWithValue("@passNum", passNum);
                DBConnection.myCommand.Parameters.AddWithValue("@passDateOfIssue", passDateOfIssue);
                DBConnection.myCommand.Parameters.AddWithValue("@passOrganizationOfIssue", passOrganizationOfIssue);
                DBConnection.myCommand.Parameters.AddWithValue("@passCode", passCode);
                DBConnection.myCommand.Parameters.AddWithValue("@addressRegister", addressRegister);
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
        /// Обновление статуса няни на программе
        /// </summary>
        /// <param name="idNanny"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool UpdateNannyOnProgramStatus(string idNanny, string status)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE nannies SET status = '{status}' WHERE ID = '{idNanny}'";
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
        /// Обновление данных няни
        /// </summary>
        /// <param name="idNanny"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="passSeries"></param>
        /// <param name="passNum"></param>
        /// <param name="passDateOfIssue"></param>
        /// <param name="passOrganizationOfIssue"></param>
        /// <param name="passCode"></param>
        /// <param name="addressRegister"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool UpdateNanny(string idNanny, string surname, string name, string middleName, string passSeries, string passNum, string passDateOfIssue, string passOrganizationOfIssue, string passCode, string addressRegister, string phoneNumber, string email)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE nannies SET surname = @surname, name = @name, middleName = @middleName, passSeries = @passSeries, passNum = @passNum,
                                                        passDateOfIssue = @passDateOfIssue, passOrganizationOfIssue = @passOrganizationOfIssue, addressRegister = @addressRegister,
                                                        phoneNumber = @phoneNumber, email = @email
                                                        WHERE ID = '{idNanny}'";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@passSeries", passSeries);
                DBConnection.myCommand.Parameters.AddWithValue("@passNum", passNum);
                DBConnection.myCommand.Parameters.AddWithValue("@passDateOfIssue", passDateOfIssue);
                DBConnection.myCommand.Parameters.AddWithValue("@passOrganizationOfIssue", passOrganizationOfIssue);
                DBConnection.myCommand.Parameters.AddWithValue("@passCode", passCode);
                DBConnection.myCommand.Parameters.AddWithValue("@addressRegister", addressRegister);
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
        /// Удаление няни
        /// </summary>
        /// <param name="idNanny"></param>
        /// <returns></returns>
        public static bool DeleteNanny(string idNanny)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM nannies WHERE ID = '{idNanny}'";
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
