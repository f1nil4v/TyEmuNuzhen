﻿using MySql.Data.MySqlClient;
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
    /// Класс для работы с типами документов
    /// </summary>
    internal class DocumentTypeClass
    {
        public static DataTable dtDocumentTypes;
        public static DataTable dtDocumentTypesS;
        public static string selectedIDTypeDocument;

        /// <summary>
        /// Получение списка типов документов
        /// </summary>
        public static void GetDocumentTypes()
        {
            try
            {
                DBConnection.myCommand.CommandText = @"SELECT ID, documentType FROM documents_type 
                    WHERE ID <> 1 ORDER BY documentType";
                dtDocumentTypes = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDocumentTypes);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение списка типов документов с возможностью поиска по названию
        /// </summary>
        /// <param name="querySearch"></param>
        public static void GetDocumentTypesList(string querySearch)
        {
            try
            {
                string whereClause = querySearch != "" ? $"WHERE ID <> 1 AND documentType LIKE @querySearch" : "WHERE ID <> 1";
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"SELECT ID, documentType FROM documents_type {whereClause}";
                if (whereClause != "")
                {
                    string wildcardSearch = querySearch + "%";
                    DBConnection.myCommand.Parameters.AddWithValue("@querySearch", wildcardSearch);
                }
                dtDocumentTypesS = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDocumentTypesS);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение названия типа документа по его ID
        /// </summary>
        /// <param name="idDocumentType"></param>
        /// <returns></returns>
        public static string GetDocumentTypeName(string idDocumentType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT documentType FROM documents_type WHERE ID = '{idDocumentType}'";
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
        /// Получение количества всех типов документов
        /// </summary>
        /// <returns></returns>
        public static string GetCountAllDocumentTypes()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM documents_type WHERE ID <> 1";
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
        /// Добавление нового типа документа
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static bool AddDocumentType(string documentType)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"INSERT INTO documents_type VALUES (null, @documentType)";
                DBConnection.myCommand.Parameters.AddWithValue("@documentType", documentType);
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
        /// Обновление типа документа
        /// </summary>
        /// <param name="idDocumentType"></param>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public static bool UpdateDocumentType(string idDocumentType, string documentType)
        {
            try
            {
                DBConnection.myCommand.Parameters.Clear();
                DBConnection.myCommand.CommandText = $@"UPDATE documents_type SET documentType = @documentType WHERE ID = '{idDocumentType}'";
                DBConnection.myCommand.Parameters.AddWithValue("@documentType", documentType);
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
        /// Удаление типа документа
        /// </summary>
        /// <param name="idDocumentType"></param>
        /// <returns></returns>
        public static bool DeleteDocumentType(string idDocumentType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM documents_type WHERE ID = '{idDocumentType}'";
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
