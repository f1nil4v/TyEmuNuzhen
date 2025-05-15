using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace TyEmuNuzhen.MyClasses
{
    [Serializable]
    public class DatabaseSettings
    {
        public string Server { get; set; } = "localhost";
        public string Database { get; set; } = "tyemunuzhen_db";
        public string Username { get; set; } = "root";
        public string Password { get; set; } = "P@ssw0rd";
        public string Charset { get; set; } = "utf8";

        public string GetConnectionString()
        {
            return $"Database={Database};Data Source={Server};user={Username};Password={Password};charset={Charset};";
        }

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TyEmuNuzhen",
            "dbsettings.xml");

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
