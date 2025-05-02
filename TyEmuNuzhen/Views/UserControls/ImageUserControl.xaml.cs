using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using System.Windows;
using TyEmuNuzhen.MyClasses;
using Microsoft.Win32;

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
                    btnDownload.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    LoadDocument(filePath, documentType);
                    break;
                case 2:
                    LoadAppealsConsents(filePath, dateFile);
                    break;
                case 3:
                    LoadWordDocument(isFirst, filePath, dateFile);
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

        private void LoadWordDocument(bool isFirst, string filePath, string dateFile)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_wordpng, UriKind.RelativeOrAbsolute);
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
                infoTextBlock.Text = $"{Path.GetFileNameWithoutExtension(filePath)} от {dateFile} (последнее)";
            else
                infoTextBlock.Text = $"{Path.GetFileNameWithoutExtension(filePath)} от {dateFile}";

        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            string filter;
            switch (_objectType)
            {
                case 1:
                    filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                    break;
                case 2:
                    filter = "Документы PDF (*.pdf)|*.pdf";
                    break;
                case 3:
                    filter = "Документы Word (*.docx)|*.docx";
                    break;
                default:
                    filter = "Все файлы (*.*)|*.*";
                    break;
            }
            string originalFileName = Path.GetFileName(_filePath);
            var saveFileDialog = new SaveFileDialog
            {
                FileName = originalFileName,
                Filter = filter
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedPath = saveFileDialog.FileName;
                CopyFilesClass.DownloadFile(_filePath, selectedPath);
            }
        }


    }
}
