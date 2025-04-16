using System;
using System.Windows;
using System.IO;

namespace TyEmuNuzhen.MyClasses
{
    internal class CopyFilesClass
    {
        public static string CopyChildImage(string imageSourcePath, string idChild)
        {
            try
            {
                string imageSaveFolderPath = @"../../Images/Childrens/";
                if (!Directory.Exists(imageSaveFolderPath))
                {
                    Directory.CreateDirectory(imageSaveFolderPath);
                }

                string fileName = DateTime.Now.ToString("dd_MM_yyyy_HHmm") + "_" + Path.GetFileName(imageSourcePath) + "_" + idChild;
                string newPath = Path.Combine(imageSaveFolderPath, fileName);
                if (File.Exists(newPath) && idChild == null)
                    newPath += "_";
                File.Copy(imageSourcePath, newPath, true);
                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static string CopyChildDocument(string documentSourcePath, string idChild)
        {
            try
            {
                string documentSaveFolderPath = @"../../Documents/Children/Documents/";
                if (!Directory.Exists(documentSaveFolderPath))
                {
                    Directory.CreateDirectory(documentSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath) + "_" + idChild;
                string newPath = Path.Combine(documentSaveFolderPath, fileName);

                if (File.Exists(newPath) && idChild == null)
                    newPath += "_";

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static string CopyChildMedicalConclusion(string documentSourcePath, string idChild)
        {
            try
            {
                string medicalConclusionsSaveFolderPath = @"../../Documents/Children/MedicalConclusion/";
                if (!Directory.Exists(medicalConclusionsSaveFolderPath))
                {
                    Directory.CreateDirectory(medicalConclusionsSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath) + "_" + idChild;
                string newPath = Path.Combine(medicalConclusionsSaveFolderPath, fileName);

                if (File.Exists(newPath) && idChild == null)
                    newPath += "_";

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static string CopyChildMedicalResults(string documentSourcePath, string idChild)
        {
            try
            {
                string medicalResultsSaveFolderPath = @"../../Documents/Children/MedicalResults/";
                if (!Directory.Exists(medicalResultsSaveFolderPath))
                {
                    Directory.CreateDirectory(medicalResultsSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath) + "_" + idChild;
                string newPath = Path.Combine(medicalResultsSaveFolderPath, fileName);

                if (File.Exists(newPath) && idChild == null)
                    newPath += "_";

                File.Copy(documentSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
