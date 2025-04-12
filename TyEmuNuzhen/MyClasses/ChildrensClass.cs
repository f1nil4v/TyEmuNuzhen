using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrensClass
    {
        public static DataTable dtChildrensList = new DataTable();
        public static DataTable dtChildrensDetailedList = new DataTable();
        public static DataTable dtChildrensCuratorList = new DataTable();

        public static void GetChildrenList(string idStatus, string idRegion, string dateAddedBeginPeriod, string dateAddedEndPeriod, string searchQuery, bool isDESC)
        {
            try
            {
                string whereClause = $"childrens.idStatus = '{idStatus}'";
                string orderByClause = isDESC ? "DESC" : "ASC";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND childrens.idRegion = '{idRegion}'";
                if (!String.IsNullOrEmpty(dateAddedBeginPeriod) && !String.IsNullOrEmpty(dateAddedEndPeriod))
                    whereClause += $" AND childrens.dateAdded BETWEEN '{dateAddedBeginPeriod}' AND '{dateAddedEndPeriod}'";
                if (!String.IsNullOrEmpty(searchQuery))
                    whereClause += $@" AND (childrens.numOfQuestionnaire LIKE @searchQuery OR childrens.surname LIKE @searchQuery OR childrens.name LIKE @searchQuery 
                                       OR childrens.middleName LIKE @searchQuery 
                                       OR CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) LIKE @searchQuery
                                       OR (SELECT description
                                        FROM childrens_description
                                        WHERE childrens_description.idChild = childrens.ID 
                                        ORDER BY childrens_description.ID DESC 
                                        LIMIT 1) LIKE @searchQueryDescription)";

                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT childrens.ID, childrens.numOfQuestionnaire, childrens.urlOfQuestionnaire, 
                        CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) AS 'fullName', childrens.birthday,
                        (SELECT statusName
                             FROM children_status 
                             WHERE children_status.ID = childrens.idStatus) AS statusName,
                        (SELECT regions.regionName
                             FROM regions 
                             WHERE regions.ID = childrens.idRegion) AS regionName,
                        (SELECT orphanages.nameOrphanage
                             FROM orphanages 
                             WHERE orphanages.ID = childrens.idOrphanage) AS orphanageName,
                        (SELECT dateAdded
                             FROM childrens_description
                             WHERE childrens_description.idChild = childrens.ID 
                             ORDER BY childrens_description.ID DESC 
                             LIMIT 1) AS dateDescriptionAdded,
                        (SELECT description
                             FROM childrens_description
                             WHERE childrens_description.idChild = childrens.ID 
                             ORDER BY childrens_description.ID DESC 
                             LIMIT 1) AS description,
                        childrens.dateAdded,
                        (SELECT childphoto.filePath 
                             FROM childphoto 
                             WHERE childphoto.idChild = childrens.ID 
                             ORDER BY childphoto.ID DESC 
                             LIMIT 1) AS latestPhotoPath,
                        childrens.isAlert
                    FROM 
                        childrens
                    WHERE
                        {whereClause}
                    ORDER BY 
                        childrens.isAlert DESC,
                        childrens.ID {orderByClause}";
                if (searchQuery != null)
                {
                    string wildcardSearch = searchQuery + "%";
                    string wildcardSearchDescription = "%" + searchQuery + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQueryDescription", wildcardSearchDescription);
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtChildrensList.Clear();
                DBConnection.myDataAdapter.Fill(dtChildrensList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetChildrenCurartorList(string idStatus, string idRegion, string dateAddedBeginPeriod, string dateAddedEndPeriod, string searchQuery, bool isDESC, string statusProgram)
        {
            try
            {
                string whereClause = $"childrens.idStatus = '{idStatus}'";
                string orderByClause = isDESC ? "DESC" : "ASC";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND childrens.idRegion = '{idRegion}'";
                if (!String.IsNullOrEmpty(dateAddedBeginPeriod) && !String.IsNullOrEmpty(dateAddedEndPeriod))
                    whereClause += $" AND childrens.dateAdded BETWEEN '{dateAddedBeginPeriod}' AND '{dateAddedEndPeriod}'";
                if (!String.IsNullOrEmpty(searchQuery))
                    whereClause += $@" AND (childrens.surname LIKE @searchQuery OR childrens.name LIKE @searchQuery 
                                       OR childrens.middleName LIKE @searchQuery 
                                       OR CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) LIKE @searchQuery)";
                if (!String.IsNullOrEmpty(statusProgram))
                    whereClause += $" AND childrens.idStatusProgram = '{statusProgram}'";

                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT childrens.ID, childrens.numOfQuestionnaire, childrens.urlOfQuestionnaire, 
                        CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) AS 'fullName', childrens.birthday,
                        (SELECT statusName
                             FROM children_status 
                             WHERE children_status.ID = childrens.idStatus) AS statusName,
                        (SELECT regions.regionName
                             FROM regions 
                             WHERE regions.ID = childrens.idRegion) AS regionName,
                        (SELECT orphanages.nameOrphanage
                             FROM orphanages 
                             WHERE orphanages.ID = childrens.idOrphanage) AS orphanageName,
                        childrens.dateAdded,
                        (SELECT childphoto.filePath 
                             FROM childphoto 
                             WHERE childphoto.idChild = childrens.ID 
                             ORDER BY childphoto.ID DESC 
                             LIMIT 1) AS latestPhotoPath,
                        (SELECT statusName FROM children_status_program 
                             WHERE children_status_program.ID = childrens.idStatusProgram) AS statusProgramName,
                        childrens.isAlert
                    FROM 
                        childrens
                    WHERE
                        {whereClause}
                    ORDER BY 
                        childrens.ID {orderByClause}";
                if (searchQuery != null)
                {
                    string wildcardSearch = searchQuery + "%";
                    string wildcardSearchDescription = "%" + searchQuery + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQueryDescription", wildcardSearchDescription);
                    DBConnection.myCommand.Parameters.AddWithValue("@searchQuery", wildcardSearch);
                }
                dtChildrensCuratorList.Clear();
                DBConnection.myDataAdapter.Fill(dtChildrensCuratorList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetChildrenListByID(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT childrens.ID, childrens.numOfQuestionnaire, childrens.urlOfQuestionnaire, 
                        childrens.surname, childrens.name, childrens.middleName, childrens.birthday,
                        (SELECT dateAdded
                         FROM childrens_description
                         WHERE childrens_description.idChild = childrens.ID 
                         ORDER BY childrens_description.ID DESC 
                         LIMIT 1) AS dateDescriptionAdded,
                        childrens.dateAdded,
                        (SELECT childphoto.filePath 
                         FROM childphoto 
                         WHERE childphoto.idChild = childrens.ID 
                         ORDER BY childphoto.ID DESC 
                         LIMIT 1) AS latestPhotoPath,
                        (SELECT regions.regionName
                             FROM regions 
                             WHERE regions.ID = childrens.idRegion) AS regionName,
                        (SELECT orphanages.nameOrphanage
                                FROM orphanages 
                                WHERE orphanages.ID = childrens.idOrphanage) AS orphanageName,
                        childrens.idRegion,
                        childrens.isAlert
                    FROM 
                        childrens
                    WHERE
                        childrens.ID = '{idChild}'";
                dtChildrensDetailedList.Clear();
                DBConnection.myDataAdapter.Fill(dtChildrensDetailedList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetLastChildrensID()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT MAX(ID) FROM childrens";
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

        public static string GetCountChildrensMonitoring(string idRegion, string idStatus)
        {
            try
            {
                string whereClause = $"idStatus = '{idStatus}'";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND idRegion = '{idRegion}'";
                DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM childrens WHERE {whereClause}";
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

        public static bool AddMonitoringInfoChildren(string numQuest, string urlQuest, string surname, string name, string birthday, string idRegion, string isAlert)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = @"INSERT INTO childrens 
                    VALUES (null, @numQuest, @urlQuest, @surname, @name, null, @birthday, 1, @idRegion, null, @dateNow, @isAlert)";
                
                DBConnection.myCommand.Parameters.AddWithValue("@numQuest", numQuest);
                DBConnection.myCommand.Parameters.AddWithValue("@urlQuest", urlQuest);
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@birthday", birthday);
                DBConnection.myCommand.Parameters.AddWithValue("@idRegion", idRegion);
                DBConnection.myCommand.Parameters.AddWithValue("@dateNow", dateNow);
                DBConnection.myCommand.Parameters.AddWithValue("@isAlert", isAlert);
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

        public static bool UpdateMonitoringInfoChildren(string idChild, string numQuest, string urlQuest, string surname, string name, string birthday, string isAlert)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE childrens 
                    SET numOfQuestionnaire = @numQuest, urlOfQuestionnaire = @urlQuest, 
                        surname = @surname, name = @name, birthday = @birthday, isAlert = @isAlert WHERE ID = '{idChild}'";
                DBConnection.myCommand.Parameters.AddWithValue("@numQuest", numQuest);
                DBConnection.myCommand.Parameters.AddWithValue("@urlQuest", urlQuest);
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@birthday", birthday);
                DBConnection.myCommand.Parameters.AddWithValue("@isAlert", isAlert);
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

        public static bool UpdateCuratorInfoChildren(string idChild, string surname, string name, string middleName, string birthday, string idRegion, string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE childrens 
                    SET surname = @surname, name = @name, middleName = @middleName, birthday = @birthday, idRegion = '{idRegion}', idOrphanage = '{idOrphanage}' WHERE ID = '{idChild}'";
                DBConnection.myCommand.Parameters.AddWithValue("@surname", surname);
                DBConnection.myCommand.Parameters.AddWithValue("@name", name);
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@birthday", birthday);
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

        public static bool AddCommonInfoChildren(string id, string middleName, string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE childrens 
                    SET middleName = @middleName, idOrphanage = @idOrphanage, idStatus = 3 WHERE ID = '{id}'";
                DBConnection.myCommand.Parameters.AddWithValue("@middleName", middleName);
                DBConnection.myCommand.Parameters.AddWithValue("@idOrphanage", idOrphanage);
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

        public static bool UpdateStatusChildren(string idChild, string idStatus)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE childrens 
                    SET idStatus = @idStatus WHERE ID = '{idChild}'";
                DBConnection.myCommand.Parameters.AddWithValue("@idStatus", idStatus);
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

    }
}
