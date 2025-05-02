using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChildrensCuratorUserControl.xaml
    /// </summary>
    public partial class ChildrensCuratorUserControl : UserControl
    {
        private string _fullNameChild;
        private byte _status;

        public ChildrensCuratorUserControl(string id, string fullName, DateTime birthDate,
            string age, string regionName, string orphanage, string photoPath, DateTime dateChildAdded, byte status)
        {
            InitializeComponent();
            this.Tag = id;
            fullNameTextBlock.Text = fullName;
            birthdayTextBlock.Text += birthDate.ToString("dd.MM.yyyy");
            ageTextBlock.Text += age;
            if (regionName == null)
                regionTextBlock.Visibility = Visibility.Collapsed;
            regionTextBlock.Text += regionName;
            orphanageTextBlock.Text += orphanage;
            dateAddedTextBlock.Text += dateChildAdded.ToString("dd.MM.yyyy");
            _status = status;
            switch (status)
            {
                case 1:
                    nanniesBtn.Visibility = Visibility.Collapsed;
                    documentsBtn.Visibility = Visibility.Collapsed;
                    transferBtn.Visibility = Visibility.Collapsed;
                    hospitalizationBtn.Visibility = Visibility.Collapsed;
                    completeWorkBtn.Visibility = Visibility.Collapsed;
                    statusTextBlock.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    consultationBtn.Visibility = Visibility.Collapsed;
                    break;

            }
            _fullNameChild = fullName;
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

        public ChildrensCuratorUserControl(string id, string fullName, DateTime birthDate,
            string age, string regionName, string orphanage, string photoPath, DateTime dateChildAdded, string statusProgram, int statusProgramSw)
        {
            InitializeComponent();
            this.Tag = id;
            consultationBtn.Visibility = Visibility.Collapsed;
            fullNameTextBlock.Text = fullName;
            birthdayTextBlock.Text += birthDate.ToString("dd.MM.yyyy");
            ageTextBlock.Text += age;
            if (regionName == null)
                regionTextBlock.Visibility = Visibility.Collapsed;
            regionTextBlock.Text += regionName;
            orphanageTextBlock.Text += orphanage;
            dateAddedTextBlock.Text += dateChildAdded.ToString("dd.MM.yyyy");
            _fullNameChild = fullName;
            statusTextBlock.Text += statusProgram;
            _status = 2;
            switch (statusProgramSw)
            {
                case 1:
                    nanniesBtn.IsEnabled = false;
                    transferBtn.IsEnabled = false;
                    hospitalizationBtn.IsEnabled = false;
                    completeWorkBtn.IsEnabled = false;
                    break;
                case 2:
                    transferBtn.IsEnabled = false;
                    hospitalizationBtn.IsEnabled = false;
                    completeWorkBtn.IsEnabled = false;
                    break;
                case 3:
                    completeWorkBtn.IsEnabled = false;
                    break;
                case 4:
                    completeWorkBtn.IsEnabled = false;
                    break;
                case 5:
                    completeWorkBtn.IsEnabled = false;
                    break;
            }
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

        private void detailedBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);                                                                                                                                                                                                                                                                        
            }
            switch (_status)
            {
                case 1:
                    NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.MedicalExamination.MedicalExaminationDetailInfoPage(this.Tag.ToString()));
                    break;
                case 2:
                    NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime.DetailedInfoPage(this.Tag.ToString()));
                    break;
            }
        }

        private void documentsBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime.DocumentsPage(Tag.ToString(), _fullNameChild));
        }

        private void consultationBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.MedicalExamination.ConsultationPage(Tag.ToString(), _fullNameChild));
        }
    }
}
