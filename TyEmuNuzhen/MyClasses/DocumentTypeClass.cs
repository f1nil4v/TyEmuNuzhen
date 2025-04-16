using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class DocumentTypeClass
    {
        public static DataTable dtDocumentTypes;
        public static string selectedIDTypeDocument;

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

    }
}
