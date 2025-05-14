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
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для DiagnosisWindow.xaml
    /// </summary>
    public partial class DiagnosisWindow : Window
    {
        private string _id;

        public DiagnosisWindow(string idChild)
        {
            InitializeComponent();
            LoadDiagnoses();
            _id = idChild;
        }

        private void LoadDiagnoses()
        {
            DiagnosesClass.GetDiagnosesForChildrenComboBoxList(_id);
            cbDiagnoses.ItemsSource = DiagnosesClass.dtDiagnosesForChildrenComboBoxList.DefaultView;
            cbDiagnoses.DisplayMemberPath = "diagnosisName";
            cbDiagnoses.SelectedValuePath = "ID";
            if (DiagnosesClass.dtDiagnosesForChildrenComboBoxList.Rows.Count > 0)
                cbDiagnoses.SelectedIndex = 0;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (cbDiagnoses.SelectedValue != null)
            {
                DiagnosesClass.selectedIDDiagnoses.Add(cbDiagnoses.SelectedValue.ToString());
                DiagnosesClass.selectedDiagnoses.Add(cbDiagnoses.Text);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите диагноз", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
