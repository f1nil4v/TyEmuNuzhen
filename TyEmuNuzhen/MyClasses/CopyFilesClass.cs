using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TyEmuNuzhen.MyClasses
{
    internal class CopyFilesClass
    {
        public static string CopyChildImage(string photoChild)
        {
            if (!string.IsNullOrEmpty(photoChild))
            {
                try
                {
                    string sourceFilePath = photoChild;
                    string fileName = System.IO.Path.GetFileName(sourceFilePath);

                    string relativeDirectoryPath = "../../Images/Childrens";
                    string relativePath = $"{relativeDirectoryPath}/{fileName}";

                    string physicalDirectoryPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, relativeDirectoryPath));

                    if (!System.IO.Directory.Exists(physicalDirectoryPath))
                    {
                        System.IO.Directory.CreateDirectory(physicalDirectoryPath);
                    }

                    string destinationPath = System.IO.Path.Combine(physicalDirectoryPath, fileName);
                    System.IO.File.Copy(sourceFilePath, destinationPath, true);

                    return relativePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при копировании файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            else
                return "Выберете изображение!";
        }
    }
}
