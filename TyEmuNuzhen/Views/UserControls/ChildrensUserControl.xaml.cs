using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
using TyEmuNuzhen.Views.Pages.Curator;
using TyEmuNuzhen.Views.Pages.Volonteer;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChildrenCuratorUserControl.xaml
    /// </summary>
    public partial class ChildrensUserControl : UserControl
    {
        private string _questUrl;
        private int _role;
        private int _status;

        public ChildrensUserControl(string id, string questNumber, string questURL, string fullName, DateTime birthDate,
            DateTime dateDescriptionAdded, string description, string age, string regionName, string photoPath, DateTime dateChildAdded, string isAlert, int role, int status)
        {
            InitializeComponent();
            this.Tag = id;
            questNumberTextBlock.Text += questNumber;
            fullNameTextBlock.Text = fullName;
            birthdayTextBlock.Text += birthDate.ToString("dd.MM.yyyy");
            ageTextBlock.Text += age;
            if (regionName == null)
                regionTextBlock.Visibility = Visibility.Collapsed;
            regionTextBlock.Text += regionName;
            dateAddedTextBlock.Text += dateChildAdded.ToString("dd.MM.yyyy");
            _questUrl = questURL;
            _role = role;
            _status = status;
            if (status == 2)
                detailedBtn.Content = "Добавить информацию";
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
            if (_role == 1)
                NavigationService.GetNavigationService(this).Navigate(new DetailChildInfoPage(this.Tag.ToString()));
            if (_role == 2)
            {
                if (_status == 1)
                    NavigationService.GetNavigationService(this).Navigate(new Pages.Curator.ChildrensWork.DetailChildrenInfoCuratorPage(this.Tag.ToString()));
                else if (_status == 2)
                    NavigationService.GetNavigationService(this).Navigate(new Pages.Curator_To_Be_On_Time.PreliminaryInWork.AddCommonInfoChildrenPage(this.Tag.ToString()));
            }
        }
    }
}
