﻿using Mysqlx.Crud;
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
    /// Логика взаимодействия для AddDoctorWindow.xaml
    /// </summary>
    public partial class AddDoctorWindow : Window
    {
        private string _id;
        private string _idPost;
        private string _idMedicalFacility;
        private bool _isInsert = true;

        public AddDoctorWindow()
        {
            InitializeComponent();
            Title = "Добавление записи";
        }

        public AddDoctorWindow(string id, string surname, string name, string middleName, string phoneNumber, string email, string idPost, string idMedicalFacility)
        {
            InitializeComponent();
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
            _idPost = idPost;
            _idMedicalFacility = idMedicalFacility;
            Title = "Редактирование записи";
            _isInsert = false;
            _id = id;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DoctorPostsClass.GetDoctorPostsList();
            postsCmbBox.ItemsSource = DoctorPostsClass.dtDoctorPostsList.DefaultView;
            postsCmbBox.DisplayMemberPath = "postName";
            postsCmbBox.SelectedValuePath = "ID";
            if (String.IsNullOrEmpty(_idPost))
                postsCmbBox.SelectedIndex = 0;
            else
                postsCmbBox.SelectedValue = _idPost;
            MedicalFacilityClass.GetMedicalFacilityForComboBoxList();
            medicalFacilityCmbBox.ItemsSource = MedicalFacilityClass.dtMedicalFacilityForComboBoxList.DefaultView;
            medicalFacilityCmbBox.DisplayMemberPath = "medicalFacilityName";
            medicalFacilityCmbBox.SelectedValuePath = "ID";
            if (String.IsNullOrEmpty(_idMedicalFacility))
                medicalFacilityCmbBox.SelectedIndex = 0;
            else
                medicalFacilityCmbBox.SelectedValue = _idMedicalFacility;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            string phoneNumber;
            if (String.IsNullOrWhiteSpace(tbName.Text)
                || String.IsNullOrWhiteSpace(tbSurname.Text) || String.IsNullOrWhiteSpace(tbMiddleName.Text) || !tbPhone.IsMaskCompleted
                || String.IsNullOrWhiteSpace(tbEmail.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!CustomFunctionsClass.IsValidEmail(tbEmail.Text))
            {
                MessageBox.Show("Неккорректно введён email", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            phoneNumber = Regex.Replace(tbPhone.Text, @"[^\d]", "");
            if (_isInsert)
            {
                if (!CustomFunctionsClass.CheckSameEmail(tbEmail.Text) || !CustomFunctionsClass.CheckSamePhoneNumber(phoneNumber))
                    return;
                if (!DoctorsOnAgreementClass.AddDoctor(tbSurname.Text, tbName.Text, tbMiddleName.Text, phoneNumber, tbEmail.Text, postsCmbBox.SelectedValue.ToString(), medicalFacilityCmbBox.SelectedValue.ToString()))
                    return;
            }
            else
            {
                if (!CustomFunctionsClass.CheckSameEmail(tbEmail.Text, _id, "doctors_on_agreement") || !CustomFunctionsClass.CheckSamePhoneNumber(phoneNumber, _id, "doctors_on_agreement"))
                    return;
                if (!DoctorsOnAgreementClass.UpdateDoctor(_id, tbSurname.Text, tbName.Text, tbMiddleName.Text, phoneNumber, tbEmail.Text, postsCmbBox.SelectedValue.ToString(), medicalFacilityCmbBox.SelectedValue.ToString()))
                    return;
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
