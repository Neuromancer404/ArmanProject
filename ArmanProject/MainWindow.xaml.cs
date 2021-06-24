using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArmanProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathToExe;
        private string pathToConfFile;
        private string pathToParameterFilesFolder;
        private string pathToJsonFile;
        private Dictionary<string, SubscriberData> gData;
        private string ActiveKey;

        public MainWindow()
        {
            InitializeComponent();

            pathToExe = Directory.GetCurrentDirectory();
            pathToConfFile = pathToExe + "\\ArmanProject.conf";

            gData = new Dictionary<string, SubscriberData>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(pathToConfFile);
            settingsWindow.ShowDialog();

            pathToParameterFilesFolder = settingsWindow.PathToParametersFolder;
            pathToJsonFile = settingsWindow.PathToJsonFile;

            if (pathToParameterFilesFolder.Length == 0 || pathToJsonFile.Length == 0)
            {
                Console.WriteLine("Error in path's: pathToParameterFilesFolder={0}, pathToJsonFile={1}", pathToParameterFilesFolder, pathToJsonFile);
                return;
            }
            KeySelectionListBox.Items.Clear();
            atStart();
            SetListBoxSelect();
        }

        /// <summary>
        /// Установка выделения на первый элемент listbox
        /// </summary>
        private void SetListBoxSelect()
        {
            if (KeySelectionListBox.Items.Count > 0)
            {
                KeySelectionListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Проверка наличия конфигурационного файла
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(pathToConfFile))
            {
                Console.WriteLine("Error ArmanProject.conf dosent exist");
                return;
            }
            string[] lines = File.ReadAllLines(pathToConfFile);
            if (lines.Length >= 1)
            {
                pathToParameterFilesFolder = lines[0];
            }
            if (lines.Length >= 2)
            {
                pathToJsonFile = lines[1];
            }
            if (pathToParameterFilesFolder.Length == 0 || pathToJsonFile.Length == 0)
            {
                Console.WriteLine("Error in path's");
                return;
            }
            atStart();
            SetListBoxSelect();
        }

        /*========== Entering Data in form ==========*/

        /// <summary>
        /// При выборе значения из ListBox вызывает метод заполнения формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeySelectionListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KeySelectionListBox.Items.Count > 0)
            {
                ActiveKey = KeySelectionListBox.SelectedItem.ToString();
                FormFilling(KeySelectionListBox.SelectedItem.ToString());
            }
        }

        private void DiscriptTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ActiveKey.Length > 0)
            {
                if (gData.ContainsKey(ActiveKey))
                {
                    gData[ActiveKey].Discript = DiscriptTextBox.Text;
                    Console.WriteLine("DiscriptTextBox entire: {0}", DiscriptTextBox.Text);
                }
            }
        }

        private void SubNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ActiveKey.Length > 0)
            {
                if (gData.ContainsKey(ActiveKey))
                {
                    gData[ActiveKey].Discript = SubNameTextBox.Text;
                    Console.WriteLine("SubNameTextBox entire: {0}", SubNameTextBox.Text);
                }
            }
        }
    }
}
