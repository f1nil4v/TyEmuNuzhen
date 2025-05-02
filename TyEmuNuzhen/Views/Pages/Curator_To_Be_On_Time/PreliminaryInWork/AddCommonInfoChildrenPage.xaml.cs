using System;
using System.Collections.Generic;
using System.Data;
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
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.PreliminaryInWork
{
    /// <summary>
    /// Логика взаимодействия для AddCommonInfoChildrenPage.xaml
    /// </summary>
    public partial class AddCommonInfoChildrenPage : Page
    {
        private string _id;
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private bool _updated = false;

        public AddCommonInfoChildrenPage(string id)
        {
            InitializeComponent();
            LoadChildData(id);
            _id = id;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_updated == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new MonitoringPage());
        }

        private void LoadChildData(string id)
        {
            ChildrensClass.GetChildrenListByID(id);
            ChildrenDescriptionClass.GetMonitoringDescriptionChildren(id);
            ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
            txtQuestNumber.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["numOfQuestionnaire"].ToString();
            txtSurname.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
            txtBirthday.Text = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
            txtName.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
            txtAge.Text = CustomFunctionsClass.CalculateAge(Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"])).ToString();
            txtDateAdded.Text = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["dateAdded"]).ToString("dd.MM.yyyy");
            btnOpenUrl.Tag = ChildrensClass.dtChildrensDetailedList.Rows[0]["urlOfQuestionnaire"].ToString();
            txtRegion.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["regionName"].ToString();
            string photoPath = ChildrensClass.dtChildrensDetailedList.Rows[0]["latestPhotoPath"].ToString();
            OrphanageClass.GetOrphanagesForComboBoxList(ChildrensClass.dtChildrensDetailedList.Rows[0]["idRegion"].ToString());
            orphanageCmbBox.ItemsSource = OrphanageClass.dtOrphanagesForComboBoxList.DefaultView;
            orphanageCmbBox.DisplayMemberPath = "nameOrphanage";
            orphanageCmbBox.SelectedValuePath = "ID";
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
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_errImagePath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                childPhoto.ImageSource = bitmap;
            }
            LoadDescriptions(id);
            loadChildPhoto(id);
        }

        private void LoadDescriptions(string childId)
        {
            notesPanel.Children.Clear();
            ChildrenDescriptionClass.GetMonitoringDescriptionChildren(childId);
            if (ChildrenDescriptionClass.dtMonitoringDescription.Rows.Count > 0)
            {
                bool isFirst = true;
                foreach (DataRow row in ChildrenDescriptionClass.dtMonitoringDescription.Rows)
                {
                    string description = row["description"].ToString();
                    string dateAdded = Convert.ToDateTime(row["dateAdded"]).ToString("dd.MM.yyyy");
                    DescriptionUserControl descriptionUserControl = new DescriptionUserControl(isFirst, dateAdded, description);
                    notesPanel.Children.Add(descriptionUserControl);
                    isFirst = false;
                }

                descriptionHistoryGrid.Visibility = Visibility.Visible;
            }
            else
            {
                descriptionHistoryGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void loadChildPhoto(string id)
        {
            photosPanel.Children.Clear();

            ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
            if (ChildrenPhotoClass.dtMonitoringPhoto.Rows.Count > 0)
            {
                photosPanel.Children.Clear();

                ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
                if (ChildrenPhotoClass.dtMonitoringPhoto.Rows.Count > 0)
                {
                    DataView view = ChildrenPhotoClass.dtMonitoringPhoto.DefaultView;

                    bool hasValidPhotos = false;
                    bool isFirst = true;
                    foreach (DataRowView row in view)
                    {
                        string photoPath = row["filePath"].ToString();
                        string dateAdded = Convert.ToDateTime(row["dateAdded"]).ToString("dd.MM.yyyy");
                        if (!string.IsNullOrEmpty(photoPath))
                        {
                            ImageUserControl photoControl = new ImageUserControl(0, isFirst, photoPath, dateAdded, "");
                            photosPanel.Children.Add(photoControl);
                            hasValidPhotos = true;
                            isFirst = false;
                        }
                    }

                    photoHistoryGrid.Visibility = hasValidPhotos ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    photoHistoryGrid.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnOpenUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = btnOpenUrl.Tag.ToString(),
                    UseShellExecute = true
                });
            }
            catch
            {
                MessageBox.Show("Ошибка открытия ссылки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddNotProblem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что у данного ребёнка проблем не выявлено?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (!ChildrensClass.UpdateStatusChildren(_id, "9"))
                    return;
                MessageBox.Show("Наблюдение за данным ребёнком завершено. Информация о нём перемещена в Архив/Без выявленных проблем", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new MonitoringPage());
            }
        }

        private void btnAddHaveProblems_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что у данного ребёнка выявленны проблемы?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                if (!ChildrensClass.UpdateStatusChildren(_id, "2"))
                    return;
                MessageBox.Show("Данный ребёнок предварительно запущен в процесс работы. Для того что-бы провести консультацию, заполните остальную информацию на странице Предварительно в работе", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new MonitoringPage());
            }
        }

        private void btnCompleeteAddInformation_Click(object sender, RoutedEventArgs e)
        {
            if (orphanageCmbBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите детский дом!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ChildrensClass.AddCommonInfoChildren(_id, txtBoxMiddleName.Text, orphanageCmbBox.SelectedValue.ToString()))
                return;

            MessageBox.Show("Информация успешно добавлена. Для дальнейшей работы с данным ребёнком перейдите на страницу Дети.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new PreliminaryInWorkPage());
        }
    }
}
