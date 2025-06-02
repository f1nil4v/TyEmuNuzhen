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
using System.Xml.Linq;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime
{
    /// <summary>
    /// Логика взаимодействия для DetailedInfoPage.xaml
    /// </summary>
    public partial class DetailedInfoPage : Page
    {
        private string _id;
        private bool _updated = false;
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private bool compl = true;

        public DetailedInfoPage(string id)
        {
            InitializeComponent();
            LoadChildData(id);
            _id = id;
            HelpManagerClass.CurrentHelpKey = "CuratorDetailedInfoPage";
        }

        public DetailedInfoPage(string id, bool a)
        {
            InitializeComponent();
            LoadChildData(id);
            _id = id;
            addDescriptionGrid.Visibility = Visibility.Collapsed;
            HelpManagerClass.CurrentHelpKey = "CuratorDetailedInfoPage";
            compl = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_updated == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new ChildrensPage(2));
            NavigationService.RemoveBackEntry();
            if (compl)
                HelpManagerClass.CurrentHelpKey = "CuratorToBeOnTimePage";
            else
                HelpManagerClass.CurrentHelpKey = "CuratorCompletedWorksPage";
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
            LoadActualDiagnoses(id);
            LoadConsultations(id);
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

        private void LoadActualDiagnoses(string childId)
        {
            actualDiagnosesPanel.Children.Clear();

            ActualDiagnosesClass.GetChildrenAcutualDiagnoses(childId);
            DataView view = ActualDiagnosesClass.dtActualChildrenDiagnoses.DefaultView;
            if (ActualDiagnosesClass.dtActualChildrenDiagnoses.Rows.Count == 0)
            {
                notDiagnoses.Visibility = Visibility.Visible;
                return;
            }
            foreach (DataRowView row in view)
            {
                string diagnosisName = row["diagnosisName"].ToString();
                DescriptionUserControl descriptionUserControl = new DescriptionUserControl(false, "", diagnosisName);
                actualDiagnosesPanel.Children.Add(descriptionUserControl);
            }
        }

        private void LoadConsultations(string childId)
        {
            consultationPanel.Children.Clear();

            ConsultationClass.GetConsultationsChildList(childId);
            DataView view = ConsultationClass.dtConsultationsChildList.DefaultView;
            foreach (DataRowView row in view)
            {
                string idConsultation = row["ID"].ToString();
                string filePathMedicalConclusion = row["filePath"].ToString();
                string dateСonclusion = Convert.ToDateTime(row["dateConsultation"]).ToString("dd.MM.yyyy");
                string middleName = row["middleName"].ToString() == null ? "" : row["middleName"].ToString()[0] + ".";
                string doctorFullNameInitials = row["surname"].ToString() + " " + row["name"].ToString()[0] + ". " + middleName;
                string doctorPost = row["postName"].ToString();
                ConsultationsHistoryUserControl consultationsHistoryUserControl = new ConsultationsHistoryUserControl(idConsultation, doctorFullNameInitials, doctorPost, filePathMedicalConclusion, dateСonclusion);
                consultationPanel.Children.Add(consultationsHistoryUserControl);
            }
        }
    }
}
