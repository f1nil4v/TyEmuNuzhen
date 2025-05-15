using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ConnectDBWindow.xaml
    /// </summary>
    public partial class ConnectDBWindow : Window
    {
        private DatabaseSettings currentSettings;

        public ConnectDBWindow()
        {
            InitializeComponent();
            currentSettings = DBConnection.Settings ?? new DatabaseSettings();
            txtServer.Text = currentSettings.Server;
            txtDatabase.Text = currentSettings.Database;
            txtUsername.Text = currentSettings.Username;
            txtPassword.Password = currentSettings.Password;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            DatabaseSettings testSettings = GetSettingsFromFields();
            if (DBConnection.TestConnection(testSettings))
            {
                MessageBox.Show("Соединение успешно установлено!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            DatabaseSettings newSettings = GetSettingsFromFields();
            if (DBConnection.SaveSettings(newSettings))
            {
                MessageBox.Show("Настройки успешно сохранены!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private DatabaseSettings GetSettingsFromFields()
        {

            return new DatabaseSettings
            {
                Server = txtServer.Text,
                Database = txtDatabase.Text,
                Username = txtUsername.Text,
                Password = txtPassword.Password
            };
        }
    }
}
