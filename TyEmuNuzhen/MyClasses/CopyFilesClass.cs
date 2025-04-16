using System;
using System.Windows;
using System.IO;

namespace TyEmuNuzhen.MyClasses
{
    internal class CopyFilesClass
    {
        private static string _imageSaveFolderPath = @"../../Images/Childrens/";
        private static string _documentSaveFolderPath = @"../../Documents/Children/";
        
        public static string CopyChildImage(string imageSourcePath, string idChild)
        {
            try
            {
                if (!Directory.Exists(_imageSaveFolderPath))
                {
                    Directory.CreateDirectory(_imageSaveFolderPath);
                }

                string fileName = DateTime.Now.ToString("dd_MM_yyyy_HHmm") + "_" + Path.GetFileName(imageSourcePath) + "_" + idChild;
                string newPath = Path.Combine(_imageSaveFolderPath, fileName);
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
                if (!Directory.Exists(_documentSaveFolderPath))
                {
                    Directory.CreateDirectory(_documentSaveFolderPath);
                }

                string fileName = Path.GetFileName(documentSourcePath) + "_" + idChild;
                string newPath = Path.Combine(_documentSaveFolderPath, fileName);

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
