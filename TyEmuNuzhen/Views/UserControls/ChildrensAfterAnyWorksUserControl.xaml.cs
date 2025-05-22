using System;
using System.Collections.Generic;
using System.Data;
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
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows.DialogWindows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ChildrensAfterAnyWorksUserControl.xaml
    /// </summary>
    public partial class ChildrensAfterAnyWorksUserControl : UserControl
    {
        private string _questUrl;
        private string _fullNameChild;

        public ChildrensAfterAnyWorksUserControl(string id, string questNumber, string questURL, string fullName, DateTime birthDate,
            string age, string regionName, string orphanageName, string photoPath, DateTime dateChildAdded, string idStatus, string statusName, string lastProgram)
        {
            InitializeComponent();
            this.Tag = id;
            questNumberTextBlock.Text += questNumber;
            fullNameTextBlock.Text = fullName;
            birthdayTextBlock.Text += birthDate.ToString("dd.MM.yyyy");
            ageTextBlock.Text += age;
            regionTextBlock.Text += regionName;
            dateAddedTextBlock.Text += dateChildAdded.ToString("dd.MM.yyyy");
            _questUrl = questURL;
            _fullNameChild = fullName;
            if (!String.IsNullOrEmpty(lastProgram))
                lastProgramTextBlock.Text += lastProgram;
            else
            {
                lastProgramTextBlock.Text += "-";
                historyProgramsBtn.IsEnabled = false;
                documentsBtn.IsEnabled = false;
            }
            if (idStatus != "11")
            {
                beginMonitoringBtn.IsEnabled = false;
                statusWarning.Visibility = Visibility.Visible;
                statusWarning.Text += statusName;
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
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime.DetailedInfoPage(this.Tag.ToString(), false));
        }

        private void documentsBtn_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.CompletedWorks.DocumentsPage(this.Tag.ToString(), _fullNameChild));
        }

        private void historyProgramsBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectProgramAndPeriodWindow selectProgramAndPeriodWindow = new SelectProgramAndPeriodWindow(this.Tag.ToString());
            if (selectProgramAndPeriodWindow.ShowDialog() == true)
            {
                ProgramHistoryWindow programHistoryWindow = new ProgramHistoryWindow(selectProgramAndPeriodWindow.idProgram, selectProgramAndPeriodWindow.filePathConsent);
                programHistoryWindow.ShowDialog();
            }
        }

        private void beginMonitoringBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!ChildrensClass.UpdateStatusChildren(this.Tag.ToString(), "1"))
                return;
            DependencyObject parent = this;
            while (parent != null && !(parent is Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            NavigationService.GetNavigationService(parent).Navigate(new Pages.Curator_To_Be_On_Time.Childrens.ChildrensPage(3));
        }
    }
}
