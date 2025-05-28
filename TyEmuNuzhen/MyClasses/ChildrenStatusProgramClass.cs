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
    /// Класс для работы со статусами детей в программе
    /// </summary>
    internal class ChildrenStatusProgramClass
    {
        public static DataTable dtChildrenStatusProgramList;

        /// <summary>
        /// Получение списка статусов детей в программе
        /// </summary>
        public static void GetChildrenStatusProgramList()
        {
            try
            {
                DBConnection.myCommand.CommandText = "SELECT ID, statusName FROM children_status_program ORDER BY statusName";
                dtChildrenStatusProgramList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrenStatusProgramList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
