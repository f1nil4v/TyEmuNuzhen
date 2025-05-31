using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Pages.Director.Settings
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            HelpManagerClass.CurrentHelpKey = "DirectorSettingsPage";
        }

        private void btnExportDB_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "SQL файлы (*.sql)|*.sql|Все файлы (*.*)|*.*",
                Title = "Сохранить резервную копию базы данных",
                FileName = $"backup_{DateTime.Now:yyyy-MM-dd_HH-mm}.sql"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    ExportDatabase(saveFileDialog.FileName);
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("Резервная копия базы данных успешно создана!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show($"Ошибка при создании резервной копии:\n{ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnImportDB_Click(object sender, RoutedEventArgs e)
        {
            // Предупреждение о последствиях
            var result = MessageBox.Show(
                "ВНИМАНИЕ! Восстановление базы данных приведет к замене всех существующих данных!\n\n" +
                "Рекомендуется сначала создать резервную копию текущей базы данных.\n\n" +
                "Вы уверены, что хотите продолжить?",
                "Предупреждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var openFileDialog = new OpenFileDialog
            {
                Filter = "SQL файлы (*.sql)|*.sql|Все файлы (*.*)|*.*",
                Title = "Выберите файл резервной копии"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                    ImportDatabase(openFileDialog.FileName);
                    Mouse.OverrideCursor = null;
                    MessageBox.Show("База данных успешно восстановлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show($"Ошибка при восстановлении базы данных:\n{ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExportDatabase(string filePath)
        {
            var settings = DBConnection.Settings;
            try
            {
                string mysqldumpPath = @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe";

                using (var process = new Process())
                {
                    process.StartInfo.FileName = mysqldumpPath;
                    process.StartInfo.Arguments = $"-u{settings.Username} -p{settings.Password} " +
                                                   $"-h{settings.Server} {settings.Database} " +
                                                   "--default-character-set=utf8mb4 " +
                                                   "--routines --triggers --events " +
                                                   "--single-transaction --quick";
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    process.Start();

                    using (var fileStream = new StreamWriter(filePath, false, Encoding.UTF8))
                    {
                        fileStream.WriteLine("SET NAMES utf8mb4;");
                        fileStream.Write(process.StandardOutput.ReadToEnd());
                    }

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        string errorOutput = process.StandardError.ReadToEnd();
                        throw new Exception($"mysqldump завершился с кодом ошибки {process.ExitCode}:\n{errorOutput}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось экспортировать базу данных: {ex.Message}", ex);
            }
        }

        private void ImportDatabase(string filePath)
        {
            try
            {
                string mysqlPath = @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe";
                var settings = DBConnection.Settings;

                string arguments = $"/C \"\"{mysqlPath}\" -u{settings.Username} -p{settings.Password} " +
                                   $"-h{settings.Server} {settings.Database} " +
                                   $"--default-character-set=utf8mb4 < \"{filePath}\"\"";

                var psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Импорт завершился с ошибкой (код {process.ExitCode}):\n{error}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Не удалось импортировать базу данных: {ex.Message}", ex);
            }
        }
    }
}
