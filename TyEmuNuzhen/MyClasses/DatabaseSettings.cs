using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для хранения настроек подключения к базе данных.
    /// </summary>
    [Serializable]
    public class DatabaseSettings
    {
        /// <summary>
        /// Сервер базы данных
        /// </summary>
        public string Server { get; set; } = "localhost";
        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string Database { get; set; } = "tyemunuzhen_db";
        /// <summary>
        /// Имя пользователя для подключения к базе данных
        /// </summary>
        public string Username { get; set; } = "root";
        /// <summary>
        /// Пароль для подключения к базе данных
        /// </summary>
        public string Password { get; set; } = "P@ssw0rd";
        /// <summary>
        /// Кодировка, используемая для подключения к базе данных
        /// </summary>
        public string Charset { get; set; } = "utf8";

        /// <summary>
        /// Получение строки подключения к базе данных в формате, используемом MySQL.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return $"Database={Database};Data Source={Server};user={Username};Password={Password};charset={Charset};";
        }

        /// <summary>
        /// Путь к файлу настроек базы данных
        /// </summary>
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TyEmuNuzhen",
            "dbsettings.xml");

        /// <summary>
        /// Сохранение настроек подключения к базе данных в файл XML
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static bool SaveSettings(DatabaseSettings settings)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));

                XmlSerializer serializer = new XmlSerializer(typeof(DatabaseSettings));
                using (FileStream fs = new FileStream(SettingsPath, FileMode.Create))
                {
                    serializer.Serialize(fs, settings);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Загрузка настроек подключения к базе данных из файла XML
        /// </summary>
        /// <returns></returns>
        public static DatabaseSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DatabaseSettings));
                    using (FileStream fs = new FileStream(SettingsPath, FileMode.Open))
                    {
                        return (DatabaseSettings)serializer.Deserialize(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке настроек: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new DatabaseSettings();
        }
    }
}
