using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.IO;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для HospitalizationMedicalDirectionUserControl.xaml
    /// </summary>
    public partial class HospitalizationMedicalDirectionUserControl : UserControl
    {
        private string _pdfpng = "../../Images/Icons/pdf.png";
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private string _filePath;

        public HospitalizationMedicalDirectionUserControl(string filePath, byte typeDoc)
        {
            InitializeComponent();
            _filePath = filePath;
            switch (typeDoc)
            {
                case 1:
                    LoadMedicalDirection(filePath);
                    break;
                case 2:
                    LoadTicket(filePath);
                    break;
            }
        }

        private void LoadMedicalDirection(string filePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_pdfpng, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                photoImage.ImageSource = bitmap;
            }
            catch
            {
                BitmapImage errorBitmap = new BitmapImage();
                errorBitmap.BeginInit();
                errorBitmap.UriSource = new Uri(_errImagePath, UriKind.RelativeOrAbsolute);
                errorBitmap.CacheOption = BitmapCacheOption.OnLoad;
                errorBitmap.EndInit();
                photoImage.ImageSource = errorBitmap;
            }
            infoTextBlock.Text = $"Медицинское направление";
        }

        private void LoadTicket(string filePath)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                photoImage.ImageSource = bitmap;
            }
            catch
            {
                BitmapImage errorBitmap = new BitmapImage();
                errorBitmap.BeginInit();
                errorBitmap.UriSource = new Uri(_errImagePath, UriKind.RelativeOrAbsolute);
                errorBitmap.CacheOption = BitmapCacheOption.OnLoad;
                errorBitmap.EndInit();
                photoImage.ImageSource = errorBitmap;
            }
            infoTextBlock.Text = $"Билет";
        }

        private void photoBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                try
                {
                    string fullPath = Path.GetFullPath(_filePath);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = fullPath,
                        UseShellExecute = true
                    });
                }
                catch
                {
                    MessageBox.Show("Ошибка при открытии файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
