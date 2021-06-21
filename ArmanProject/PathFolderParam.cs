using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;


namespace ArmanProject
{
    class PathFolderParam : SettingsWindow
    {
        public void ChooseFolder()
        {
            FolderBrowserDialog FolderBrowserDialog = new FolderBrowserDialog();
            FolderBrowserDialog.ShowNewFolderButton = true;
            DialogResult result = FolderBrowserDialog.ShowDialog();
        }
    }
}
