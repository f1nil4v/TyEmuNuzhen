using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Documents;

namespace TyEmuNuzhen.MyClasses
{
    internal class DiagnosesClass
    {
        public static DataTable dtDiagnosesForChildrenComboBoxList;
        public static List<string> selectedIDDiagnoses = new List<string>();
        public static List<string> selectedDiagnoses = new List<string>();

        public static void GetDiagnosesForChildrenComboBoxList(string idChild)
        {
            try
            {
                string queryParam = String.Join(",", selectedIDDiagnoses);
                if (string.IsNullOrEmpty(queryParam))
                    DBConnection.myCommand.CommandText = $@"SELECT * FROM diagnoses";
                else
                    DBConnection.myCommand.CommandText = $@"SELECT * FROM diagnoses WHERE ID NOT IN ({queryParam})";
                dtDiagnosesForChildrenComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtDiagnosesForChildrenComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
