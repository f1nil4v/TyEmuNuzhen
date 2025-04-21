using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class MedicalFacilityClass
    {
        public static DataTable dtMedicalFacilityForComboBoxList;

        public static void GetDoctrosForComboBoxList()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT ID, medicalFacilityName FROM medical_facility ORDER BY medicalFacilityName";
                dtMedicalFacilityForComboBoxList = new DataTable();
                DBConnection.myDataAdapter.Fill(dtMedicalFacilityForComboBoxList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
