using System;
using System.Collections.Generic;
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
using TyEmuNuzhen.Views.Windows.DialogWindows;

namespace TyEmuNuzhen.Views.Pages.Director.DoctorsOnAgreement
{
    /// <summary>
    /// Логика взаимодействия для MedicalFacilitiesPage.xaml
    /// </summary>
    public partial class MedicalFacilitiesPage : Page
    {
        public MedicalFacilitiesPage()
        {
            InitializeComponent();
            LoadMedicalFacilities();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadMedicalFacilities();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMedicalFacilityWindow addMedicalFacilityWindow = new AddMedicalFacilityWindow();
            if (addMedicalFacilityWindow.ShowDialog() == true)
                LoadMedicalFacilities();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            AddMedicalFacilityWindow addMedicalFacilityWindow = new AddMedicalFacilityWindow(btn.Tag.ToString());
            if (addMedicalFacilityWindow.ShowDialog() == true)
                LoadMedicalFacilities();
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                return;
            var btn = (Button)sender;
            if (MedicalFacilityClass.DeleteMedicalFacility(btn.Tag.ToString()))
                LoadMedicalFacilities();
        }

        private void LoadMedicalFacilities()
        {
            MedicalFacilityClass.GetMedicalFacilityList(searchTextBox.Text);
            medicalFacilitiesGrid.ItemsSource = MedicalFacilityClass.dtMedicalFacilityList.DefaultView;
            CountRecords();
        }

        private void CountRecords()
        {
            string _countAllRecords = MedicalFacilityClass.GetCountAllMedicalFacility();
            string countResRecords = MedicalFacilityClass.dtMedicalFacilityList.Rows.Count.ToString();
            if (_countAllRecords == "0")
            {
                countRecords.Text = "Записей нет";
                return;
            }
            countRecords.Text = $"{countResRecords} из {_countAllRecords} записей";
        }
    }
}
