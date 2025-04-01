using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                        Convert.ToDateTime(row["dateAdded"])
                    );

                    Border border = new Border
                    {
                        Margin = new Thickness(10, 10, 10, 10),
                        BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3"),
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
    }
}
