using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с деталями трансфера
    /// </summary>
    internal class TransferDetailClass
    {
        public static DataTable dtTransferDetailedSide1Data;
        public static DataTable dtTransferDetailedSide0Data;
        public static DataTable dtTransferDetailData;

        /// <summary>
        /// Получение данных о деталях трансфера для указанного идентификатора трансфера
        /// </summary>
        /// <param name="idTransfer"></param>
        /// <param name="side"></param>
        public static void GetTransferDetailsData(string idTransfer, bool side)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT transfer_detail.ID, transport_type.transportType, transfer_detail.cost, transfer_detail.filePath
                                                        FROM transfer_detail, transport_type
                                                        WHERE transfer_detail.idTransportType = transport_type.ID AND transfer_detail.idTransfer = {idTransfer}";
                if (side)
                {
                    dtTransferDetailedSide1Data = new DataTable();
                    DBConnection.myDataAdapter.Fill(dtTransferDetailedSide1Data);
                }
                else
                {
                    dtTransferDetailedSide0Data = new DataTable();
                    DBConnection.myDataAdapter.Fill(dtTransferDetailedSide0Data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Получение данных о деталях трансфера по идентификатору детали трансфера
        /// </summary>
        /// <param name="idTransferDetail"></param>
        public static void GetTransferDetailData(string idTransferDetail)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"SELECT *
                                                        FROM transfer_detail
                                                        WHERE ID = {idTransferDetail}";
                dtTransferDetailData = new DataTable();
                DBConnection.myDataAdapter.Fill(dtTransferDetailData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при выполнении запроса. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Добавление детали трансфера в базу данных
        /// </summary>
        /// <param name="idTransfer"></param>
        /// <param name="idTransportType"></param>
        /// <param name="cost"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool AddTransferDetail(string idTransfer, string idTransportType, string cost, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"INSERT INTO transfer_detail VALUES (null, '{idTransfer}', '{idTransportType}', '{cost}', '{filePath}')";
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
        /// Обновление детали трансфера в базе данных
        /// </summary>
        /// <param name="idTransferDetail"></param>
        /// <param name="idTransportType"></param>
        /// <param name="cost"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool UpdateTransferDetail(string idTransferDetail, string idTransportType, string cost, string filePath)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"UPDATE transfer_detail SET idTransportType = '{idTransportType}', cost = '{cost}', filePath = '{filePath}'";
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

        /// <summary>
        /// Удаление детали трансфера из базы данных
        /// </summary>
        /// <param name="idTranferDetail"></param>
        /// <returns></returns>
        public static bool DeleteTransferDetail(string idTranferDetail)
        {
            try
            {
                DBConnection.myCommand.CommandText = $@"DELETE FROM transfer_detail WHERE ID = '{idTranferDetail}'";
                if (DBConnection.myCommand.ExecuteNonQuery() > 0)
                    return true;
                else
                    return false;
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
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
