using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrenDocumentClass
    {
        public static DataTable dtChildrenDocumentsScan;
        public static DataTable dtChildrenAppealsConsents;
        public static DataTable dtTemporaryDocumentChildrenDocuments;

        public static void GetChildrenDocuments(string idChild, bool isScan, string idOrphanage)
        {
            try
            {
                string idActualProgram = ActualProgramClass.GetIDLastActualProgramChildren(idChild);
                string selectDateConclusion = isScan ? "" : ", consents.dateСonclusion";
                string consentsTable = isScan ? "" : ", consents";
                string isScanWhereClause = isScan ? "AND children_documents.idDocumentType <> 1" : $"AND children_documents.ID = consents.idDocument AND children_documents.idDocumentType = 1 AND consents.idActualProgram = '{idActualProgram}'";
                string orderByClause = isScan ? "ORDER BY documents_type.documentType" : "ORDER BY children_documents.ID DESC";

                DBConnection.myCommand.CommandText = $@"SELECT children_documents.ID, children_documents.filePath, documents_type.documentType
                        {selectDateConclusion}
                    FROM children_documents, documents_type {consentsTable}
                    WHERE children_documents.idDocumentType = documents_type.ID AND children_documents.idChild = '{idChild}'
                    {isScanWhereClause}
                    {orderByClause}";
                if (isScan)
                {
                    dtChildrenDocumentsScan = new DataTable();
                    DBConnection.myDataAdapter.Fill(dtChildrenDocumentsScan);
                }
                else
                {
                    dtChildrenAppealsConsents = new DataTable();
                    DBConnection.myDataAdapter.Fill(dtChildrenAppealsConsents);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static string GetCountChildrenDocuments(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $"SELECT COUNT(children_documents.ID) FROM children_documents WHERE children_documents.idChild = '{idChild}' AND children_documents.idDocumentType <> 1";
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

        public static bool AddChildrenDocument(string idChild, string idDocumentType, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"INSERT INTO children_documents 
                    VALUES (null, '{idChild}', '{idDocumentType}', '{filePath}')";
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

        public static bool UpdateChildrenDocument(string idChild, string idDocumentType, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE children_documents SET filePath = '{filePath}' WHERE idChild = '{idChild}' AND idDocumentType = '{idDocumentType}'";
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

        public static string GetLastDocumentID()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(ID) FROM children_documents";
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

        public static void GetSameDocumentScanByDocumentID(string idChild, string idDocumentType)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM children_documents 
                    WHERE idChild = '{idChild}' AND idDocumentType = '{idDocumentType}'";
                dtTemporaryDocumentChildrenDocuments = new DataTable();
                DBConnection.myDataAdapter.Fill(dtTemporaryDocumentChildrenDocuments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 