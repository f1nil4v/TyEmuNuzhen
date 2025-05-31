using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private string _idEmployee;
        private string _idUser;
        private int _employee;
        private string _idRole;
        private string _regionName;
        private bool isInsert;

        public AddUserWindow(int employee)
        {
            InitializeComponent();
            isInsert = true;
            _employee = employee;
            switch (employee)
            {
                case 2:
                    regionsCmbBox.Visibility = Visibility.Visible;
                    break;
                case 3:
                    curatorRoleCmbBox.Visibility = Visibility.Visible;
                    break;
            }
            Title = "Добавление записи";
        }

        public AddUserWindow(string idEmployee, string login, string surname, string name, string middleName, string phoneNumber, string email, string regionName, string idUser, string idRole, int employee)
        {
            InitializeComponent();
            tbLogin.Text = login;
            tbLogin.IsEnabled = false;
            tbSurname.Text = surname;
            tbName.Text = name;
            tbMiddleName.Text = middleName;
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length >= 11 && phoneNumber.All(char.IsDigit))
            {
                tbPhone.Text = $"+{phoneNumber[0]} ({phoneNumber.Substring(1, 3)}) {phoneNumber.Substring(4, 3)}-{phoneNumber.Substring(7, 2)}-{phoneNumber.Substring(9, 2)}";
            }
            else
            {
                tbPhone.Text = phoneNumber;
            }
            tbEmail.Text = email;
            _regionName = regionName;
            _idRole = idRole;
            _employee = employee;
            switch (employee)
            {
                case 2:
                    regionsCmbBox.Visibility = Visibility.Visible; 
                    break;
                case 3:
                    curatorRoleCmbBox.Visibility = Visibility.Visible; 
                    break;
            }
            isInsert = false;
            Title = "Редактирование записи";
            _idEmployee = idEmployee;
            _idUser = idUser;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_employee)
            {
                case 2:
                    RegionsClass.GetRegionsList();
                    regionsCmbBox.ItemsSource = RegionsClass.dtRegions.DefaultView;
                    regionsCmbBox.DisplayMemberPath = "regionName";
                    regionsCmbBox.SelectedValuePath = "ID";
                    if (isInsert)
                        regionsCmbBox.SelectedIndex = 0;
                    else 
                        regionsCmbBox.Text = _regionName;
                    break;
                case 3:
                    RolesClass.GetCuratorRoles();
                    curatorRoleCmbBox.ItemsSource = RolesClass.dtCuratoRoles.DefaultView;
                    curatorRoleCmbBox.DisplayMemberPath = "roleName";
                    curatorRoleCmbBox.SelectedValuePath = "ID";
                    if (isInsert)
                        curatorRoleCmbBox.SelectedIndex = 0;
                    else
                        curatorRoleCmbBox.SelectedValue = _idRole;
                    break;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber;
            string tableName = null;
            switch (_employee)
            {
                case 1:
                    tableName = "directors";
                    break;
                case 2:
                    tableName = "volunteers";
                    break;
                case 3:
                    tableName = "curators";
                    break;
            }
            if (isInsert)
            {
                if (String.IsNullOrWhiteSpace(tbLogin.Text) || String.IsNullOrWhiteSpace(tbPassword.Text) || String.IsNullOrWhiteSpace(tbName.Text)
                    || String.IsNullOrWhiteSpace(tbSurname.Text) || String.IsNullOrWhiteSpace(tbMiddleName.Text) || !tbPhone.IsMaskCompleted
                    || String.IsNullOrWhiteSpace(tbEmail.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                phoneNumber = Regex.Replace(tbPhone.Text, @"[^\d]", "");
                if (!CustomFunctionsClass.IsValidPassword(tbPassword.Text))
                    return;
                if (!CustomFunctionsClass.CheckSameEmail(tbEmail.Text) || !CustomFunctionsClass.CheckSamePhoneNumber(phoneNumber))
                    return;
                if (_employee == 1)
                {
                    if (!UserClass.AddUser(tbLogin.Text, tbPassword.Text, "3")
                    || !DirectorClass.AddDirector(tbSurname.Text, tbName.Text, tbMiddleName.Text, phoneNumber, tbEmail.Text))
                        return;
                }
                if (_employee == 2)
                {
                    if (!UserClass.AddUser(tbLogin.Text, tbPassword.Text, "1")
                    || !VolonteerClass.AddVolonteer(tbSurname.Text, tbName.Text, tbMiddleName.Text,
                    phoneNumber, tbEmail.Text, regionsCmbBox.SelectedValue.ToString()))
                        return;
                }
                if (_employee == 3)
                {
                    if (!UserClass.AddUser(tbLogin.Text, tbPassword.Text, curatorRoleCmbBox.SelectedValue.ToString())
                    || !CuratorClass.AddCurator(tbSurname.Text, tbName.Text, tbMiddleName.Text, phoneNumber, tbEmail.Text))
                        return;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(tbLogin.Text) || String.IsNullOrWhiteSpace(tbName.Text)
                    || String.IsNullOrWhiteSpace(tbSurname.Text) || String.IsNullOrWhiteSpace(tbMiddleName.Text) || String.IsNullOrWhiteSpace(tbPhone.Text)
                    || String.IsNullOrWhiteSpace(tbEmail.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля (пароль необязательно)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!String.IsNullOrWhiteSpace(tbPassword.Text))
                {
                    if (!CustomFunctionsClass.IsValidPassword(tbPassword.Text))
                        return;
                }
                phoneNumber = Regex.Replace(tbPhone.Text, @"[^\d]", "");
                if (!CustomFunctionsClass.CheckSameEmail(tbEmail.Text, _idEmployee, tableName) || !CustomFunctionsClass.CheckSamePhoneNumber(phoneNumber, _idEmployee, tableName))
                    return;
                if (_employee == 1)
                {
                    if (!UserClass.UpdateUser(_idUser, tbPassword.Text)
                || !DirectorClass.UpdateDirector(_idEmployee, tbSurname.Text, tbName.Text, tbMiddleName.Text, phoneNumber, tbEmail.Text))
                        return;
                }
                if (_employee == 2)
                {
                    if (!UserClass.UpdateUser(_idUser, tbPassword.Text)
                    || !VolonteerClass.UpdateVolonteer(_idEmployee, tbSurname.Text, tbName.Text,
                    tbMiddleName.Text, phoneNumber, tbEmail.Text, regionsCmbBox.SelectedValue.ToString()))
                        return;
                }
                if (_employee == 3)
                {
                    if (!UserClass.UpdateUser(_idUser, tbPassword.Text, curatorRoleCmbBox.SelectedValue.ToString())
                    || !CuratorClass.UpdateCurator(_idEmployee, tbSurname.Text, tbName.Text, tbMiddleName.Text, phoneNumber, tbEmail.Text))
                        return;
                }
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbOrphanageName_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-ЯёЁ]");
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
