using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class VolonteerClass
    {
        public static string idRegion;
        public static DataTable dtVolonteersList;
        public static DataTable dtVolonteerDataList;

        public static string GetVolonteerID(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT ID FROM volunteers WHERE idUser = '{idUser}'";
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

        public static string GetVolonteerFullName(string ID)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) FROM volunteers WHERE ID = '{ID}'";
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

        public static string GetVolonteerRegion(string idUser)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT idRegion FROM volunteers WHERE idUser = '{idUser}'";
                Object resultIDRegion = DBConnection.myCommand.ExecuteScalar();
                if (resultIDRegion != null)
                {
                    idRegion = resultIDRegion.ToString();
                    return resultIDRegion.ToString();
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

        public static void GetVolonteersList(string querySearch, string orderByValue)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                string whereClause = string.IsNullOrEmpty(querySearch) ? null :
                    $@"AND (volunteers.surname LIKE @searchQuery OR volunteers.name LIKE @searchQuery OR volunteers.middleName LIKE @searchQuery 
                        OR volunteers.phoneNumber LIKE @searchQuery OR volunteers.email LIKE @searchQuery OR users.login LIKE @searchQuery
                        OR CONCAT_WS(' ', volunteers.surname, volunteers.name, IFNULL(volunteers.middleName, '') LIKE @searchQuery))";
                string orderBy = string.IsNullOrEmpty(orderByValue) ? null : $"ORDER BY {orderByValue}";
                DBConnection.myCommand.CommandText = $@"SELECT volunteers.ID, users.login, volunteers.surname, volunteers.name, IFNULL(volunteers.middleName, '-') as 'middleName',
                                            volunteers.phoneNumber, volunteers.email, regions.regionName, volunteers.idUser
                                        FROM volunteers, users, regions
                                        WHERE users.ID = volunteers.idUser AND regions.ID = volunteers.idRegion {whereClause}
                                        {orderBy}";
                if (whereClause != null)
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtVolonteersList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtVolonteersList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetVolonteerData(string idVolonteer)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT volunteers.ID, users.login, volunteers.surname, volunteers.name, IFNULL(volunteers.middleName, '-') as 'middleName',
                                            volunteers.phoneNumber, volunteers.email, regions.regionName, volunteers.idUser
                                        FROM volunteers, regions, users
                                        WHERE users.ID = volunteers.idUser AND regions.ID = volunteers.idRegion AND volunteers.ID = '{idVolonteer}'";
                dtVolonteerDataList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtVolonteerDataList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetCountAllVolonteers()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM volunteers";
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

        public static bool AddVolonteer(string surname, string name, string middleName, string phoneNumber, string email, string idRegion)
        {
            try
            {
                string idUser = UserClass.GetLastUserID();
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $"INSERT INTO volunteers VALUES (null, @surname, @name, @middleName, @phoneNumber, @email, '{idRegion}', '{idUser}')";
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

        public static bool UpdateVolonteer(string idVolonteer, string surname, string name, string middleName, string phoneNumber, string email, string idRegion)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE volunteers SET surname = @surname, name = @name, middleName = @middleName, 
                                                            phoneNumber = @phoneNumber, email = @email, idregion = '{idRegion}'
                                                        WHERE ID = '{idVolonteer}'";
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

        public static bool DeleteVolonteer(string idVolonteer)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"DELETE FROM volunteers WHERE ID = '{idVolonteer}'";
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
