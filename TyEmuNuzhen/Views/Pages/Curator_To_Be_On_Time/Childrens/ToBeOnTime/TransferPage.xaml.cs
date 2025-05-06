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

namespace TyEmuNuzhen.Views.Pages.Curator_To_Be_On_Time.Childrens.ToBeOnTime
{
    /// <summary>
    /// Логика взаимодействия для TransferPage.xaml
    /// </summary>
    public partial class TransferPage : Page
    {
        private string _idChild;
        private bool _changedStatus = false;

        public TransferPage(string idChild, string fioChild)
        {
            InitializeComponent();
            _idChild = idChild;
            fullNameChild.Text = fioChild;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_changedStatus == false)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new ChildrensPage(2));
            NavigationService.RemoveBackEntry();
        }
    }
}
