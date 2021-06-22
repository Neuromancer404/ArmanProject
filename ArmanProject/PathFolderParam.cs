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

            if (result == System.Windows.Forms.DialogResult.OK && FileExtensionCheking(FolderBrowserDialog.SelectedPath))
            {
                pathToFolder = FolderBrowserDialog.SelectedPath;
            }
            
            SetParam setParam = new SetParam();
            setParam.SetData(pathToFolder);
        }

        private bool FileExtensionCheking(string path) 
        {

            name = System.IO.Directory.GetFiles(path, "*.par");
            bool result = false;
            if (name.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                "В выбранной папке отстутсвуют файлы с подходящим расширением (.par)",
                "Ошибка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1,
                System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
            }
            else 
            {
                result = true;
            };
            return result;
        }

        public string getPath()
        {
            return pathToFolder;
        }

        private string pathToFolder;

        private string[] name;

        public string PathToFolder { get => pathToFolder; set => pathToFolder = value; }
        public string[] FilesName { get => name; set => name = value; }
    }
}
