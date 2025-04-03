using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.UserControls;

namespace TyEmuNuzhen.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MonitoringPage.xaml
    /// </summary>
    public partial class MonitoringPage : Page
    {
        public MonitoringPage()
        {
            InitializeComponent();
            LoadChildrenData();
        }

        private void LoadChildrenData()
        {
            ChildrensClass.GetChildrenList("1");

            if (ChildrensClass.dtChildrensList.Rows.Count > 0)
            {
                foreach (DataRow row in ChildrensClass.dtChildrensList.Rows)
                {
                    ChildrensUserControl childControl = new ChildrensUserControl(row["ID"].ToString(), row["numOfQuestionnaire"].ToString(), row["urlOfQuestionnaire"].ToString(),
                        row["fullName"].ToString(), Convert.ToDateTime(row["birthday"]), Convert.ToDateTime(row["dateDescriptionAdded"]), row["description"].ToString(), CalculateAge(Convert.ToDateTime(row["birthday"])), row["latestPhotoPath"].ToString(),
                        Convert.ToDateTime(row["dateAdded"]), row["isAlert"].ToString());

                    SolidColorBrush solidColorBrush = (row["isAlert"].ToString() == "0"
                        ? (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3")
                        : (SolidColorBrush)new BrushConverter().ConvertFrom("#DD880707"));

                    Border border = new Border
                    {
                        Margin = new Thickness(10, 10, 10, 10),
                        BorderBrush = solidColorBrush,
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(10),
                        Background = new SolidColorBrush(Colors.White)
                    };
                    border.Child = childControl;
                    childrenContainer.Children.Add(border);
                }
            }
            else
                lbl.Visibility = Visibility.Visible;
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private void addChildBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Volonteer.UpdateInsertChildrenPage());
        }
    }
}
