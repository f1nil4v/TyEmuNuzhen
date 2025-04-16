using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace TyEmuNuzhen.MyClasses
{
    internal class CopyFilesClass
    {
        private static string _imageSaveFolderPath = @"../../Images/Childrens/";
        private static string _documentSaveFolderPath = @"../../Documents/Children/";
        
        public static string CopyChildImage(string imageSourcePath)
        {
            try
            {
                if (!Directory.Exists(_imageSaveFolderPath))
                {
                    Directory.CreateDirectory(_imageSaveFolderPath);
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmm") + "_" + Path.GetFileName(imageSourcePath);
                string newPath = Path.Combine(_imageSaveFolderPath, fileName);

                File.Copy(imageSourcePath, newPath, true);

                return newPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public static string CopyChildDocument(string documentSourcePath)
        {
            try
            {
                if (!Directory.Exists(_documentSaveFolderPath))
                {
                    Directory.CreateDirectory(_documentSaveFolderPath);
                }

                string fileName = DateTime.Now.ToString("yyyyMMddHHmm") + "_" + Path.GetFileName(documentSourcePath);
                string newPath = Path.Combine(_documentSaveFolderPath, fileName);

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
