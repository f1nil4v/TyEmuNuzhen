using Mysqlx.Crud;
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
    /// Логика взаимодействия для AddNannyWindow.xaml
    /// </summary>
    public partial class AddNannyWindow : Window
    {
        private string _id;
        private bool isInsert = true;

        public AddNannyWindow()
        {
            InitializeComponent();
            Title = "Добавление записи";
            dpPassDateOfIssue.SelectedDate = DateTime.Now;
        }

        public AddNannyWindow(string id, string surname, string name, string middleName, string passSeries, string passNum, string passDateOfIssue, string passOrganizationOfIssue, string passCode, string addressRegister, string phoneNumber, string email)
        {
            InitializeComponent();
            tbSurname.Text = surname;
            tbName.Text = name;
            tbMiddleName.Text = middleName;
            tbPassSeries.Text = passSeries;
            tbPassNum.Text = passNum;
            dpPassDateOfIssue.SelectedDate = Convert.ToDateTime(passDateOfIssue);
            tbPassOrganizationOfIssue.Text = passOrganizationOfIssue;
            tbPassCode.Text = passCode;
            tbAddressRegister.Text = addressRegister;
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length >= 11 && phoneNumber.All(char.IsDigit))
            {
                tbPhone.Text = $"+{phoneNumber[0]} ({phoneNumber.Substring(1, 3)}) {phoneNumber.Substring(4, 3)}-{phoneNumber.Substring(7, 2)}-{phoneNumber.Substring(9, 2)}";
            }
            else
            {
                tbPhone.Text = phoneNumber;
            }
            tbEmail.Text = email;
            _id = id;
            Title = "Редактирование записи";
            isInsert = false;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSurname.Text) || string.IsNullOrWhiteSpace(tbName.Text) ||
                string.IsNullOrWhiteSpace(tbPassSeries.Text) || string.IsNullOrWhiteSpace(tbPassNum.Text) || 
                string.IsNullOrWhiteSpace(tbPassOrganizationOfIssue.Text) || !tbPassCode.IsMaskCompleted || 
                string.IsNullOrWhiteSpace(tbAddressRegister.Text) || dpPassDateOfIssue.SelectedDate == null ||
                !tbPhone.IsMaskCompleted || string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Внимение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (tbPassSeries.Text.Length < 4)
            {
                MessageBox.Show("Серия паспорта должна состоять из 4 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (tbPassNum.Text.Length < 6)
            {
                MessageBox.Show("Номер паспорта должен состоять из 6 цифр.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!CustomFunctionsClass.IsValidEmail(tbEmail.Text))
                return;

            if (isInsert)
            {
                if (!NanniesClass.GetSamePassData(tbPassSeries.Text, tbPassNum.Text))
                    return;
                string phoneNumber = Regex.Replace(tbPhone.Text, @"[^\d]", "");
                if (!CustomFunctionsClass.CheckSameEmail(tbEmail.Text) || !CustomFunctionsClass.CheckSamePhoneNumber(phoneNumber))
                    return;
                if (!NanniesClass.AddNanny(tbSurname.Text, tbName.Text, tbMiddleName.Text, tbPassSeries.Text, tbPassNum.Text, 
                    dpPassDateOfIssue.SelectedDate.Value.ToString("yyyy-MM-dd"), tbPassOrganizationOfIssue.Text, tbPassCode.Text, tbAddressRegister.Text, phoneNumber, tbEmail.Text))
                    return;
            }
            else
            {
                if (!NanniesClass.GetSamePassData(tbPassSeries.Text, tbPassNum.Text, _id))
                    return;
                string phoneNumber = Regex.Replace(tbPhone.Text, @"[^\d]", "");
                if (!CustomFunctionsClass.CheckSameEmail(tbEmail.Text, _id, "nannies") || !CustomFunctionsClass.CheckSamePhoneNumber(phoneNumber, _id, "nannies"))
                    return;
                if (!NanniesClass.UpdateNanny(_id, tbSurname.Text, tbName.Text, tbMiddleName.Text, tbPassSeries.Text, tbPassNum.Text,
                    dpPassDateOfIssue.SelectedDate.Value.ToString("yyyy-MM-dd"), tbPassOrganizationOfIssue.Text, tbPassCode.Text, tbAddressRegister.Text, phoneNumber, tbEmail.Text))
                    return;
                MessageBox.Show("Запись успешно изменена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
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

        private void tbPassSeries_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void tbEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9A-z@.]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        private void tbAddressRegister_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9а-яА-ЯёЁю.,/]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
