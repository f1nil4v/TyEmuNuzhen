using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using System.Windows;
using MaterialDesignThemes.Wpf;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ImageUserControl.xaml
    /// </summary>
    public partial class ImageUserControl : UserControl
    {
        private int _objectType;
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private string _wordpng = "../../Images/Icons/word.png";
        private string _pdfpng = "../../Images/Icons/pdf.png";
        private string _excelpng = "../../Images/Icons/excel.png";
        private string _filePath;

        public ImageUserControl(int objectType, bool isFirst, string filePath, string dateFile, string documentType)
        {
            InitializeComponent();
            _objectType = objectType;
            _filePath = filePath;
            switch (objectType)
            {
                case 0:
                    LoadImage(isFirst, filePath, dateFile);
                    break;
                case 1:
                    LoadDocument(filePath, documentType);
                    photoBorder.Cursor = Cursors.Hand;
                    break;
                case 2:
                    LoadAppealsConsents(filePath, dateFile);
                    photoBorder.Cursor = Cursors.Hand;
                    break;
                default:
                    infoTextBlock.Text = "Неизвестный тип объекта";
                    break;
            }
        }

        private void LoadImage(bool isFirst, string filePath, string dateFile)
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
            if (isFirst)
                infoTextBlock.Text = dateFile + " (последнее)";
            else
                infoTextBlock.Text = dateFile;
        }

        private void LoadDocument(string filePath, string documentType)
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
            infoTextBlock.Text = documentType;
        }

        private void LoadAppealsConsents(string filePath, string dateFile)
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
            infoTextBlock.Text = $"{Path.GetFileNameWithoutExtension(filePath)} от {dateFile}";
        }

        private void photoBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_objectType != 0)
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
}
