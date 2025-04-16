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

namespace TyEmuNuzhen.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для DescriptionUserControl.xaml
    /// </summary>
    public partial class DescriptionUserControl : UserControl
    {
        public DescriptionUserControl(bool isFirst,string date, string description)
        {
            InitializeComponent();
            if (isFirst)
                dateTextBlock.Text = date + " (последнее)";
            else
                dateTextBlock.Text = date;
            descriptionTextBlock.Text = description;
        }
    }
}
