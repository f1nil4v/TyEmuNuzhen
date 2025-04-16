using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;

namespace TyEmuNuzhen.MyClasses
{
    internal class ChildrenDiagnosisClass
    {
        public static DataTable dtChildrenDiagnoses;
        public static List<string> listChildrenIDDiagnoses = new List<string>();

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

        public static bool AddChildrenDiagnosis(string date)
        {
            try
            {
                foreach (string id in listChildrenIDDiagnoses)
                {
                    DBConnection.myCommand.CommandText = $@"INSERT INTO children_diagnosis 
                    VALUES (null, '{id}', '{date}')";
                    if (DBConnection.myCommand.ExecuteNonQuery() < 0)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении диагноза. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
} 