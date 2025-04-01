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
                switch (authorizationData)
                {
                    case "1":
                        this.Hide();
                        mainWindow = new MainWindow();
                        mainWindow.Show();
                        break;
                    case "2":
                        this.Hide();
                        mainWindow = new MainWindow();
                        mainWindow.Show();
                        break;
                    case "3":
                        this.Hide();
                        mainWindow = new MainWindow();
                        mainWindow.Show();
                        break;
                    default:
                        passwordBox.Password = "";
                        passwordTextBox.Text = "";
                        loginTextBox.Text = "";
                        loginBorder.BorderThickness = new Thickness(2);
                        passwordBorder.BorderThickness = new Thickness(2);
                        loginBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DD880707");
                        passwordBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DD880707");
                        errorPassLogLabel.Content = "*Неверный логин или пароль";
                        ShakeElement(errorPassLogLabel);
                        break;
                }
            }
            else
            {
                errorPassLogLabel.Content = "*Заполните все поля";
                ShakeElement(errorPassLogLabel);
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
            errorPassLogLabel.Content = " ";
            loginBorder.BorderThickness = new Thickness(1);
            passwordBorder.BorderThickness = new Thickness(1);
            loginBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
            passwordBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            errorPassLogLabel.Content = " ";
            loginBorder.BorderThickness = new Thickness(1);
            passwordBorder.BorderThickness = new Thickness(1);
            loginBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
            passwordBorder.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#A101A6");
        }

        private void ShakeElement(UIElement element)
        {
            TranslateTransform trans = new TranslateTransform();
            element.RenderTransform = trans;

            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames();
            anim.Duration = TimeSpan.FromMilliseconds(200);

            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.1)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromPercent(0.3)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(-10, KeyTime.FromPercent(0.5)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(10, KeyTime.FromPercent(0.7)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(-5, KeyTime.FromPercent(0.9)));
            anim.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));

            trans.BeginAnimation(TranslateTransform.XProperty, anim);
        }
    }
}
