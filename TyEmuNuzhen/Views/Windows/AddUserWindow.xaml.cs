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
        private int _employee;
        private string _idRole;
        private string _regionName;
        private bool isInsert;
        public string phoneNumber;

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
        }

        public AddUserWindow(string login, string surname, string name, string middleName, string phoneNumber, string email, string regionName, string idRole, int employee)
        {
            InitializeComponent();
            tbLogin.Text = login;
            tbLogin.IsEnabled = false;
            tbSurname.Text = surname;
            tbName.Text = name;
            tbMiddleName.Text = middleName;
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length >= 11 && phoneNumber.All(char.IsDigit))
            {
                // Форматируем номер телефона в формате +7 (XXX) XXX-XX-XX
                tbPhone.Text = $"+{phoneNumber[0]} ({phoneNumber.Substring(1, 3)}) {phoneNumber.Substring(4, 3)}-{phoneNumber.Substring(7, 2)}-{phoneNumber.Substring(9, 2)}";
            }
            else
            {
                // Если номер телефона некорректный, устанавливаем пустое значение или используем свойство маски
                tbPhone.Text = phoneNumber; // или просто пустую строку ""
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
            if (isInsert)
            {
                if (String.IsNullOrWhiteSpace(tbLogin.Text) || String.IsNullOrWhiteSpace(tbPassword.Text) || String.IsNullOrWhiteSpace(tbName.Text)
                    || String.IsNullOrWhiteSpace(tbSurname.Text) || String.IsNullOrWhiteSpace(tbMiddleName.Text) || !tbPhone.IsMaskCompleted
                    || String.IsNullOrWhiteSpace(tbEmail.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!CustomFunctionsClass.IsValidPassword(tbPassword.Text))
                    return;
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
            }
            if (!CustomFunctionsClass.IsValidEmail(tbEmail.Text))
            {
                MessageBox.Show("Неккорректно введён email", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            phoneNumber = Regex.Replace(tbPhone.Text, @"[^\d]", "");
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
