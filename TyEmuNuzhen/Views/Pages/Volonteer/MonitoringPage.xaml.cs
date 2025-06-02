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
            HelpManagerClass.CurrentHelpKey = "VolonteerMonitoringPage";
        }

        private void LoadChildrenData()
        {
            int countRecords = 0;
            string countAllRecords = ChildrensClass.GetCountChildrensMonitoring(VolonteerClass.idRegion, "1");
            string _dateAddedBeginPeriod = dateAddedBeginPeriodPicker.SelectedDate == null ? null : dateAddedBeginPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string _dateAddedEndPeriod = dateAddedEndPeriodPicker.SelectedDate == null ? null : dateAddedEndPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string _searchQuery = searchTxt == null ? null : searchTxt.Text;
            bool _isDESC = true;
            if (sortCmbBox.SelectedIndex == 0)
                _isDESC = false;
            ChildrensClass.GetChildrenList("1", VolonteerClass.idRegion, _dateAddedBeginPeriod, _dateAddedEndPeriod, _searchQuery, _isDESC);
            childrenContainer.Children.Clear();
            if (ChildrensClass.dtChildrensList.Rows.Count > 0)
            {
                lbl.Visibility = Visibility.Hidden;
                foreach (DataRow row in ChildrensClass.dtChildrensList.Rows)
                {
                    ChildrensUserControl childControl = new ChildrensUserControl(row["ID"].ToString(), row["numOfQuestionnaire"].ToString(), row["urlOfQuestionnaire"].ToString(),
                        row["fullName"].ToString(), Convert.ToDateTime(row["birthday"]), Convert.ToDateTime(row["dateDescriptionAdded"]), row["description"].ToString(), CustomFunctionsClass.CalculateAge(Convert.ToDateTime(row["birthday"])), null, row["latestPhotoPath"].ToString(),
                        Convert.ToDateTime(row["dateAdded"]), row["isAlert"].ToString(), 1, 1);

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
                    countRecords++;
                }
            }
            else
                lbl.Visibility = Visibility.Visible;
            countRecordsTxt.Text = $"{countRecords} из {countAllRecords} записей";
        }

        private void addChildBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Volonteer.UpdateInsertChildrenPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            sortCmbBox.SelectedIndex = 1;
            LoadChildrenData();
        }

        private void btnRefreshFiltration_Click(object sender, RoutedEventArgs e)
        {
            searchTxt.Text = "";
            LoadChildrenData();
        }

        private void searchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadChildrenData();
        }

        private void dateAddedBeginPeriodPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateAddedEndPeriodPicker.SelectedDate != null && dateAddedEndPeriodPicker.SelectedDate >= dateAddedBeginPeriodPicker.SelectedDate)
                LoadChildrenData();
        }

        private void dateAddedEndPeriodPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateAddedBeginPeriodPicker.SelectedDate != null && dateAddedEndPeriodPicker.SelectedDate >= dateAddedBeginPeriodPicker.SelectedDate)
                LoadChildrenData();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChildrenData();
        }

        private void btnRefreshFiltrationDate_Click(object sender, RoutedEventArgs e)
        {
            dateAddedBeginPeriodPicker.SelectedDate = null;
            dateAddedEndPeriodPicker.SelectedDate = null;
            LoadChildrenData();
        }

    }
}
