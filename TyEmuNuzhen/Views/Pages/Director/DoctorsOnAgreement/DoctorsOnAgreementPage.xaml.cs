using System.Windows;
using System.Windows.Controls;
using TyEmuNuzhen.MyClasses;
using TyEmuNuzhen.Views.Windows;

namespace TyEmuNuzhen.Views.Pages.Director.DoctorsOnAgreement
{
    /// <summary>
    /// Логика взаимодействия для DoctorsOnAgreementPage.xaml
    /// </summary>
    public partial class DoctorsOnAgreementPage : Page
    {
        public DoctorsOnAgreementPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDoctors("");
            CountRecords();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            AddDoctorWindow addDoctorWindow = new AddDoctorWindow();
            if (!addDoctorWindow.ShowDialog() == true)
                return;
            if (!DoctorsOnAgreementClass.AddDoctor(addDoctorWindow.tbSurname.Text, addDoctorWindow.tbName.Text, addDoctorWindow.tbMiddleName.Text, 
                addDoctorWindow.phoneNumber, addDoctorWindow.tbEmail.Text, addDoctorWindow.postsCmbBox.SelectedValue.ToString(), addDoctorWindow.medicalFacilityCmbBox.SelectedValue.ToString()))
                return;
            LoadDoctors(querySearch);
            CountRecords();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            var deleteBtn = sender as Button;
            DoctorsOnAgreementClass.GetDoctorData(deleteBtn.Tag.ToString());
            if (!DoctorsOnAgreementClass.DeleteDoctor(deleteBtn.Tag.ToString()))
                return;
            LoadDoctors(querySearch);
            CountRecords();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var changeBtn = sender as Button;
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            DoctorsOnAgreementClass.GetDoctorData(changeBtn.Tag.ToString());
            string surname = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["surname"].ToString();
            string name = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["name"].ToString();
            string middleName = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["middleName"].ToString();
            string phoneNumber = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["phoneNumber"].ToString();
            string email = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["email"].ToString();
            string idPost = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["idPost"].ToString();
            string idMedicalFacility = DoctorsOnAgreementClass.dtDoctorDataList.Rows[0]["idMedicalFacility"].ToString();
            AddDoctorWindow addDoctorWindow = new AddDoctorWindow(surname, name, middleName, phoneNumber, email, idPost, idMedicalFacility);
            if (!addDoctorWindow.ShowDialog() == true)
                return;
            if (!DoctorsOnAgreementClass.UpdateDoctor(changeBtn.Tag.ToString(), addDoctorWindow.tbSurname.Text, addDoctorWindow.tbName.Text, 
                addDoctorWindow.tbMiddleName.Text, addDoctorWindow.phoneNumber, addDoctorWindow.tbEmail.Text, addDoctorWindow.postsCmbBox.SelectedValue.ToString(), addDoctorWindow.medicalFacilityCmbBox.SelectedValue.ToString()))
                return;
            LoadDoctors(querySearch);
            CountRecords();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDoctors(querySearch);
            CountRecords();
        }

        private void sortCmbBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string querySearch = string.IsNullOrWhiteSpace(searchTextBox.Text) ? "" : searchTextBox.Text;
            LoadDoctors(querySearch);
            CountRecords();
        }

        private void LoadDoctors(string querySearch)
        {
            DoctorsOnAgreementClass.GetDoctorsList(querySearch, SortValue());
            doctorsGrid.ItemsSource = DoctorsOnAgreementClass.dtDoctorsList.DefaultView;
        }

        private void CountRecords()
        {
            string _countAllRecords = DoctorsOnAgreementClass.GetCountAllDoctors();
            string countResRecords = DoctorsOnAgreementClass.dtDoctorsList.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }

        private string SortValue()
        {
            string sortValue;
            int sortIndex = sortCmbBox.SelectedIndex;
            switch (sortIndex)
            {
                case 0:
                    sortValue = "doctors_on_agreement.surname";
                    break;
                case 1:
                    sortValue = "doctors_on_agreement.name";
                    break;
                case 2:
                    sortValue = "doctors_on_agreement.middleName";
                    break;
                default:
                    sortValue = "";
                    break;
            }
            return sortValue;
        }
    }
}
