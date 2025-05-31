using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows
{
    public partial class AddOrphanage : Window
    {
        private string _idRegion;
        private string _id;
        private bool isInsert = true;

        public AddOrphanage()
        {
            InitializeComponent();
            Title = "Добавление записи";
        }

        public AddOrphanage(string id, string nameOrphanage, string directorSurname, string directorName, string directorMiddleName, string idRegion, string address, string email)
        {
            InitializeComponent();
            tbOrphanageName.Text = nameOrphanage;
            tbDirectorSurname.Text = directorSurname;
            tbDirectorName.Text = directorName;
            tbDirectorMiddleName.Text = directorMiddleName;
            _idRegion = idRegion;
            tbOrphanageAddress.Text = address;
            tbOrphanageEmail.Text = email;
            _id = id;
            isInsert = false;
            Title = "Редактирование записи";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegionsClass.GetRegionsList();
            regionsCmbBox.ItemsSource = RegionsClass.dtRegions.DefaultView;
            regionsCmbBox.DisplayMemberPath = "regionName";
            regionsCmbBox.SelectedValuePath = "ID";
            if (String.IsNullOrEmpty(_idRegion))
                regionsCmbBox.SelectedIndex = 0;
            else
                regionsCmbBox.SelectedValue = _idRegion;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbOrphanageName.Text) || string.IsNullOrWhiteSpace(tbDirectorSurname.Text) ||
                string.IsNullOrWhiteSpace(tbDirectorName.Text) || string.IsNullOrWhiteSpace(tbOrphanageAddress.Text) || 
                string.IsNullOrWhiteSpace(tbOrphanageEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!CustomFunctionsClass.IsValidEmail(tbOrphanageEmail.Text))
            {
                MessageBox.Show("Неккорректно введён email", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isInsert)
            {
                if (!CustomFunctionsClass.CheckSameEmail(tbOrphanageEmail.Text))
                    return;
                if (!OrphanageClass.GetSameDataOrphanage(regionsCmbBox.SelectedValue.ToString(), regionsCmbBox.SelectedValue.ToString()))
                    return;
                if (!OrphanageClass.AddOrphanage(tbOrphanageName.Text, tbDirectorSurname.Text, tbDirectorName.Text, tbDirectorMiddleName.Text, regionsCmbBox.SelectedValue.ToString(), tbOrphanageAddress.Text, tbOrphanageEmail.Text))
                    return;
            }
            else
            {
                if (!CustomFunctionsClass.CheckSameEmail(tbOrphanageEmail.Text, _id, "orphanages"))
                    return;
                if (!OrphanageClass.GetSameDataOrphanage(regionsCmbBox.SelectedValue.ToString(), regionsCmbBox.SelectedValue.ToString(), _id))
                    return;
                if (!OrphanageClass.UpdateOrphanage(_id, tbOrphanageName.Text, tbDirectorSurname.Text, tbDirectorName.Text, tbDirectorMiddleName.Text, regionsCmbBox.SelectedValue.ToString(), tbOrphanageAddress.Text, tbOrphanageEmail.Text))
                    return;
                MessageBox.Show("Запись успешно изменена. При необходимости сформируйте новое соглашение о социальном партнёрстве.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
                DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbOrphanageName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-ЯёЁ№.]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
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

        private void tbEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9A-z@.]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}