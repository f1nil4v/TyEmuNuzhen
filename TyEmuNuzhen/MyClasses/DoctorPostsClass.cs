using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class DoctorPostsClass
    {
        public static DataTable dtDoctorPostsList;

        public static void GetDoctorPostsList()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT ID, postName FROM doctor_posts ORDER BY postName";
                dtDoctorPostsList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDoctorPostsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
