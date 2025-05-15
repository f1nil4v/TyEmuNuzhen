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
        static MySqlConnection myConnection;
        public static MySqlCommand myCommand;

        public static DatabaseSettings Settings { get; private set; }

        static DBConnection()
        {
            LoadSettings();
        }

        public static void LoadSettings()
        {
            Settings = DatabaseSettings.LoadSettings();
        }

        public static bool SaveSettings(DatabaseSettings newSettings)
        {
            if (DatabaseSettings.SaveSettings(newSettings))
            {
                Settings = newSettings;
                return true;
            }
            return false;
        }

        public static bool Connect_DB()
        {
            try
            {
                myConnection = new MySqlConnection(Settings.GetConnectionString());
                myConnection.Open();
                myCommand = new MySqlCommand();
                myCommand.Connection = myConnection;
                myDataAdapter = new MySqlDataAdapter(myCommand);
                return true;
            }
            catch (MySqlException ex)
            {
                string userMessage = GetUserFriendlyError(ex);
                MessageBox.Show(userMessage, "Ошибка подключения к БД", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка при подключении к БД.\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool TestConnection(DatabaseSettings testSettings)
        {
            MySqlConnection testConnection = null;
            try
            {
                testConnection = new MySqlConnection(testSettings.GetConnectionString());
                testConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                string userMessage = GetUserFriendlyError(ex);
                MessageBox.Show(userMessage, "Ошибка подключения к БД", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка при подключении к БД.\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                if (testConnection != null && testConnection.State == System.Data.ConnectionState.Open)
                {
                    testConnection.Close();
                }
            }
        }

        public static void Disconnect_DB()
        {
            if (myConnection != null && myConnection.State == System.Data.ConnectionState.Open)
            {
                myConnection.Close();
            }
        }

        private static string GetUserFriendlyError(MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    return "Не удалось подключиться к серверу базы данных. Проверьте настройки подключения и доступность сервера.";
                case 1042:
                    return "Не удается подключиться к серверу MySQL. Проверьте адрес сервера";
                case 1045:
                    return "Неверное имя пользователя или пароль для подключения к базе данных.";
                case 1049:
                    return "Указанная база данных не существует.";
                case 2003:
                    return "Нет соединения с сервером MySQL (сервер не отвечает или неверный порт).";
                case 2005:
                    return "Сервер MySQL с таким именем не найден.";
                default:
                    return $"Ошибка MySQL ({ex.Number}): {ex.Message}";
            }
        }
    }
}
