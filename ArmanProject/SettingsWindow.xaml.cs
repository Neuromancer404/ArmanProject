using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private string pathToParameterFilesFolder;
        private string pathToJsonFile;
        private string pathToConfFile;

        public string PathToParametersFolder { get => pathToParameterFilesFolder; set => pathToParameterFilesFolder = value; }
        public string PathToJsonFile { get => pathToJsonFile; set => pathToJsonFile = value; }
        
        /// <summary>
        /// Конструктор окна настроек
        /// Выводит в TextBox пути к  файлам
        /// </summary>
        /// <param name="_pathToConfFile">Путь к файлу настроек</param>
        public SettingsWindow(string _pathToConfFile)
        {
            InitializeComponent();

            pathToConfFile = _pathToConfFile;
            pathToParameterFilesFolder = "";

            if (File.Exists(pathToConfFile))
            {
                string[] lines = File.ReadAllLines(pathToConfFile);
                if (lines.Length >= 1)
                {
                    pathToParameterFilesFolder = pathToFilePAR.Text = lines[0];
                }
                if (lines.Length >= 2)
                {
                    pathToJsonFile = pathToFileJSON.Text = lines[1];
                }
            }
        }

        private void selectFilePath_Click(object sender, RoutedEventArgs e)
        {
            pathToParameterFilesFolder = getPath();
            pathToFilePAR.Text = pathToParameterFilesFolder;

            string[] name = System.IO.Directory.GetFiles(pathToParameterFilesFolder, "*.par");
            if (name.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("В выбранной папке отстутсвуют файлы с подходящим расширением (.par)",
                                                "Ошибка",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error,
                                                MessageBoxDefaultButton.Button1,
                                                System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pathToJsonFile = getPath() + "\\eventsConfig.json";
            Console.WriteLine(pathToJsonFile);
            pathToFileJSON.Text = pathToJsonFile;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(pathToParameterFilesFolder.Length == 0 || pathToJsonFile.Length == 0)
            {
                return;
            }

            using (StreamWriter sw = new StreamWriter(pathToConfFile, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(pathToParameterFilesFolder);
                sw.WriteLine(pathToJsonFile);
                sw.Close();
            }
        }

        private string getPath()
        {
            FolderBrowserDialog FolderBrowserDialog = new FolderBrowserDialog();
            FolderBrowserDialog.ShowNewFolderButton = true;
            DialogResult result = FolderBrowserDialog.ShowDialog();
            return FolderBrowserDialog.SelectedPath;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
