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


        /// <summary>
        /// Проверка файлов в выбранной директории на наличие .par файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectFilePath_Click(object sender, RoutedEventArgs e)
        {
            string p = pathToParameterFilesFolder;
            pathToParameterFilesFolder = getPath();

            string[] name = System.IO.Directory.GetFiles(pathToParameterFilesFolder, "*.par");
            if (name.Length == 0)
            {

                System.Windows.Forms.MessageBox.Show(
                    "В выбранной папке отстутсвуют файлы с подходящим расширением (.par)",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);

                pathToParameterFilesFolder = p;
            }
            pathToFilePAR.Text = pathToParameterFilesFolder;

         }


        /// <summary>
        /// Устанавливается путь к json файлу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pathToJsonFile = getPath() + "\\eventsConfig.json";
            Console.WriteLine(pathToJsonFile);
            pathToFileJSON.Text = pathToJsonFile;
        }


        /// <summary>
        /// Проверка записи путей к .par и .json и запись из в конфигурационный файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();
            _folderBrowserDialog.ShowNewFolderButton = true;
            DialogResult result = _folderBrowserDialog.ShowDialog();
            return _folderBrowserDialog.SelectedPath;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
