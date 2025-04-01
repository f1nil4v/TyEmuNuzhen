using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class DBConnection
    {
        public static MySqlDataAdapter myDataAdapter;
        static string connectionString = @"Database = tyemunuzhen_db;
                                           Data Source = localhost;
                                           user = root;
                                           password = P@ssw0rd;
                                           charset = utf8;";

        static MySqlConnection myConnection;
        public static MySqlCommand myCommand;

        public static bool Connect_DB()
        {
            try
            {
                myConnection = new MySqlConnection(connectionString);
                myConnection.Open();
                myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myDataAdapter = new MySqlDataAdapter(myCommand);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при подключении к БД. \r\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static void Disconnect_DB()
        {
            myConnection.Close();
        }
    }
}
