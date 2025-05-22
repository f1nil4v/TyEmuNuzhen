using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace TyEmuNuzhen.MyClasses
{
    internal class ProgramTypeClass
    {
        public static DataTable dtProgramTypeList;

        public static void GetProgramTypeList()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM program_type";
                dtProgramTypeList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtProgramTypeList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
