using System;
using System.Windows;
using System.IO;

namespace TyEmuNuzhen.MyClasses
{
    /// <summary>
    /// Класс для работы с файлами
    /// </summary>
    internal class CopyFilesClass
    {
        /// <summary>
        /// Копироваение изображения ребёнка в директорию проекта
        /// </summary>
        /// <param name="imageSourcePath"></param>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static string CopyChildImage(string imageSourcePath, string idChild)
        {
            try
            {
                string imageSaveFolderPath = @"../../Images/Childrens/";
                if (!Directory.Exists(imageSaveFolderPath))
                {
                    Directory.CreateDirectory(imageSaveFolderPath);
                }

                string fileName = Path.GetFileName(imageSourcePath);
                string newPath = Path.Combine(imageSaveFolderPath, fileName);
                if (File.Exists(newPath))
                    throw new Exception($"файл уже существует. Прикрепите другой файл.");
                File.Copy(imageSourcePath, newPath, true);
                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Копирование документа ребёнка в директорию проекта
        /// </summary>
        /// <param name="documentSourcePath"></param>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static string CopyChildDocument(string documentSourcePath, string idChild)
        {
            try
            {
                string documentSaveFolderPath = @"../../Documents/Children/Documents/";
                if (!Directory.Exists(documentSaveFolderPath))
                {
                    Directory.CreateDirectory(documentSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath);
                string newPath = Path.Combine(documentSaveFolderPath, fileName);

                if (File.Exists(newPath))
                    throw new Exception($"файл уже существует. Прикрепите другой файл.");

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Копирование медицинского заключения ребёнка в директорию проекта
        /// </summary>
        /// <param name="documentSourcePath"></param>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static string CopyChildMedicalConclusion(string documentSourcePath, string idChild)
        {
            try
            {
                string medicalConclusionsSaveFolderPath = @"../../Documents/Children/MedicalConclusion/";
                if (!Directory.Exists(medicalConclusionsSaveFolderPath))
                {
                    Directory.CreateDirectory(medicalConclusionsSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath);
                string newPath = Path.Combine(medicalConclusionsSaveFolderPath, fileName);

                if (File.Exists(newPath))
                    throw new Exception($"файл уже существует. Прикрепите другой файл.");

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Копирование медицинских результатов ребёнка в директорию проекта
        /// </summary>
        /// <param name="documentSourcePath"></param>
        /// <param name="idChild"></param>
        /// <returns></returns>
        public static string CopyChildMedicalResults(string documentSourcePath, string idChild)
        {
            try
            {
                string medicalResultsSaveFolderPath = @"../../Documents/Children/MedicalResults/";
                if (!Directory.Exists(medicalResultsSaveFolderPath))
                {
                    Directory.CreateDirectory(medicalResultsSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath);
                string newPath = Path.Combine(medicalResultsSaveFolderPath, fileName);

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Копирование медицинского направления на госпитализацию в директорию проекта
        /// </summary>
        /// <param name="documentSourcePath"></param>
        /// <returns></returns>
        public static string CopyHospitalizationMedicalDirection(string documentSourcePath)
        {
            try
            {
                string medicalDirectionSaveFolderPath = @"../../Documents/Children/Hospitalization/MedicalDirection/";
                if (!Directory.Exists(medicalDirectionSaveFolderPath))
                {
                    Directory.CreateDirectory(medicalDirectionSaveFolderPath);
                }
                string fileName = Path.GetFileName(documentSourcePath);
                string newPath = Path.Combine(medicalDirectionSaveFolderPath, fileName);

                if (File.Exists(newPath))
                    throw new Exception($"файл уже существует. Прикрепите другой файл.");

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Копирование билета в директорию проекта
        /// </summary>
        /// <param name="documentSourcePath"></param>
        /// <returns></returns>
        public static string CopyTicket(string documentSourcePath)
        {
            try
            {
                string ticketSaveFolderPath = @"../../Documents/Children/Hospitalization/Tickets/";
                if (!Directory.Exists(ticketSaveFolderPath))
                {
                    Directory.CreateDirectory(ticketSaveFolderPath);
                }
                string fileName = Path.GetFileName(documentSourcePath);
                string newPath = Path.Combine(ticketSaveFolderPath, fileName);

                if (File.Exists(newPath))
                    throw new Exception($"файл уже существует. Прикрепите другой файл.");

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        /// <summary>
        /// Скачивание файла из директории проекта в указанную директорию пользователя
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <returns></returns>
        public static bool DownloadFile(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                if (!File.Exists(sourceFilePath))
                    throw new Exception("файл не найден по указанному пути.");
                File.Copy(sourceFilePath, destinationFilePath, true);
                MessageBox.Show("Файл успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        /// <summary>
        /// Удаление файла из директории проекта
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFile(string filePath)
        {
            try
            {

                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Файл не найден по указанному пути.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                File.Delete(filePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        
    }
}
