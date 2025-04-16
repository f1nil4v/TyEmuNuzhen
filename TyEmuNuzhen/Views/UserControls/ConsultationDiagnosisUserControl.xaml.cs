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

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ConsultationDiagnosisUserControl.xaml
    /// </summary>
    public partial class ConsultationDiagnosisUserControl : UserControl
    {
        private int _index;

        public ConsultationDiagnosisUserControl(string description, int index)
        {
            InitializeComponent();
            descriptionTextBlock.Text = description;
            _index = index;
        }

        public delegate void DeleteRequestEventHandler();
        public event DeleteRequestEventHandler DeleteRequested;

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DiagnosesClass.selectedDiagnoses.RemoveAt(_index);
            DiagnosesClass.selectedIDDiagnoses.RemoveAt(_index);
            DeleteRequested?.Invoke();
        }
    }
}
