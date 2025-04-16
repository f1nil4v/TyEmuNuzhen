using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ConsentsClass
    {
        public static bool AddChildrenAppealConsent(string idOrphanage)
        {
            try
            {
                string idDocument = ChildrenDocumentClass.GetLastDocumentID();
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                DBConnection.myCommand.CommandText = $@"INSERT INTO consents 
                    VALUES (null, '{dateNow}', '{idDocument}', '{idOrphanage}')";

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
    }
}
