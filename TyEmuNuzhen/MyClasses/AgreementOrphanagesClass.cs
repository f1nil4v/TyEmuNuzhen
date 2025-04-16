using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class AgreementOrphanagesClass
    {
        public static DataTable dtAgreementOrphanageData;

        public static void GetAgreementOrphanageData(string idOrphanage)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM agreement_orphanage 
                                                        WHERE idOrphanage = '{idOrphanage}'";
                dtAgreementOrphanageData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtAgreementOrphanageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
