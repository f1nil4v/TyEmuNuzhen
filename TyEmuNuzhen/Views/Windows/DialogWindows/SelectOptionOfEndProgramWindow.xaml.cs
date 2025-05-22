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

namespace TyEmuNuzhen.Views.Windows.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для SelectOptionOfEndProgramWindow.xaml
    /// </summary>
    public partial class SelectOptionOfEndProgramWindow : Window
    {
        public byte option;

        public SelectOptionOfEndProgramWindow()
        {
            InitializeComponent();
        }

        private void btnHealthRoute_Click(object sender, RoutedEventArgs e)
        {
            option = 1;
            DialogResult = true;
        }

        private void btnEndProgram_Click(object sender, RoutedEventArgs e)
        {
            option = 2;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
