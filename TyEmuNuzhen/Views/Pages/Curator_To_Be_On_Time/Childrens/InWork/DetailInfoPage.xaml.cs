using Microsoft.Win32;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.InWork
{
    /// <summary>
    /// Логика взаимодействия для DetailInfoPage.xaml
    /// </summary>
    public partial class DetailInfoPage : Page
    {
        private string _id;
        private bool _updated = false;
        private string _errImagePath = "../../Images/Childrens/errImage.png";

        public DetailInfoPage(string id)
        {
            InitializeComponent();
            LoadComboBoxes();
            LoadChildData(id);
            _id = id;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_updated == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new ChildrensPage());
            NavigationService.RemoveBackEntry();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            edtSurname.Text = txtSurname.Text;
            edtName.Text = txtName.Text;
            edtMiddleName.Text = txtMiddleName.Text;
            edtRegion.Text = txtRegion.Text;
            edtOrphanage.Text = txtOrphanage.Text;

            if (DateTime.TryParse(txtBirthday.Text, out DateTime birthDate))
            {
                edtBirthday.SelectedDate = birthDate;
            }
            viewGrid.Visibility = Visibility.Collapsed;
            editGrid.Visibility = Visibility.Visible;
            photoBorder.IsEnabled = false;
            photoHistoryGrid.IsEnabled = false;
            descriptionHistoryGrid.IsEnabled = false;
            addDescriptionGrid.IsEnabled = false;
            diagnosesGrid.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            viewGrid.Visibility = Visibility.Visible;
            editGrid.Visibility = Visibility.Collapsed;
            photoBorder.IsEnabled = true;
            photoHistoryGrid.IsEnabled = true;
            descriptionHistoryGrid.IsEnabled = true;
            addDescriptionGrid.IsEnabled = true;
            diagnosesGrid.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(edtSurname.Text) ||
                string.IsNullOrWhiteSpace(edtName.Text) ||
                edtBirthday.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля: Фамилия, Имя и Дата рождения",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string birthDay = edtBirthday.SelectedDate.Value.ToString("yyyy-MM-dd");

            if (!ChildrensClass.UpdateCuratorInfoChildren(_id, edtSurname.Text, edtName.Text, edtMiddleName.Text, birthDay, edtRegion.SelectedValue.ToString(), edtOrphanage.SelectedValue.ToString()))
                return;

            LoadChildData(_id);
            viewGrid.Visibility = Visibility.Visible;
            editGrid.Visibility = Visibility.Collapsed;
            photoBorder.IsEnabled = true;
            photoHistoryGrid.IsEnabled = true;
            descriptionHistoryGrid.IsEnabled = true;
            addDescriptionGrid.IsEnabled = true;
            diagnosesGrid.IsEnabled = true;
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

        private void edtRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            edtOrphanage.SelectedIndex = -1;
            string _idRegion = edtRegion.SelectedValue == null ? null : edtRegion.SelectedValue.ToString();
            OrphanageClass.GetOrphanagesForComboBoxList(_idRegion);
            edtOrphanage.ItemsSource = OrphanageClass.dtOrphanagesForComboBoxList.DefaultView;
            edtOrphanage.DisplayMemberPath = "nameOrphanage";
            edtOrphanage.SelectedValuePath = "ID";
            edtOrphanage.SelectedIndex = 0;
        }

        private void LoadChildData(string id)
        {
            ChildrensClass.GetChildrenListByID(id);
            ChildrenDescriptionClass.GetMonitoringDescriptionChildren(id);
            ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
            txtSurname.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
            txtName.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
            txtMiddleName.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["middleName"].ToString();
            txtBirthday.Text = Convert.ToDateTime(ChildrensClass.dtChildrensList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
            txtAge.Text = CustomFunctionsClass.CalculateAge(Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"])).ToString();
            txtDateAdded.Text = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["dateAdded"]).ToString("dd.MM.yyyy");
            txtRegion.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["regionName"].ToString();
            txtOrphanage.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["orphanageName"].ToString();
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
            LoadDiagnoses(id);
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

        private void LoadDiagnoses(string childId)
        {
            diagnosisPanel.Children.Clear();

            ChildrenDiagnosisClass.GetChildrenDiagnoses(childId);
            if (ChildrenPhotoClass.dtMonitoringPhoto.Rows.Count > 0)
            {
                DataView view = ChildrenDiagnosisClass.dtChildrenDiagnoses.DefaultView;
                foreach (DataRowView row in view)
                {
                    string diagnosisName = row["diagnosisName"].ToString();
                    string dateAdded = Convert.ToDateTime(row["updateDate"]).ToString("dd.MM.yyyy");
                    DescriptionUserControl descriptionUserControl = new DescriptionUserControl(false, dateAdded, diagnosisName);
                    diagnosisPanel.Children.Add(descriptionUserControl);
                }
            }
            else
            {
                diagnosisPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadComboBoxes()
        {
            edtRegion.SelectedIndex = -1;
            edtOrphanage.SelectedIndex = -1;
            RegionsClass.GetRegionsListForEditInfoChildren();
            edtRegion.ItemsSource = RegionsClass.dtRegionsForEditInfoChildren.DefaultView;
            edtRegion.DisplayMemberPath = "regionName";
            edtRegion.SelectedValuePath = "ID";
            string _idRegion = edtRegion.SelectedValue == null ? null : edtRegion.SelectedValue.ToString();
            OrphanageClass.GetOrphanagesForComboBoxList(_idRegion);
            edtOrphanage.ItemsSource = OrphanageClass.dtOrphanagesForComboBoxList.DefaultView;
            edtOrphanage.DisplayMemberPath = "nameOrphanage";
            edtOrphanage.SelectedValuePath = "ID";
        }
    }
}
