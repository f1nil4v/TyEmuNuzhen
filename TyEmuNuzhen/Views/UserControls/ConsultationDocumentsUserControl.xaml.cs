using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;
using System.Windows;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ConsultationDocumentsUserControl.xaml
    /// </summary>
    public partial class ConsultationDocumentsUserControl : UserControl
    {
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private string _pdfpng = "../../Images/Icons/pdf.png";
        private string _filePath;
        private bool _isConsultationResult;
        private int _indexOfFilePath;

        public delegate void DeleteRequestEventHandler();
        public event DeleteRequestEventHandler DeleteRequested;

        public ConsultationDocumentsUserControl(bool isConsultationResult, int index, string filePath)
        {
            InitializeComponent();
            _isConsultationResult = isConsultationResult;
            _indexOfFilePath = index;
            _filePath = filePath;
            if (isConsultationResult)
                LoadDocumentScan(filePath);
            else
                LoadDocument(filePath);
        }

        private void LoadDocument(string filePath)
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
            infoTextBlock.Text = $"{Path.GetFileNameWithoutExtension(filePath)}";
        }

        private void LoadDocumentScan(string filePath)
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
            infoTextBlock.Text = $"{Path.GetFileNameWithoutExtension(filePath)}";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_isConsultationResult)
            {
                ResultConsultationClass.oldFilePaths.RemoveAt(_indexOfFilePath);
                DeleteRequested?.Invoke();
            }
            else
                DeleteRequested?.Invoke();
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
