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
using System.Windows.Shapes;
using TyEmuNuzhen.MyClasses;

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для EndProgramWindow.xaml
    /// </summary>
    public partial class EndProgramWindow : Window
    {
        private string _idChild;
        private string _idActualProgram;
        private byte _countDiagnoses = 0;

        public EndProgramWindow(string idChild, string fullNameChild)
        {
            InitializeComponent();
            txtChildName.Text += fullNameChild;
            _idChild = idChild;
            LoadActualDiagnoses();
            LoadProgramName();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NanniesOnProgramClass.GetActiveNannyOnProgramData(_idActualProgram);
            if (NanniesOnProgramClass.dtActiveNannyOnProgramData.Rows.Count > 0)
            {
                warningNanny.Visibility = Visibility.Visible;
                btnSave.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConclusion.Text))
            {
                MessageBox.Show("Пожалуйста, заполните заключение", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            List<string> selectedDiagnosisIds = new List<string>();
            foreach (var child in diagnosesPanel.Children)
            {
                if (child is CheckBox checkBox && checkBox.IsChecked == true)
                {
                    selectedDiagnosisIds.Add(checkBox.Tag.ToString());
                }
            }
            if (selectedDiagnosisIds.Count != _countDiagnoses)
            {
                SelectOptionOfEndProgramWindow selectOptionOfEndProgramWindow = new SelectOptionOfEndProgramWindow();
                if (selectOptionOfEndProgramWindow.ShowDialog() == true)
                {
                    if (selectOptionOfEndProgramWindow.option == 1)
                    {
                        if (!ChildrensClass.UpdateStatusChildrenEndProgram(_idChild, "6"))
                            return;
                    }
                    else
                    {
                        if (!ChildrensClass.UpdateStatusChildrenEndProgram(_idChild, "11"))
                            return;
                    }
                    if (selectedDiagnosisIds.Count != 0)
                    {
                        if (!ActualDiagnosesClass.DeleteChildrenDiagnosis(selectedDiagnosisIds))
                            return;
                    }
                    if (!CreateDocumentsClass.CreateReportToBeOnTimeChild(_idChild, txtConclusion.Text))
                        return;
                }
                else
                    return;
            }
            else
            {
                if (!ChildrensClass.UpdateStatusChildrenEndProgram(_idChild, "11"))
                    return;
                if (!ActualDiagnosesClass.DeleteChildrenDiagnosis(selectedDiagnosisIds))
                    return;
                if (!CreateDocumentsClass.CreateReportToBeOnTimeChild(_idChild, txtConclusion.Text))
                    return;
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void LoadProgramName()
        {
            string idActualProgram = ActualProgramClass.GetIDLastActualProgramChildren(_idChild);
            string programName = ActualProgramClass.GetLastActualProgramChildren(idActualProgram);
            txtProgramInfo.Text += programName;
            _idActualProgram = idActualProgram;
        }

        private void LoadActualDiagnoses()
        {
            ActualDiagnosesClass.GetChildrenAcutualDiagnoses(_idChild);

            foreach (DataRow row in ActualDiagnosesClass.dtActualChildrenDiagnoses.Rows)
            {
                CheckBox checkBox = new CheckBox
                {
                    Content = row["diagnosisName"].ToString(),
                    Tag = row["ID"],
                    Style = (Style)FindResource("CheckBoxStyle")
                };
                diagnosesPanel.Children.Add(checkBox);
                _countDiagnoses++;
            }
        }
    }
}
