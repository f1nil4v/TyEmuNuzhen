using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Documents;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с диагнозами детей в рамках консультаций.
    /// </summary>
    internal class ChildrenDiagnosisClass
    {
        public static DataTable dtChildrenDiagnoses;

        /// <summary>
        /// Получение списка диагнозов в рамках консультаций ребенка по ID ребёнка.
        /// </summary>
        /// <param name="idConsultation"></param>
        public static void GetChildrenDiagnoses(string idConsultation)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT diagnoses.diagnosisName
                    FROM children_diagnosis, diagnoses
                    WHERE children_diagnosis.idDiagnosis = diagnoses.ID AND
                        children_diagnosis.idConsultation = '{idConsultation}'
                    ORDER BY diagnoses.diagnosisName";
                dtChildrenDiagnoses = new DataTable();
                DBConnection.myDataAdapter.Fill(dtChildrenDiagnoses);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавление диагноза в рамках консультации в таблицу БД.
        /// </summary>
        /// <returns></returns>
        public static bool AddChildrenDiagnosis()
        {
            try
            {
                string idConsultation = ConsultationClass.GetLastIdConsultation();
                foreach (string idDiagnosis in DiagnosesClass.selectedIDDiagnoses)
                {
                    DBConnection.myCommand.CommandText = $@"INSERT INTO children_diagnosis 
                    VALUES (null, '{idConsultation}','{idDiagnosis}')";
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