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
        public DiagnosisWindow()
        {
            InitializeComponent();
            LoadDiagnoses();
        }

        private void LoadDiagnoses()
        {
            DocumentTypeClass.GetDocumentTypes();
            cbDocumentType.ItemsSource = DocumentTypeClass.dtDocumentTypes.DefaultView;
            if (DocumentTypeClass.dtDocumentTypes.Rows.Count > 0)
                cbDocumentType.SelectedIndex = 0;
        }
    }
}
