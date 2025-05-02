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

namespace TyEmuNuzhen.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectProgramWindow.xaml
    /// </summary>
    public partial class SelectProgramWindow : Window
    {
        public byte program;

        public SelectProgramWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnHealthRoute_Click(object sender, RoutedEventArgs e)
        {
            program = 1;
            DialogResult = true;
        }

        private void btnToBeOnTime_Click(object sender, RoutedEventArgs e)
        {
            program = 2;
            DialogResult = true;
        }
    }
}
