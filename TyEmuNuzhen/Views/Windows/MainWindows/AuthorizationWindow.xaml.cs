using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DBConnection.Connect_DB() == false)
                this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DBConnection.Disconnect_DB();
        }

        private void closeApplicationBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void authBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow;
            if (passwordBox.Password != "" && passwordTextBox.Text != null && loginTextBox.Text != null)
            {
                string authorizationData = AuthorizationClass.Authorization(loginTextBox.Text, passwordBox.Password);
                string idUser = null;
                string idEmployee = null;
                switch (authorizationData)
                {
                    case "1":
                        idUser = AuthorizationClass.GetUserId(loginTextBox.Text, passwordBox.Password);
                        idEmployee = VolonteerClass.GetVolonteerID(idUser);
                        string idRegion = VolonteerClass.GetVolonteerRegion(idUser);
                        if (idEmployee != null && idRegion != null)
                        {
                            this.Hide();
                            mainWindow = new MainWindow(authorizationData, idEmployee, "1");
                            mainWindow.Show();
                        }
                        else
                        {
                            errorPassLogLabel.Text = "*Произошла ошибка при авторизации. Информации о данном сотруднике нет.";
                            AnimationsClass.ShakeElement(errorPassLogLabel);
                        }
                        break;
                    case "2":
                        idUser = AuthorizationClass.GetUserId(loginTextBox.Text, passwordBox.Password);
                        idEmployee = CuratorClass.GetCuratorID(idUser);
                        CuratorClass.idCurator = idEmployee;
                        if (idEmployee != null)
                        {
                            this.Hide();
                            mainWindow = new MainWindow(authorizationData, idEmployee, "2");
                            mainWindow.Show();
                        }
                        else
                        {
                            errorPassLogLabel.Text = "*Произошла ошибка при авторизации. Информации о данном сотруднике нет. За помощью обратитесь к администратору системы";
                            AnimationsClass.ShakeElement(errorPassLogLabel);
                        }
                        break;
                    case "3":
                        idUser = AuthorizationClass.GetUserId(loginTextBox.Text, passwordBox.Password);
                        idEmployee = DirectorClass.GetDirectorID(idUser);
                        if (idEmployee != null)
                        {
                            this.Hide();
                            mainWindow = new MainWindow(authorizationData, idEmployee, "3");
                            mainWindow.Show();
                        }
                        else
                        {
                            errorPassLogLabel.Text = "*Произошла ошибка при авторизации. Информации о данном сотруднике нет. За помощью обратитесь к администратору системы";
                            AnimationsClass.ShakeElement(errorPassLogLabel);
                        }
                        break;
                    default:
                        passwordBox.Password = "";
                        passwordTextBox.Text = "";
                        loginTextBox.Text = "";
                        loginBorder.BorderThickness = new Thickness(2);
                        passwordBorder.BorderThickness = new Thickness(2);
                        loginBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DD880707");
                        passwordBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DD880707");
                        errorPassLogLabel.Text = "*Неверный логин или пароль";
                        AnimationsClass.ShakeElement(errorPassLogLabel);
                        break;
                }
            }
            else
            {
                errorPassLogLabel.Text = "*Заполните все поля";
                AnimationsClass.ShakeElement(errorPassLogLabel);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Visibility == Visibility.Visible)
            {
                passwordTextBox.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Collapsed;
                passwordTextBox.Visibility = Visibility.Visible;
                ((Button)sender).Content = "Скрыть пароль";
            }
            else
            {
                passwordBox.Password = passwordTextBox.Text;
                passwordBox.Visibility = Visibility.Visible;
                passwordTextBox.Visibility = Visibility.Collapsed;
                ((Button)sender).Content = "Показать пароль";
            }

        }

        private void loginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            errorPassLogLabel.Text = " ";
            loginBorder.BorderThickness = new Thickness(1);
            passwordBorder.BorderThickness = new Thickness(1);
            loginBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
            passwordBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            errorPassLogLabel.Text = " ";
            loginBorder.BorderThickness = new Thickness(1);
            passwordBorder.BorderThickness = new Thickness(1);
            loginBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
            passwordBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
        }
    }
}
