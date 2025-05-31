using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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

namespace TyEmuNuzhen.Views.Pages.Curator.ChildrensWork
{
    /// <summary>
    /// Логика взаимодействия для DetailChildrenInfoCuratorPage.xaml
    /// </summary>
    public partial class DetailChildrenInfoCuratorPage : Page
    {
        private string _id;
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private bool _updated = false;

        public DetailChildrenInfoCuratorPage(string id)
        {
            InitializeComponent();
            LoadChildData(id);
            _id = id;
            edtBirthday.DisplayDateEnd = DateTime.Today.AddMonths(-1);
            edtBirthday.SelectedDate = DateTime.Today.AddMonths(-1);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_updated == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new MonitoringPage());
            NavigationService.RemoveBackEntry();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            edtQuestNumber.Text = txtQuestNumber.Text;
            edtSurname.Text = txtSurname.Text;
            edtName.Text = txtName.Text;
            edtUrl.Text = btnOpenUrl.Tag?.ToString() ?? "";

            if (DateTime.TryParse(txtBirthday.Text, out DateTime birthDate))
            {
                edtBirthday.SelectedDate = birthDate;
            }

            edtIsAlert.IsChecked = txtIsAlert.Text == "Да";
            viewGrid.Visibility = Visibility.Collapsed;
            editGrid.Visibility = Visibility.Visible;
            photoBorder.IsEnabled = false;
            photoHistoryGrid.IsEnabled = false;
            descriptionHistoryGrid.IsEnabled = false;
            addDescriptionGrid.IsEnabled = false;

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            viewGrid.Visibility = Visibility.Visible;
            editGrid.Visibility = Visibility.Collapsed;
            photoBorder.IsEnabled = true;
            photoHistoryGrid.IsEnabled = true;
            descriptionHistoryGrid.IsEnabled = true;
            addDescriptionGrid.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string isAlert = "0";

            if (string.IsNullOrWhiteSpace(edtSurname.Text) ||
                string.IsNullOrWhiteSpace(edtName.Text) ||
                edtBirthday.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: Фамилия, Имя и Дата рождения",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string birthDay = edtBirthday.SelectedDate.Value.ToString("yyyy-MM-dd");

            if (edtIsAlert.IsChecked == true)
            {
                isAlert = "1";
            }

            if (!ChildrensClass.GetSameNumOfQuestionnaire(edtQuestNumber.Text, _id))
                return;
            if (!ChildrensClass.GetSameNumOfQuestionnaire(edtUrl.Text, _id))
                return;

            if (!ChildrensClass.UpdateMonitoringInfoChildren(_id, edtQuestNumber.Text, edtUrl.Text, edtSurname.Text, edtName.Text, birthDay, isAlert))
                return;

            LoadChildData(_id);
            viewGrid.Visibility = Visibility.Visible;
            editGrid.Visibility = Visibility.Collapsed;
            photoBorder.IsEnabled = true;
            photoHistoryGrid.IsEnabled = true;
            descriptionHistoryGrid.IsEnabled = true;
            addDescriptionGrid.IsEnabled = true;
            _updated = true;
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            changePhotoBtn.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            changePhotoBtn.Visibility = Visibility.Hidden;
        }

        private void LoadChildData(string id)
        {
            ChildrensClass.GetChildrenListByID(id);
            ChildrenDescriptionClass.GetMonitoringDescriptionChildren(id);
            ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
            txtQuestNumber.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["numOfQuestionnaire"].ToString();
            if (ChildrensClass.dtChildrensDetailedList.Rows[0]["isAlert"].ToString() == "1")
            {
                alertIcon.Visibility = Visibility.Visible;
                txtIsAlert.Foreground = new SolidColorBrush(Colors.Red);
                txtIsAlert.Text = "Да";
            }
            else
            {
                alertIcon.Visibility = Visibility.Collapsed;
                txtIsAlert.Foreground = new SolidColorBrush(Colors.Black);
                txtIsAlert.Text = "Нет";
            }
            txtSurname.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
            txtBirthday.Text = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
            txtName.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
            txtAge.Text = CustomFunctionsClass.CalculateAge(Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"])).ToString();
            txtDateAdded.Text = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["dateAdded"]).ToString("dd.MM.yyyy");
            btnOpenUrl.Tag = ChildrensClass.dtChildrensDetailedList.Rows[0]["urlOfQuestionnaire"].ToString();
            txtRegion.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["regionName"].ToString();
            txtRegion1.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["regionName"].ToString();
            string photoPath = ChildrensClass.dtChildrensDetailedList.Rows[0]["latestPhotoPath"].ToString();

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

        private void changePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string photoPath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(photoPath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                childPhoto.ImageSource = bitmap;
                string image = CopyFilesClass.CopyChildImage(photoPath, _id);
                if (!ChildrenPhotoClass.AddMonitoringPhotoChildren(_id, image))
                    return;
                LoadChildData(_id);
                _updated = true;
            }
        }

        private void btnAddDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtNewDescription.Text))
            {
                if (!ChildrenDescriptionClass.AddMonitoringDescriptionChildren(_id, txtNewDescription.Text))
                    return;
                txtNewDescription.Text = null;
                LoadChildData(_id);
                _updated = true;
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
                if (!ChildrensClass.UpdateStatusChildren(_id, "11"))
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
    }
}
