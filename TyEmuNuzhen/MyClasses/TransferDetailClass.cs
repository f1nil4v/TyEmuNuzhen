using System;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class TransferDetailClass
    {
        public static DataTable dtTransferDetailedSide1Data;
        public static DataTable dtTransferDetailedSide0Data;
        public static DataTable dtTransferDetailData;

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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при удалении записи. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
