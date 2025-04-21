using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class CuratorClass
    {
        public static string fullNameCurator;

        public static DataTable dtCuratorsList;
        public static DataTable dtCuratorDataList;

        public static string GetCuratorID(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT ID FROM curators WHERE idUser = '{idUser}'";
                Object resultID = DBConnection.myCommand.ExecuteScalar();
                if (resultID != null)
                {
                    return resultID.ToString();
                }
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

        public static string GetCuratorFullName(string ID)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) FROM curators WHERE ID = '{ID}'";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                {
                    fullNameCurator = result.ToString();
                    return result.ToString();
                }
                else
                {
                    fullNameCurator = null;
                    return null;
                }
            }
            catch (Exception ex)
            {
                fullNameCurator = null;
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static void GetCuratorsList(string querySearch, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (curators.surname LIKE @searchQuery OR curators.name LIKE @searchQuery OR curators.middleName LIKE @searchQuery 
                        OR curators.phoneNumber LIKE @searchQuery OR curators.email LIKE @searchQuery OR users.login LIKE @searchQuery
                        OR CONCAT_WS(' ', curators.surname, curators.name, IFNULL(curators.middleName, '')) LIKE @searchQuery)";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT curators.ID, users.login, curators.surname, curators.name, IFNULL(curators.middleName, '-') as 'middleName',
                                            curators.phoneNumber, curators.email, curators.idUser 
                                        FROM curators, users
                                        WHERE users.ID = curators.idUser {whereClause}
                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtCuratorsList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtCuratorsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetCuratorData(string idCurator)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT curators.ID, users.login, curators.surname, curators.name, IFNULL(curators.middleName, '-') as 'middleName',
                                            curators.phoneNumber, curators.email, curators.idUser, users.idRole
                                        FROM curators, users
                                        WHERE users.ID = curators.idUser AND curators.ID = '{idCurator}'";
                dtCuratorDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtCuratorDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetCountAllCurators()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM curators";
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

        public static bool AddCurator(string surname, string name, string middleName, string phoneNumber, string email)
        {
            try
            {
                string idUser = UserClass.GetLastUserID();
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO curators VALUES (null, @surname, @name, @middleName, @phoneNumber, @email, '{idUser}')";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
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

        public static bool UpdateCurator(string idCurator, string surname, string name, string middleName, string phoneNumber, string email)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE curators SET surname = @surname, name = @name, middleName = @middleName, 
                                                            phoneNumber = @phoneNumber, email = @email
                                                        WHERE ID = '{idCurator}'";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
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

        public static bool DeleteCurator(string idCurator)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM curators WHERE ID = '{idCurator}'";
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
