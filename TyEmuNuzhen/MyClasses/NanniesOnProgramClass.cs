using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с нянями в программе
    /// </summary>
    internal class NanniesOnProgramClass
    {
        public static DataTable dtActiveNannyOnProgramData;
        public static DataTable dtHistoryNannyOnProgramData;
        public static DataTable dtHistoryNannyOnProgramDataForPrint;

        /// <summary>
        /// Получение данных активной няни в программе
        /// </summary>
        /// <param name="idActualProgram"></param>
        public static void GetActiveNannyOnProgramData(string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT nannies_on_program.ID, nannies_on_program.idNanny, CONCAT_WS(' ', nannies.surname, nannies.name, IFNULL(nannies.middleName, '')) as 'fullName', nannies.phoneNumber, nannies.email,
                                                            agreement_nanny_on_program.dateConclusion, agreement_nanny_on_program.costPerDay, agreement_nanny_on_program.filePath
                                                        FROM nannies, nannies_on_program, agreement_nanny_on_program
                                                        WHERE nannies_on_program.idNanny = nannies.ID AND agreement_nanny_on_program.idNannyOnProgram = nannies_on_program.ID 
                                                            AND nannies_on_program.idActualProgram = '{idActualProgram}' AND nannies.status = 1
                                                        ORDER BY nannies_on_program.ID DESC LIMIT 1";
                dtActiveNannyOnProgramData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtActiveNannyOnProgramData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных истории нянь в программе
        /// </summary>
        /// <param name="idActualProgram"></param>
        public static void GetHistoryNannyOnProgramData(string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT nannies_on_program.ID, CONCAT_WS(' ', nannies.surname, nannies.name, IFNULL(nannies.middleName, '')) as 'fullName', nannies.phoneNumber, nannies.email,
                                                            agreement_nanny_on_program.dateConclusion, agreement_nanny_on_program.costPerDay, act_of_completed_works.countWorkDays,
                                                            agreement_nanny_on_program.filePath AS 'agreementPath', act_of_completed_works.filePath AS 'actPath'
                                                        FROM nannies, nannies_on_program, agreement_nanny_on_program, act_of_completed_works
                                                        WHERE nannies_on_program.idNanny = nannies.ID AND agreement_nanny_on_program.idNannyOnProgram = nannies_on_program.ID 
                                                            AND act_of_completed_works.idNannyOnProgram = nannies_on_program.ID AND nannies_on_program.idActualProgram = '{idActualProgram}'
                                                            AND nannies_on_program.status = 0
                                                        ORDER BY nannies_on_program.ID DESC";
                dtHistoryNannyOnProgramData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtHistoryNannyOnProgramData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных истории нянь в программе для печати
        /// </summary>
        /// <param name="idActualProgram"></param>
        public static void GetHistoryNannyOnProgramDataForPrint(string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT CONCAT_WS(' ', nannies.surname, nannies.name, IFNULL(nannies.middleName, '')) as 'fullName', agreement_nanny_on_program.costPerDay, act_of_completed_works.countWorkDays,
                                                            act_of_completed_works.payment
                                                        FROM nannies, nannies_on_program, agreement_nanny_on_program, act_of_completed_works
                                                        WHERE nannies_on_program.idNanny = nannies.ID AND agreement_nanny_on_program.idNannyOnProgram = nannies_on_program.ID 
                                                            AND act_of_completed_works.idNannyOnProgram = nannies_on_program.ID AND nannies_on_program.idActualProgram = '{idActualProgram}'
                                                            AND nannies_on_program.status = 0
                                                        ORDER BY nannies_on_program.ID DESC";
                dtHistoryNannyOnProgramDataForPrint = new DataTable();
                DBConnection.myDataAdapter.Fill(dtHistoryNannyOnProgramDataForPrint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение ID последней няни в программе по ID няни
        /// </summary>
        /// <param name="idNanny"></param>
        /// <returns></returns>
        public static string GetLastNannyOnProgramID(string idNanny)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(ID) FROM nannies_on_program";
                Object result = DBConnection.myCommand.ExecuteScalar();
                if (result != null)
                    return result.ToString();
                else
                    return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Добавление няни в программу
        /// </summary>
        /// <param name="idNanny"></param>
        /// <param name="idActualProgram"></param>
        /// <returns></returns>
        public static bool AddNannyOnProgram(string idNanny, string idActualProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"INSERT INTO nannies_on_program VALUES (null, '{idNanny}', '{idActualProgram}', '1')";
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

        /// <summary>
        /// Обновление статуса няни в программе
        /// </summary>
        /// <param name="idNannyOnProgram"></param>
        /// <returns></returns>
        public static bool UpdateStatusNannyOnProgram(string idNannyOnProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE nannies_on_program SET status = 0 WHERE ID = '{idNannyOnProgram}'";
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
