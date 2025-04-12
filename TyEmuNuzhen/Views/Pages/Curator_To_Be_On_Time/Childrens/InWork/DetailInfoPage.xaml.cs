using Microsoft.Win32;
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

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.InWork
{
    /// <summary>
    /// Логика взаимодействия для DetailInfoPage.xaml
    /// </summary>
    public partial class DetailInfoPage : Page
    {
        private string _id;
        private string _errImagePath = "../../Images/Childrens/errImage.png";
        private bool _updated = false;

        public DetailInfoPage(string id)
        {
            InitializeComponent();
            LoadChildData(id);
            _id = id;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
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
            agreementGrid.IsEnabled = false;
            consentsGrid.IsEnabled = false;
            documentsGrid.IsEnabled = false;
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
            agreementGrid.IsEnabled = true;
            consentsGrid.IsEnabled = true;
            documentsGrid.IsEnabled = true;
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
            agreementGrid.IsEnabled = true;
            consentsGrid.IsEnabled = true;
            documentsGrid.IsEnabled = true;
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
        }

        private void LoadDescriptions(string childId)
        {

            notesPanel.Children.Clear();

            ChildrenDescriptionClass.GetMonitoringDescriptionChildren(childId);
            if (ChildrenDescriptionClass.dtMonitoringDescription.Rows.Count > 0)
            {
                DataView view = ChildrenDescriptionClass.dtMonitoringDescription.DefaultView;

                int index = 0;
                foreach (DataRowView row in view)
                {
                    Grid noteGrid = new Grid();
                    noteGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    noteGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    TextBlock dateText = new TextBlock
                    {
                        FontSize = 12,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A101A6")),
                        Margin = new Thickness(0, 0, 0, 5)
                    };
                    string dateString = Convert.ToDateTime(row["dateAdded"]).ToString("dd.MM.yyyy");
                    if (index == 0)
                    {
                        dateString += " (последнее)";
                    }
                    dateText.Text = dateString;
                    TextBlock descriptionText = new TextBlock
                    {
                        Text = row["description"].ToString(),
                        TextWrapping = TextWrapping.Wrap
                    };
                    Grid.SetRow(dateText, 0);
                    Grid.SetRow(descriptionText, 1);
                    noteGrid.Children.Add(dateText);
                    noteGrid.Children.Add(descriptionText);
                    Border noteBorder = new Border
                    {
                        Margin = new Thickness(0, 5, 0, 5),
                        BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCF5FD3")),
                        BorderThickness = new Thickness(0, 0, 0, 1),
                        Padding = new Thickness(0, 0, 0, 5),
                        Child = noteGrid
                    };
                    notesPanel.Children.Add(noteBorder);
                    index++;
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
                int index = 0;
                photosPanel.Children.Clear();

                ChildrenPhotoClass.GetMonitoringPhotoChildren(id);
                if (ChildrenPhotoClass.dtMonitoringPhoto.Rows.Count > 0)
                {
                    DataView view = ChildrenPhotoClass.dtMonitoringPhoto.DefaultView;

                    bool hasValidPhotos = false;

                    foreach (DataRowView row in view)
                    {
                        string photoPath = row["filePath"].ToString();

                        if (!string.IsNullOrEmpty(photoPath))
                        {
                            StackPanel photoPanel = new StackPanel();
                            photoPanel.Width = 150;
                            Border imageBorder = new Border
                            {
                                Width = 140,
                                Height = 140,
                                CornerRadius = new CornerRadius(8),
                                Margin = new Thickness(0, 0, 0, 5)
                            };
                            ImageBrush photoBrush = new ImageBrush
                            {
                                Stretch = Stretch.UniformToFill
                            };
                            try
                            {
                                BitmapImage bitmap = new BitmapImage();
                                bitmap.BeginInit();
                                bitmap.UriSource = new Uri(photoPath, UriKind.RelativeOrAbsolute);
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();

                                photoBrush.ImageSource = bitmap;
                            }
                            catch
                            {
                                BitmapImage errorBitmap = new BitmapImage();
                                errorBitmap.BeginInit();
                                errorBitmap.UriSource = new Uri(_errImagePath, UriKind.RelativeOrAbsolute);
                                errorBitmap.CacheOption = BitmapCacheOption.OnLoad;
                                errorBitmap.EndInit();
                                photoBrush.ImageSource = errorBitmap;
                            }

                            imageBorder.Background = photoBrush;
                            TextBlock dateText;
                            if (index == 0)
                            {
                                dateText = new TextBlock
                                {
                                    Text = Convert.ToDateTime(row["dateAdded"]).ToString("dd.MM.yyyy") + " (последнее)",
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A101A6")),
                                    FontSize = 12
                                };
                                index++;
                            }
                            else
                            {
                                dateText = new TextBlock
                                {
                                    Text = Convert.ToDateTime(row["dateAdded"]).ToString("dd.MM.yyyy"),
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A101A6")),
                                    FontSize = 12
                                };
                            }
                            photoPanel.Children.Add(imageBorder);
                            photoPanel.Children.Add(dateText);
                            Border photoBorder = new Border
                            {
                                Margin = new Thickness(5),
                                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCF5FD3")),
                                BorderThickness = new Thickness(1),
                                CornerRadius = new CornerRadius(10),
                                Padding = new Thickness(5),
                                Child = photoPanel
                            };
                            photosPanel.Children.Add(photoBorder);
                            hasValidPhotos = true;
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
                string image = CopyFilesClass.CopyChildImage(photoPath);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void edtRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string _idRegion = edtRegion.SelectedValue == null ? null : edtRegion.SelectedValue.ToString();
            OrphanageClass.GetOrphanagesForComboBoxList(_idRegion);
            edtRegion.ItemsSource = RegionsClass.dtRegionsForEditInfoChildren.DefaultView;
            edtRegion.DisplayMemberPath = "regionName";
            edtRegion.SelectedValuePath = "ID";
            edtOrphanage.SelectedIndex = 0;
        }
    }
}
