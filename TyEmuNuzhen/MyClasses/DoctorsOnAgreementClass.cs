using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class DoctorsOnAgreementClass
    {
        public static DataTable dtDoctorsForComboBoxList;

        public static void GetDoctrosForComboBoxList(string idPost)
        {
            try
            {
                string whereClause = string.IsNullOrEmpty(idPost) ? "" : $"WHERE idPost = '{idPost}'";
                DBConnection.myCommand.CommandText = $@"SELECT ID, CONCAT_WS(' ', surname, name, IFNULL(middleName, '')) AS fullName FROM doctors_on_agreement 
                                                        {whereClause}";
                dtDoctorsForComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorsForComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
