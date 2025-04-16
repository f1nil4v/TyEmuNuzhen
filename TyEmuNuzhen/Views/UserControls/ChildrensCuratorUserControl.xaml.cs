using System;
using System.Collections.Generic;
using System.Data;
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

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChildrensCuratorUserControl.xaml
    /// </summary>
    public partial class ChildrensCuratorUserControl : UserControl
    {
        private string _fullNameChild;

        public ChildrensCuratorUserControl(string id, string fullName, DateTime birthDate,
            string age, string regionName, string orphanage, string photoPath, DateTime dateChildAdded, string status)
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
            statusTextBlock.Text += status;
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

        private void detailedBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);                                                                                                                                                                                                                                                                        
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.InWork.DetailInfoPage(Tag.ToString()));
        }

        private void documentsBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.InWork.DocumentsChildPage(Tag.ToString(), _fullNameChild));
        }

        private void consultationBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.InWork.ConsultationPage());
        }
    }
}
