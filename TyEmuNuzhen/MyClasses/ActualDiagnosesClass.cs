using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class ActualDiagnosesClass
    {
        public static DataTable dtActualChildrenDiagnoses;

        public static void GetChildrenAcutualDiagnoses(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT diagnoses.diagnosisName
                    FROM actual_children_diagnosis, diagnoses
                    WHERE actual_children_diagnosis.idDiagnosis = diagnoses.ID AND
                        actual_children_diagnosis.idChild = '{idChild}'
                    ORDER BY diagnoses.diagnosisName";
                dtActualChildrenDiagnoses = new DataTable();
                DBConnection.myDataAdapter.Fill(dtActualChildrenDiagnoses);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool AddChildrenDiagnosis(string idChild)
        {
            try
            {
                foreach (string idDiagnosis in DiagnosesClass.selectedIDDiagnoses)
                {
                    DBConnection.myCommand.CommandText = $@"INSERT INTO actual_children_diagnosis 
                    VALUES (null, '{idDiagnosis}', '{idChild}')";
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
