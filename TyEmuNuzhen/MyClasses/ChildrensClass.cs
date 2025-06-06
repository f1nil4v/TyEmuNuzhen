using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с данными о детях
    /// </summary>
    internal class ChildrensClass
    {
        public static DataTable dtChildrensList;
        public static DataTable dtChildrensDetailedList;
        public static DataTable dtChildrensCuratorList;

        /// <summary>
        /// Получение списка детей
        /// </summary>
        /// <param name="idStatus"></param>
        /// <param name="idRegion"></param>
        /// <param name="dateAddedBeginPeriod"></param>
        /// <param name="dateAddedEndPeriod"></param>
        /// <param name="searchQuery"></param>
        /// <param name="isDESC"></param>
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
                dtChildrensList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrensList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка детей для консультаций
        /// </summary>
        /// <param name="idRegion"></param>
        /// <param name="idOrphange"></param>
        /// <param name="dateAddedBeginPeriod"></param>
        /// <param name="dateAddedEndPeriod"></param>
        /// <param name="searchQuery"></param>
        /// <param name="isDESC"></param>
        public static void GetChildrenMedicalExaminationList(string idRegion, string idOrphange, string dateAddedBeginPeriod, string dateAddedEndPeriod, string searchQuery, bool isDESC)
        {
            try
            {
                string whereClause = $"childrens.idStatus = 3";
                string orderByClause = isDESC ? "DESC" : "ASC";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND childrens.idRegion = '{idRegion}'";
                if (!String.IsNullOrEmpty(idOrphange))
                    whereClause += $" AND childrens.idOrphanage = '{idOrphange}'";
                if (!String.IsNullOrEmpty(dateAddedBeginPeriod) && !String.IsNullOrEmpty(dateAddedEndPeriod))
                    whereClause += $" AND childrens.dateAdded BETWEEN '{dateAddedBeginPeriod}' AND '{dateAddedEndPeriod}'";
                if (!String.IsNullOrEmpty(searchQuery))
                    whereClause += $@" AND (childrens.surname LIKE @searchQuery OR childrens.name LIKE @searchQuery 
                                       OR childrens.middleName LIKE @searchQuery 
                                       OR CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) LIKE @searchQuery)";

                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT childrens.ID, childrens.numOfQuestionnaire, childrens.urlOfQuestionnaire, 
                        CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) AS 'fullName', childrens.birthday,
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
                        childrens.idOrphanage
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
                dtChildrensCuratorList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrensCuratorList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка детей, участвующих в программе
        /// </summary>
        /// <param name="idStatus"></param>
        /// <param name="idRegion"></param>
        /// <param name="idOrphange"></param>
        /// <param name="dateAddedBeginPeriod"></param>
        /// <param name="dateAddedEndPeriod"></param>
        /// <param name="searchQuery"></param>
        /// <param name="isDESC"></param>
        /// <param name="statusProgram"></param>
        /// <param name="idCurator"></param>
        public static void GetChildrenActualProgramList(string idStatus, string idRegion, string idOrphange, string dateAddedBeginPeriod, string dateAddedEndPeriod, string searchQuery, bool isDESC, string statusProgram, string idCurator)
        {
            try
            {
                string whereClause = $"actual_program.idChild = childrens.ID AND childrens.idStatus = '{idStatus}' AND actual_program.idCurator = '{idCurator}' AND actual_program.status = 1";
                string orderByClause = isDESC ? "DESC" : "ASC";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND childrens.idRegion = '{idRegion}'";
                if (!String.IsNullOrEmpty(idOrphange))
                    whereClause += $" AND childrens.idOrphanage = '{idOrphange}'";
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
                        childrens.idStatusProgram,
                        childrens.isAlert, childrens.idOrphanage
                    FROM 
                        childrens, actual_program
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
                dtChildrensCuratorList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrensCuratorList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка детей, с которыми проводились работы
        /// </summary>
        /// <param name="idRegion"></param>
        /// <param name="idOrphange"></param>
        /// <param name="dateAddedBeginPeriod"></param>
        /// <param name="dateAddedEndPeriod"></param>
        /// <param name="searchQuery"></param>
        /// <param name="isDESC"></param>
        public static void GetChildrenHistoryAnyWorks(string idRegion, string idOrphange, string dateAddedBeginPeriod, string dateAddedEndPeriod, string searchQuery, bool isDESC)
        {
            try
            {
                string whereClause = $@"(childrens.ID IN (SELECT idChild FROM actual_program WHERE status = 0) OR childrens.idStatus = 11)";
                string orderByClause = isDESC ? "DESC" : "ASC";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND childrens.idRegion = '{idRegion}'";
                if (!String.IsNullOrEmpty(idOrphange))
                    whereClause += $" AND childrens.idOrphanage = '{idOrphange}'";
                if (!String.IsNullOrEmpty(dateAddedBeginPeriod) && !String.IsNullOrEmpty(dateAddedEndPeriod))
                    whereClause += $" AND childrens.dateAdded BETWEEN '{dateAddedBeginPeriod}' AND '{dateAddedEndPeriod}'";
                if (!String.IsNullOrEmpty(searchQuery))
                    whereClause += $@" AND (childrens.surname LIKE @searchQuery OR childrens.name LIKE @searchQuery 
                                       OR childrens.middleName LIKE @searchQuery 
                                       OR CONCAT_WS(' ', childrens.surname, childrens.name, IFNULL(childrens.middleName, '')) LIKE @searchQuery)";

                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT DISTINCT childrens.ID, childrens.numOfQuestionnaire, childrens.urlOfQuestionnaire, 
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
                        childrens.idStatusProgram, childrens.idStatus,
                        childrens.isAlert, childrens.idOrphanage,
                        (SELECT CONCAT(program_type.programType, ' (Период: ', DATE_FORMAT(actual_program.dateBegin, '%d.%m.%Y'), ' - ', DATE_FORMAT(actual_program.dateEnd, '%d.%m.%Y'), ')') FROM actual_program, program_type
                             WHERE actual_program.idProgramType = program_type.ID AND actual_program.idChild = childrens.ID
                             ORDER BY actual_program.ID DESC LIMIT 1) AS lastCompletedProgram
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
                dtChildrensCuratorList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrensCuratorList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение детальной информации о ребенке по его ID
        /// </summary>
        /// <param name="idChild"></param>
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
                        childrens.idOrphanage,
                        childrens.isAlert,
                        childrens.idStatusProgram
                    FROM 
                        childrens
                    WHERE
                        childrens.ID = '{idChild}'";
                dtChildrensDetailedList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrensDetailedList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение ID последнего добавленного ребенка
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Получение количества детей по статусу и региону
        /// </summary>
        /// <param name="idRegion"></param>
        /// <param name="idStatus"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Получение количества детей по статусу, региону и куратору
        /// </summary>
        /// <param name="idRegion"></param>
        /// <param name="idStatus"></param>
        /// <param name="idCurator"></param>
        /// <returns></returns>
        public static string GetCountChildrensMonitoring(string idRegion, string idStatus, string idCurator)
        {
            try
            {
                string whereClause = $" AND childrens.idStatus = '{idStatus}'";
                if (!String.IsNullOrEmpty(idRegion))
                    whereClause += $" AND childrens.idRegion = '{idRegion}'";
                DBConnection.myCommand.CommandText = $"SELECT COUNT(childrens.ID) FROM childrens, actual_program WHERE actual_program.idCurator = '{idCurator}' AND actual_program.idChild = childrens.ID {whereClause}";
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

        /// <summary>
        /// Проверка на уникальность номера анкеты 
        /// </summary>
        /// <param name="numOfQuest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool GetSameNumOfQuestionnaire(string numOfQuest, string id = null)
        {
            try
            {
                string whereClause = String.IsNullOrEmpty(id) ? "" : $"AND ID <> '{id}'";
                DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM childrens WHERE numOfQuestionnaire = '{numOfQuest}' {whereClause}";
                if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Ребёнок с данным номером анкеты уже есть в системе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// Проверка на уникальность ссылки анкеты
        /// </summary>
        /// <param name="urlOfQuestionnaire"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool GetSameURLOfQuestionnaire(string urlOfQuestionnaire, string id = null)
        {
            try
            {
                string whereClause = String.IsNullOrEmpty(id) ? "" : $"AND ID <> '{id}'";
                DBConnection.myCommand.CommandText = $"SELECT COUNT(ID) FROM childrens WHERE urlOfQuestionnaire = '{urlOfQuestionnaire}' {whereClause}";
                if (Convert.ToInt32(DBConnection.myCommand.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Ребёнок с данной анкетой уже есть в системе!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        /// Получение количества детей, с которыми проводились работы
        /// </summary>
        /// <returns></returns>
        public static string GetCountChildrensHistoryAnyWorks()
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT DISTINCT COUNT(ID) FROM childrens WHERE childrens.ID IN (SELECT idChild FROM actual_program WHERE status = 0) OR childrens.idStatus = 11";
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

        /// <summary>
        /// Добавление информации о ребенке в мониторинг
        /// </summary>
        /// <param name="numQuest"></param>
        /// <param name="urlQuest"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="birthday"></param>
        /// <param name="idRegion"></param>
        /// <param name="isAlert"></param>
        /// <returns></returns>
        public static bool AddMonitoringInfoChildren(string numQuest, string urlQuest, string surname, string name, string birthday, string idRegion, string isAlert)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = @"INSERT INTO childrens 
                    VALUES (null, @numQuest, @urlQuest, @surname, @name, null, @birthday, 1, null, null, @idRegion, @dateNow, @isAlert)";
                
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

        /// <summary>
        /// Обновление информации о ребенке в мониторинге
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="numQuest"></param>
        /// <param name="urlQuest"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="birthday"></param>
        /// <param name="isAlert"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновление информации о ребенке куратором
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="middleName"></param>
        /// <param name="birthday"></param>
        /// <param name="idRegion"></param>
        /// <param name="idOrphanage"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Добавление остальной информации о ребёнке
        /// </summary>
        /// <param name="id"></param>
        /// <param name="middleName"></param>
        /// <param name="idOrphanage"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновление статуса ребенка
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="idStatus"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Обновление статуса программы ребенка
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="idStatusProgram"></param>
        /// <returns></returns>
        public static bool UpdateStatusProgramChildren(string idChild, string idStatusProgram)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE childrens 
                    SET idStatusProgram = @idStatusProgram WHERE ID = '{idChild}'";
                DBConnection.myCommand.Parameters.AddWithValue("@idStatusProgram", idStatusProgram);
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
        /// Обновление статуса ребенка при завершении программы
        /// </summary>
        /// <param name="idChild"></param>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        public static bool UpdateStatusChildrenEndProgram(string idChild, string idStatus)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE childrens 
                    SET idStatus = @idStatus, idStatusProgram = null  WHERE ID = '{idChild}'";
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
