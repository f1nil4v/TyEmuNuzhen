using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Pages.Volonteer
{
    public partial class UpdateInsertChildrenPage : Page
    {
        private string _photoPath = null;

        public UpdateInsertChildrenPage()
        {
            InitializeComponent();
            birthdayDatePicker.DisplayDateEnd = DateTime.Today.AddMonths(-1);
            birthdayDatePicker.SelectedDate = DateTime.Today.AddMonths(-1);
            HelpManagerClass.CurrentHelpKey = "CuratorAddChildPage";
        }

        private void selectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                _photoPath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_photoPath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                photoPreviewBrush.ImageSource = bitmap;
                photoPreviewBorder.Visibility = Visibility.Visible;
                errorImage.Text = null;
                errorFields.Text = null;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            errorImage.Text = null;
            string image = CopyFilesClass.CopyChildImage(_photoPath, "photo");
            string isAlert = "0";
            if (string.IsNullOrWhiteSpace(surnameTextBox.Text) ||
                string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                string.IsNullOrWhiteSpace(numOfQuestionnaireTextBox.Text) ||
                string.IsNullOrWhiteSpace(urlOfQuestionnaireTextBox.Text) ||
                string.IsNullOrWhiteSpace(descriptionTextBox.Text) ||
                birthdayDatePicker.SelectedDate == null)
            {
                if (String.IsNullOrEmpty(_photoPath))
                {
                    errorImage.Text = "*Выберите изображение";
                    AnimationsClass.ShakeElement(errorImage);
                }
                errorFields.Text = "*Заполните все поля!";
                AnimationsClass.ShakeElement(errorFields);
                return;
            }

            if (String.IsNullOrEmpty(_photoPath))
            {
                errorImage.Text = "*Выберите изображение";
                AnimationsClass.ShakeElement(errorImage);
                return;
            }

            if (String.IsNullOrEmpty(image))
            {
                errorImage.Text = "*Выберите другое изображение";
                AnimationsClass.ShakeElement(errorImage);
                return;
            }

            if (isAlertToggleButton.IsChecked == true)
            {
                isAlert = "1";
            }

            if (!ChildrensClass.GetSameNumOfQuestionnaire(numOfQuestionnaireTextBox.Text))
                return;
            if (!ChildrensClass.GetSameNumOfQuestionnaire(urlOfQuestionnaireTextBox.Text))
                return;

            string birthDay = birthdayDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            if (!ChildrensClass.AddMonitoringInfoChildren(numOfQuestionnaireTextBox.Text, urlOfQuestionnaireTextBox.Text, surnameTextBox.Text, nameTextBox.Text, birthDay, VolonteerClass.idRegion, isAlert))
                return;
            string idChild = ChildrensClass.GetLastChildrensID();
            if (!ChildrenPhotoClass.AddMonitoringPhotoChildren(idChild, image))
                return;
            if (!ChildrenDescriptionClass.AddMonitoringDescriptionChildren(idChild, descriptionTextBox.Text))
                return;
            NavigationService.Navigate(new MonitoringPage());
            NavigationService.RemoveBackEntry();
            HelpManagerClass.CurrentHelpKey = "VolonteerMonitoringPage";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(surnameTextBox.Text) ||
                !string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                !string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите отменить добавление? Все несохраненные данные будут утеряны.",
                    "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            NavigationService.GoBack();
            HelpManagerClass.CurrentHelpKey = "VolonteerMonitoringPage";
        }

        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(surnameTextBox.Text) ||
                !string.IsNullOrWhiteSpace(nameTextBox.Text) ||
                !string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы уверены, что хотите отменить добавление? Все несохраненные данные будут утеряны.",
                    "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            NavigationService.GoBack();
            HelpManagerClass.CurrentHelpKey = "VolonteerMonitoringPage";
        }

        private void surnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorFields.Text = null;
        }

        private void tbSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void tbSurname_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }
    }
}
