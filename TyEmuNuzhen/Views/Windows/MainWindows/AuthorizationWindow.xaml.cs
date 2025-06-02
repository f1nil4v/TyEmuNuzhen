using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;
using TyEmuNuzhen.Views.Windows.DialogWindows;
using TyEmuNuzhen.Views.Windows.MainWindows;

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
            LoadScreensaver();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            while (!DBConnection.Connect_DB())
            {
                ConnectDBWindow connectDBWindow = new ConnectDBWindow();
                if (connectDBWindow.ShowDialog() != true)
                {
                    errorPassLogLabel.Text = "Нет подключения к базе данных!";
                    AnimationsClass.ShakeElement(errorPassLogLabel);
                    EnableControls(false);
                    return;
                }
            }
            errorPassLogLabel.Text = " ";
            EnableControls(true);
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

        private async void LoadScreensaver()
        {
            this.Hide();
            ScreensaverWindow window = new ScreensaverWindow();
            window.Show();
            await Task.Delay(5000);
            window.Close();
            this.Show();
        }

        private void connectDBBtn_Click(object sender, RoutedEventArgs e)
        {
            ConnectDBWindow connectDBWindow = new ConnectDBWindow();
            if (connectDBWindow.ShowDialog() == true)
            {
                DBConnection.Disconnect_DB();
                if (DBConnection.Connect_DB())
                {
                    MessageBox.Show("Подключение к базе данных успешно установлено!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    errorPassLogLabel.Text = " ";
                    EnableControls(true);
                }
                else
                {
                    errorPassLogLabel.Text = "Нет подключения к базе данных!";
                    AnimationsClass.ShakeElement(errorPassLogLabel);
                    EnableControls(false);
                }
            }
        }

        private void EnableControls(bool connectDB)
        {
            if (connectDB)
            {
                loginTextBox.IsEnabled = true;
                passwordTextBox.IsEnabled = true;
                passwordBox.IsEnabled = true;
                authBtn.IsEnabled = true;
            }
            else
            {
                loginTextBox.IsEnabled = false;
                passwordTextBox.IsEnabled = false;
                passwordBox.IsEnabled = false;
                authBtn.IsEnabled = false;
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F1)
            {
                ShowHelp();
                e.Handled = true;
            }
        }

        private void ShowHelp()
        {
            ReferenceInformationWindow helpWindow = new ReferenceInformationWindow("AuthorizationHelp");
            helpWindow.ShowDialog();
        }
    }
}
