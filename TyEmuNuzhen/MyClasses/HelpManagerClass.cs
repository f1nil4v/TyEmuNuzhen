using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TyEmuNuzhen.Views.Windows.DialogWindows;

namespace TyEmuNuzhen.MyClasses
{
    internal class HelpManagerClass
    {
        public static string CurrentHelpKey { get; set; } = "DefaultHelp";

        /// <summary>
        /// Показывает справку для текущего контекста
        /// </summary>
        public static void ShowHelp()
        {
            ReferenceInformationWindow helpWindow = new ReferenceInformationWindow(CurrentHelpKey);
            helpWindow.ShowDialog();
        }
    }
}
