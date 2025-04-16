using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class OrphanageClass
    {
        public static DataTable dtOrphanagesForComboBoxList;
        public static DataTable dtOrphanageDataForPrintDocuments;

        public static void GetOrphanagesForComboBoxList(string idRegion)
        {
            try
            {
                string whereClause = string.IsNullOrEmpty(idRegion) ? "" : $" WHERE idRegion = '{idRegion}'";
                DBConnection.myCommand.CommandText = $@"SELECT ID, nameOrphanage FROM orphanages 
                                                        {whereClause}
                                                        ORDER BY nameOrphanage";
                dtOrphanagesForComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtOrphanagesForComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetOrphanageDataForPrintDocuments(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM orphanages 
                                                        WHERE ID = '{idOrphanage}'";
                dtOrphanageDataForPrintDocuments = new DataTable();
                DBConnection.myDataAdapter.Fill(dtOrphanageDataForPrintDocuments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
    }
}
