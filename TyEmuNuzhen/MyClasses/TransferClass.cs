using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с данными о трансфере
    /// </summary>
    internal class TransferClass
    {
        public static DataTable dtTransferOnHozpitalizationSide1Data;
        public static DataTable dtTransferOnHozpitalizationSide0Data;
        public static DataTable dtTransferOnHozpitalizationData;

        /// <summary>
        /// Получение данных о трансфере по ID госпитализации в медучреждение
        /// </summary>
        /// <param name="idHospitalization"></param>
        public static void GetTransferOnHozpitalizationSide1Data(string idHospitalization)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT ID, dateDeparture, dateArrival, totalCost
                                                        FROM transfer
                                                        WHERE idHospitalization = '{idHospitalization}' AND transferSide = 1";
                dtTransferOnHozpitalizationSide1Data = new DataTable();
                DBConnection.myDataAdapter.Fill(dtTransferOnHozpitalizationSide1Data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных о трансфере по ID госпитализации из медучреждения
        /// </summary>
        /// <param name="idHospitalization"></param>
        public static void GetTransferOnHozpitalizationSide0Data(string idHospitalization)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT ID, dateDeparture, dateArrival, totalCost
                                                        FROM transfer
                                                        WHERE idHospitalization = '{idHospitalization}' AND transferSide = 0";
                dtTransferOnHozpitalizationSide0Data = new DataTable();
                DBConnection.myDataAdapter.Fill(dtTransferOnHozpitalizationSide0Data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных о трансфере по ID трансфера
        /// </summary>
        /// <param name="idTransfer"></param>
        public static void GetTransferOnHozpitalizationData(string idTransfer)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT *
                                                        FROM transfer
                                                        WHERE ID = '{idTransfer}'";
                dtTransferOnHozpitalizationData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtTransferOnHozpitalizationData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавление нового трансфера
        /// </summary>
        /// <param name="idHospitalization"></param>
        /// <param name="dateDeparture"></param>
        /// <param name="dateArrival"></param>
        /// <param name="transferSide"></param>
        /// <returns></returns>
        public static bool AddTransfer(string idHospitalization, string dateDeparture, string dateArrival, string transferSide)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"INSERT INTO transfer VALUES (null, '{idHospitalization}', '{dateDeparture}', '{dateArrival}', 0, '{transferSide}')";
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
        /// Обновление информации о трансфере
        /// </summary>
        /// <param name="idTransfer"></param>
        /// <param name="dateDeparture"></param>
        /// <param name="dateArrival"></param>
        /// <returns></returns>
        public static bool UpdateTransfer(string idTransfer, string dateDeparture, string dateArrival)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE transfer SET dateDeparture = '{dateDeparture}', dateArrival = '{dateArrival}' WHERE ID = '{idTransfer}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлнении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
