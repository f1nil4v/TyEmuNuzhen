using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class HospitalizationClass
    {
        public static DataTable dtHospitalizationData;

        public static void GetHospitalizationData(string idHospitalization, string idActualProgram)
        {
            try
            {
                string whereClause = string.IsNullOrEmpty(idHospitalization) ? "" : $" AND hospitalization.ID = '{idHospitalization}'";
                DBConnection.myCommand.CommandText = $@"SELECT hospitalization.ID, CONCAT(medical_facility.medicalFacilityName, ' (Адрес: ', medical_facility.address, ')') as 'medicalFacility', 
                                                            hospitalization.dateHospitalization, hospitalization.dateDischarge, hospitalization.totalCost, hospitalization.filePath
                                                        FROM hospitalization, medical_facility
                                                        WHERE hospitalization.idMedicalFacility = medical_facility.ID AND hospitalization.idActualProgram = '{idActualProgram}'
                                                        {whereClause}";
                dtHospitalizationData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtHospitalizationData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool AddHospitalization(string idMedicalFacility, string idActualProgram, string dateHospitalization, string dateDischarge, string filePath)
        {
            try
            {
                dateDischarge = dateDischarge == "" ? "null" : $"'{dateDischarge}'";
                DBConnection.myCommand.CommandText = $@"INSERT INTO hospitalization VALUES (null, '{idMedicalFacility}', '{idActualProgram}', '{dateHospitalization}', {dateDischarge}, 0, '{filePath}')";
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

        public static bool UpdateHospitalization(string idHospitalization, string idMedicalFacility, string dateHospitalization, string dateDischarge, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE hospitalization SET idMedicalFacility = '{idMedicalFacility}', dateHospitalization = '{dateHospitalization}', dateDischarge = '{dateDischarge}', filePath = '{filePath}' WHERE ID = '{idHospitalization}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
