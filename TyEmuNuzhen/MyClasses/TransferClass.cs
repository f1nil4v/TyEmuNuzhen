using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class TransferClass
    {
        public static DataTable dtTransferOnHozpitalizationSide1Data;
        public static DataTable dtTransferOnHozpitalizationSide0Data;
        public static DataTable dtTransferOnHozpitalizationData;

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
