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

namespace TyEmuNuzhen.Views.Pages.Curator.ChildrensWork
{
    /// <summary>
    /// Логика взаимодействия для MonitoringPage.xaml
    /// </summary>
    public partial class MonitoringPage : Page
    {
        public MonitoringPage()
        {
            InitializeComponent();
        }

        private void LoadChildrenData()
        {
            int countRecords = 0;
            string _idRegion = regionsCmbBox.SelectedValue == null ? null : regionsCmbBox.SelectedValue.ToString();
            string _dateAddedBeginPeriod = dateAddedBeginPeriodPicker.SelectedDate == null ? null : dateAddedBeginPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string _dateAddedEndPeriod = dateAddedEndPeriodPicker.SelectedDate == null ? null : dateAddedEndPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string _searchQuery = searchTxt == null ? null : searchTxt.Text;
            bool _isDESC = true;
            if (sortCmbBox.SelectedIndex == 0)
                _isDESC = false;
            string countAllRecords = ChildrensClass.GetCountChildrensMonitoring(_idRegion);
            ChildrensClass.GetChildrenList("1", _idRegion, _dateAddedBeginPeriod, _dateAddedEndPeriod, _searchQuery, _isDESC);
            childrenContainer.Children.Clear();
            if (ChildrensClass.dtChildrensList.Rows.Count > 0)
            {
                lbl.Visibility = Visibility.Hidden;
                foreach (DataRow row in ChildrensClass.dtChildrensList.Rows)
                {
                    ChildrensUserControl childControl = new ChildrensUserControl(
                        row["ID"].ToString(), row["numOfQuestionnaire"].ToString(), row["urlOfQuestionnaire"].ToString(),
                        row["fullName"].ToString(), Convert.ToDateTime(row["birthday"]), Convert.ToDateTime(row["dateDescriptionAdded"]), row["description"].ToString(),
                        CustomFunctionsClass.CalculateAge(Convert.ToDateTime(row["birthday"])), row["regionName"].ToString(), row["latestPhotoPath"].ToString(),
                        Convert.ToDateTime(row["dateAdded"]), row["isAlert"].ToString(), 2
                    );

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RegionsClass.GetRegionsList();
            regionsCmbBox.ItemsSource = RegionsClass.dtRegions.DefaultView;
            regionsCmbBox.DisplayMemberPath = "regionName";
            regionsCmbBox.SelectedValuePath = "ID";
            sortCmbBox.SelectedIndex = 1;
            LoadChildrenData();
        }

        private void addChildBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddChildrenInfoCuratorPage());
        }

        private void regionsCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (regionsCmbBox.SelectedIndex != -1)
                LoadChildrenData();
        }

        private void btnRefreshFiltration_Click(object sender, RoutedEventArgs e)
        {
            regionsCmbBox.SelectedIndex = -1;
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
