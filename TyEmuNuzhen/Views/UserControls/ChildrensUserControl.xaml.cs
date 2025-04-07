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
using System.Windows.Shapes;
using TyEmuNuzhen.Views.Pages.Volonteer;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChildrensUserControl.xaml
    /// </summary>
    public partial class ChildrensUserControl : UserControl
    {
        private string _questUrl;

        public ChildrensUserControl(string id, string questNumber, string questURL, string fullName, DateTime birthDate, 
            DateTime dateDescriptionAdded, string description, string age, string photoPath, DateTime dateChildAdded, string isAlert)
        {
            InitializeComponent();
            this.Tag = id;
            questNumberTextBlock.Text += questNumber;
            fullNameTextBlock.Text = fullName;
            birthdayTextBlock.Text += birthDate.ToString("dd.MM.yyyy");
            ageTextBlock.Text += age;
            dateAddedTextBlock.Text += dateChildAdded.ToString("dd.MM.yyyy");
            _questUrl = questURL;
            if (isAlert == "1")
                alertTextBlock.Visibility = Visibility.Visible;
            descriptionTextBlock.Text += $"от {dateDescriptionAdded.ToString("dd.MM.yyyy")}: \r\n{description}";
            if (!string.IsNullOrEmpty(photoPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(photoPath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                childPhoto.ImageSource = bitmap;
            }
            else
                MessageBox.Show("Фотография не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void questURLBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = _questUrl,
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Ошибка открытия ссылки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void detailedBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new DetailChildInfoPage(this.Tag.ToString()));
        }
    }
}
