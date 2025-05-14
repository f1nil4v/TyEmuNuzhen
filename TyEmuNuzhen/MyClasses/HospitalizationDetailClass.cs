using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class HospitalizationDetailClass
    {
        public static DataTable dtHospitalizationDetailData;
        public static DataTable dtHospitalizationDetailDataChange;

        public static void GetHospitalizationDetailData(string idHospitalization)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT hospitalization_detail.ID, medical_care_type.medicalCareType, hospitalization_detail.idTypeMedicalHelp, hospitalization_detail.cost, hospitalization_detail.dateMedicalHelp
                                                        FROM hospitalization_detail, medical_care_type
                                                        WHERE hospitalization_detail.idTypeMedicalHelp = medical_care_type.ID AND hospitalization_detail.idHospitalization = '{idHospitalization}'";
                dtHospitalizationDetailData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtHospitalizationDetailData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void GetHospitalizationDetailDataChange(string idHospitalizationDetail)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT hospitalization_detail.idTypeMedicalHelp, hospitalization_detail.cost, hospitalization_detail.dateMedicalHelp
                                                        FROM hospitalization_detail, medical_care_type
                                                        WHERE hospitalization_detail.idTypeMedicalHelp = medical_care_type.ID AND hospitalization_detail.ID = '{idHospitalizationDetail}'";
                dtHospitalizationDetailDataChange = new DataTable();
                DBConnection.myDataAdapter.Fill(dtHospitalizationDetailDataChange);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool GetSameHospitalizationDetail(string idHospitalization, string idTypeMedicalHelp, string dateMedicalHelp)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM hospitalization_detail WHERE idHospitalization = '{idHospitalization}' AND idTypeMedicalHelp = '{idTypeMedicalHelp}' AND dateMedicalHelp = '{dateMedicalHelp}'";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool GetSameHospitalizationDetail(string idHospitalizationDetail, string idHospitalization, string idTypeMedicalHelp, string dateMedicalHelp)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT COUNT(ID) FROM hospitalization_detail WHERE idHospitalization = '{idHospitalization}' AND idTypeMedicalHelp = '{idTypeMedicalHelp}' AND dateMedicalHelp = '{dateMedicalHelp}' AND ID <> '{idHospitalizationDetail}'";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (Convert.ToInt32(result) == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool AddHospitalizationDetail(string idHospitalization, string idTypeMedicalHelp, string cost, string dateMedicalHelp)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"INSERT INTO hospitalization_detail VALUES (null, '{idHospitalization}', '{idTypeMedicalHelp}', '{cost}', '{dateMedicalHelp}')";
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

        public static bool UpdateHospitalizationDetail(string idHospitalizationDetail, string idTypeMedicalHelp, string cost, string dateMedicalHelp)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE hospitalization_detail SET idTypeMedicalHelp = '{idTypeMedicalHelp}', cost = '{cost}', dateMedicalHelp = '{dateMedicalHelp}' WHERE ID = '{idHospitalizationDetail}'";
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

        public static bool DeleteHospitalizationDetail(string idHospitalizationDetail)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM hospitalization_detail WHERE ID = '{idHospitalizationDetail}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
