using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class DirectorClass
    {
        public static DataTable dtDirectorsList;
        public static DataTable dtDirectorDataList;

        public static string GetDirectorID(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT ID FROM directors WHERE idUser = '{idUser}'";
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

        public static string GetDirectorFullName(string ID)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) FROM directors WHERE ID = '{ID}'";
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

        public static void GetDirectorsList(string querySearch, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (directors.surname LIKE @searchQuery OR directors.name LIKE @searchQuery OR directors.middleName LIKE @searchQuery 
                        OR directors.phoneNumber LIKE @searchQuery OR directors.email LIKE @searchQuery OR users.login LIKE @searchQuery
                        OR CONCAT_WS(' ', directors.surname, directors.name, IFNULL(directors.middleName, '')) LIKE @searchQuery)";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT directors.ID, users.login, directors.surname, directors.name, IFNULL(directors.middleName, '-') as 'middleName',
                                            directors.phoneNumber, directors.email, directors.idUser
                                        FROM directors, users
                                        WHERE users.ID = directors.idUser {whereClause}
                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtDirectorsList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDirectorsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetDirectorData(string idDirector)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT directors.ID, users.login, directors.surname, directors.name, IFNULL(directors.middleName, '-') as 'middleName',
                                            directors.phoneNumber, directors.email, directors.idUser
                                        FROM directors, users
                                        WHERE users.ID = directors.idUser AND directors.ID = '{idDirector}'";
                dtDirectorDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDirectorDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetCountAllDirecrors()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM directors";
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

        public static bool AddDirector(string surname, string name, string middleName, string phoneNumber, string email)
        {
            try
            {
                string idUser = UserClass.GetLastUserID();
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO directors VALUES (null, @surname, @name, @middleName, @phoneNumber, @email, '{idUser}')";
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

        public static bool UpdateDirector(string idDirector, string surname, string name, string middleName, string phoneNumber, string email)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE directors SET surname = @surname, name = @name, middleName = @middleName, 
                                                            phoneNumber = @phoneNumber, email = @email
                                                        WHERE ID = '{idDirector}'";
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

        public static bool DeleteDirector(string idDirector)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM directors WHERE ID = '{idDirector}'";
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
