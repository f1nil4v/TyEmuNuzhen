using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNannyOnProgramWindow.xaml
    /// </summary>
    public partial class AddNannyOnProgramWindow : Window
    {
        string _idActualProgram;
        string _idChild;
        public AddNannyOnProgramWindow(string idActualProgram, string idChild)
        {
            InitializeComponent();
            _idActualProgram = idActualProgram;
            _idChild = idChild;
            LoadNannies("");
        }

        private void selectNanny_Click(object sender, RoutedEventArgs e)
        {
            var selectNannyBtn = sender as Button;
            string idNanny = selectNannyBtn.Tag.ToString();
            string costPerDay;
            CostPerDayNannyWindow costPerDayNannyWindow = new CostPerDayNannyWindow();
            if (costPerDayNannyWindow.ShowDialog() == true)
                costPerDay = costPerDayNannyWindow.tbCostPerDay.Text;
            else
                return;
            if (!NanniesOnProgramClass.AddNannyOnProgram(idNanny, _idActualProgram) || !CreateDocumentsClass.CreateAgreementNanny(idNanny, _idChild, costPerDay))
                return;
            DialogResult = true;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadNannies(querySearch);
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadNannies(querySearch);
        }

        private void LoadNannies(string querySearch)
        {
            NanniesClass.GetNanniesListForSelectOnProgram(querySearch, SortValue());
            nanniesGrid.ItemsSource = NanniesClass.dtNanniesForSelectProgramList.DefaultView;
        }

        private string SortValue()
        {
            string sortValue;
            int sortIndex = sortCmbBox.SelectedIndex;
            switch (sortIndex)
            {
                case 0:
                    sortValue = "nannies.surname";
                    break;
                case 1:
                    sortValue = "nannies.name";
                    break;
                case 2:
                    sortValue = "nannies.middleName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }
    }
}
