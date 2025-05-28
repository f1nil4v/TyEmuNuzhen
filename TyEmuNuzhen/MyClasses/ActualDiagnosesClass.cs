using MySql.Data.MySqlClient;
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
    /// Класс для работы с актуальными диагнозами детей
    /// </summary>
    internal class ActualDiagnosesClass
    {
        public static DataTable dtActualChildrenDiagnoses;

        /// <summary>
        /// Получение списка актуальных диагнозов ребенка из таблицы actual_children_diagnosis
        /// </summary>
        /// <param name="idChild"></param>
        public static void GetChildrenAcutualDiagnoses(string idChild)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT actual_children_diagnosis.ID, diagnoses.diagnosisName
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

        /// <summary>
        /// Добавление актуальных диагнозов в таблицу actual_children_diagnosis для конкретного ребенка
        /// </summary>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static bool AddChildrenDiagnosis(string idChild)
        {
            try
            {
                foreach (string idDiagnosis in DiagnosesClass.selectedIDDiagnoses)
                {
                    DBConnection.myCommand.CommandText = $@"INSERT INTO actual_children_diagnosis 
                    VALUES (null, '{idDiagnosis}', '{idChild}')";
                    if (DBConnection.myCommand.ExecuteNonQuery() <= 0)
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

        /// <summary>
        /// Удаление актуальных диагнозов из таблицы actual_children_diagnosis для конкретного ребенка
        /// </summary>
        /// <param name="selectedDiagnosisIds"></param>
        /// <returns></returns>
        public static bool DeleteChildrenDiagnosis(List<string> selectedDiagnosisIds)
        {
            try
            {
                foreach (string idDiagnosis in selectedDiagnosisIds)
                {
                    DBConnection.myCommand.CommandText = $@"DELETE FROM actual_children_diagnosis 
                    WHERE ID = '{idDiagnosis}'";
                    if (DBConnection.myCommand.ExecuteNonQuery() <= 0)
                        return false;
                }
                return true;
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    MessageBox.Show($"Запись не может быть удалена, так как она используется в других таблицах.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении диагноза(-ов). \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
