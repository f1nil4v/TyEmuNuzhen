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

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.MedicalExamination
{
    /// <summary>
    /// Логика взаимодействия для MedicalExaminationChildrensPage.xaml
    /// </summary>
    public partial class MedicalExaminationChildrensPage : Page
    {
        public MedicalExaminationChildrensPage()
        {
            InitializeComponent();
            HelpManagerClass.CurrentHelpKey = "CuratorMedicalExaminationChildrensPage";
            LoadFiltartions();
            LoadChildrenData();
        }

        private void LoadChildrenData()
        {
            int countRecords = 0;
            string _idRegion = regionsCmbBox.SelectedValue == null ? null : regionsCmbBox.SelectedValue.ToString();
            string _idOrphanage = orphanagesCmbBox.SelectedValue == null ? null : orphanagesCmbBox.SelectedValue.ToString();
            string _dateAddedBeginPeriod = dateAddedBeginPeriodPicker.SelectedDate == null ? null : dateAddedBeginPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string _dateAddedEndPeriod = dateAddedEndPeriodPicker.SelectedDate == null ? null : dateAddedEndPeriodPicker.SelectedDate.Value.ToString("yyyy-MM-dd");
            string _searchQuery = searchTxt == null ? null : searchTxt.Text;
            bool _isDESC = true;
            if (sortCmbBox.SelectedIndex == 0)
                _isDESC = false;
            string countAllRecords = ChildrensClass.GetCountChildrensMonitoring(_idRegion, "3");
            ChildrensClass.GetChildrenMedicalExaminationList(_idRegion, _idOrphanage, _dateAddedBeginPeriod, _dateAddedEndPeriod, _searchQuery, _isDESC);
            childrenContainer.Children.Clear();
            if (ChildrensClass.dtChildrensCuratorList.Rows.Count > 0)
            {
                lbl.Visibility = Visibility.Hidden;
                foreach (DataRow row in ChildrensClass.dtChildrensCuratorList.Rows)
                {
                    ChildrensCuratorUserControl childControl = new ChildrensCuratorUserControl(
                        row["ID"].ToString(),
                        row["fullName"].ToString(), Convert.ToDateTime(row["birthday"]),
                        CustomFunctionsClass.CalculateAge(Convert.ToDateTime(row["birthday"])), row["regionName"].ToString(), row["orphanageName"].ToString(), row["latestPhotoPath"].ToString(),
                        Convert.ToDateTime(row["dateAdded"]), 1);

                    SolidColorBrush solidColorBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFCF5FD3");

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

        private void LoadFiltartions()
        {
            RegionsClass.GetRegionsList();
            regionsCmbBox.ItemsSource = RegionsClass.dtRegions?.DefaultView;
            regionsCmbBox.DisplayMemberPath = "regionName";
            regionsCmbBox.SelectedValuePath = "ID";

            string _idRegion = regionsCmbBox.SelectedValue == null ? null : regionsCmbBox.SelectedValue.ToString();

            OrphanageClass.GetOrphanagesForComboBoxList(_idRegion);
            orphanagesCmbBox.ItemsSource = OrphanageClass.dtOrphanagesForComboBoxList?.DefaultView;
            orphanagesCmbBox.DisplayMemberPath = "nameOrphanage";
            orphanagesCmbBox.SelectedValuePath = "ID";
            sortCmbBox.SelectedIndex = 1;
        }

        private void searchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadChildrenData();
        }

        private void regionsCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            orphanagesCmbBox.SelectedIndex = -1;
            string _idRegion = regionsCmbBox.SelectedValue == null ? null : regionsCmbBox.SelectedValue?.ToString();
            OrphanageClass.GetOrphanagesForComboBoxList(_idRegion);
            orphanagesCmbBox.ItemsSource = OrphanageClass.dtOrphanagesForComboBoxList?.DefaultView;
            orphanagesCmbBox.DisplayMemberPath = "nameOrphanage";
            orphanagesCmbBox.SelectedValuePath = "ID";
            LoadChildrenData();
        }

        private void orphanagesCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChildrenData();
        }

        private void statusProgramCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChildrenData();
        }

        private void btnRefreshFiltration_Click(object sender, RoutedEventArgs e)
        {
            orphanagesCmbBox.SelectedIndex = -1;
            regionsCmbBox.SelectedIndex = -1;
            searchTxt.Text = "";
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

        private void btnRefreshFiltrationDate_Click(object sender, RoutedEventArgs e)
        {
            dateAddedBeginPeriodPicker.SelectedDate = null;
            dateAddedEndPeriodPicker.SelectedDate = null;
            LoadChildrenData();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadChildrenData();
        }
    }
}
