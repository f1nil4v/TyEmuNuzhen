using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.MedicalExamination
{
    /// <summary>
    /// Логика взаимодействия для MedicalExaminationDetailInfoPage.xaml
    /// </summary>
    public partial class MedicalExaminationDetailInfoPage : Page
    {
        private string _id;
        private bool _updated = false;
        private string _errImagePath = "../../Images/Childrens/errImage.png";

        public MedicalExaminationDetailInfoPage(string id)
        {
            InitializeComponent();
            HelpManagerClass.CurrentHelpKey = "CuratorMedicalExaminationDetailInfoPage";
            LoadChildData(id);
            _id = id;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_updated == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new ChildrensPage(1));
            NavigationService.RemoveBackEntry();
            HelpManagerClass.CurrentHelpKey = "CuratorMedicalExaminationChildrensPage";
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

        private void LoadChildData(string id)
        {
            ChildrensClass.GetChildrenListByID(id);
            ChildrenDescriptionClass.GetMonitoringDescriptionChildren(id);
            ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
            txtSurname.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["surname"].ToString();
            txtName.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["name"].ToString();
            txtMiddleName.Text = ChildrensClass.dtChildrensDetailedList.Rows[0]["middleName"].ToString();
            txtBirthday.Text = Convert.ToDateTime(ChildrensClass.dtChildrensDetailedList.Rows[0]["birthday"]).ToString("dd.MM.yyyy");
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
    }
}
