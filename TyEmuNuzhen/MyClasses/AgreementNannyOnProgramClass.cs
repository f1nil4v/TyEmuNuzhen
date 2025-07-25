﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с договорами нянь по программе
    /// </summary>
    internal class AgreementNannyOnProgramClass
    {
        public static DataTable dtAgreementNannyData;

        /// <summary>
        /// Получение данных договора няни по программе для печати
        /// </summary>
        /// <param name="idNannyOnProgram"></param>
        public static void GetAgreementNannyDataForPrint(string idNannyOnProgram)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT * FROM agreement_nanny_on_program 
                                                        WHERE idNannyOnProgram = '{idNannyOnProgram}' ORDER BY ID DESC LIMIT 1";
                dtAgreementNannyData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtAgreementNannyData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение максимального номера договора няни по программе
        /// </summary>
        /// <returns></returns>
        public static string GetMaxNumAgreementNanny()
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT MAX(numOfAgreement) FROM agreement_nanny_on_program";
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
        /// Добавление договора няни по программе в базу данных
        /// </summary>
        /// <param name="numAgreement"></param>
        /// <param name="idNannyOnProgram"></param>
        /// <param name="costPerDay"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool AddAgreement(string numAgreement, string idNannyOnProgram, string costPerDay, string filePath)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                DBConnection.myCommand.CommandText =
                    $"INSERT INTO agreement_nanny_on_program VALUES (null, '{numAgreement}', '{dateNow}', '{idNannyOnProgram}', '{costPerDay}', '{filePath}')";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи. \r\n{ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
