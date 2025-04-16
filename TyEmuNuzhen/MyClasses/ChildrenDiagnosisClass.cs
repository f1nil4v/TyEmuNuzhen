using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrenDiagnosisClass
    {
        public static DataTable dtChildrenDiagnoses;

        public static void GetChildrenDiagnoses(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT children_diagnosis.ID, diagnoses.diagnosisName,
                    children_diagnosis.updateDate
                    FROM children_diagnosis, diagnoses, consultation
                    WHERE consultation.ID = children_diagnosis.idConsultation AND children_diagnosis.idDiagnosis = diagnoses.ID AND
                        consultation.idChild = '{idChild}'
                    ORDER BY children_diagnosis.ID DESC";
                dtChildrenDiagnoses = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrenDiagnoses);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //public static bool AddChildrenDiagnosis(string idChild, string idDiagnosis)
        //{
        //    try
        //    {
        //        string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                
        //        DBConnection.myCommand.Parameters.Clear();
        //        DBConnection.myCommand.CommandText = @"INSERT INTO children_diagnoses 
        //            VALUES (null, @idDiagnosis, @idChild, @dateNow)";
                
        //        DBConnection.myCommand.Parameters.AddWithValue("@idDiagnosis", idDiagnosis);
        //        DBConnection.myCommand.Parameters.AddWithValue("@idChild", idChild);
        //        DBConnection.myCommand.Parameters.AddWithValue("@dateNow", dateNow);
                
        //        if (DBConnection.myCommand.ExecuteNonQuery() > 0)
        //            return true;
        //        else 
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Произошла ошибка при добавлении диагноза. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return false;
        //    }
        //}
    }
} 