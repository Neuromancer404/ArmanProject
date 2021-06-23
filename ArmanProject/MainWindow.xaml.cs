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

            if(pathToParameterFilesFolder.Length == 0 || pathToJsonFile.Length == 0)
            {
                Console.WriteLine("Error in path's: pathToParameterFilesFolder={0}, pathToJsonFile={1}", pathToParameterFilesFolder, pathToJsonFile);
                return;
            }

            atStart();
        }

        

        /// <summary>
        /// 
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


            //KeySelectionComboBox.ItemsSource = new BindingSource(gData.Keys, null);

            atStart();
            FormFilling("343_1_7");
            foreach (KeyValuePair<string, SubscriberData> item in gData)
            {
                KeySelectionListBox.Items.Add(item.Key);
            }
        }
        
        private void FormFilling(string key)
        {
            SubNumLabel.Content = gData[key].SubNumber;
            EventIdLabel.Content = gData[key].eventId;
            KeyNumLabel.Content = gData[key].value_key;


        }

        private void atStart()
        {
            gData.Clear();

            Console.WriteLine("atStart starting");

            PathFolderParam pfp = new PathFolderParam();
            if (pfp.FileExtensionCheking(pathToParameterFilesFolder))
            {
                Console.WriteLine("Error no .par in {0}", pathToParameterFilesFolder);
                return;
            }

            string[] filesName = pfp.FilesName;

            foreach (string pathToFile in filesName)
            {
                parseFile(pathToFile);
            }

            foreach (KeyValuePair<string, SubscriberData> kvp in gData)
            {
                Console.WriteLine("Key = {0}, Value = {1}",
                    kvp.Key, kvp.Value.eventId);
            }
            //comboBoxWriting();
        }

        public void parseFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string sub = "";

            foreach (string par in lines)
            {
                if (par.Contains("par11"))
                {
                    sub = getSub(par);
                }
                if (par.Contains("par24") && sub.Length > 0)
                {
                    SubscriberData sd = new SubscriberData();
                    if (checkEvent(par, sd))
                    {
                        sd.SubNumber = sub;
                        string key = sd.SubNumber + "_" + sd.eventId + "_" + sd.value_key;
                        Console.WriteLine(key);
                        gData.Add(key, sd);
                    }
                }
            }
        }


        private bool checkEvent(string parameterData, SubscriberData _subscriberData)
        {
            if (parameterData.Contains("eventrecv"))
            {
                _subscriberData.value_key = getButton(parameterData);
                _subscriberData.eventId = getEvent(parameterData);
                return true;
            }

            return false;
        }

        public string getButton(string par)
        {
            string str = par.Substring(6, par.Length - 6);
            int a = str.IndexOf(":");
            str = str.Substring(0, a);
            Console.WriteLine("button number = {0}", str);

            return str;
        }

        private string getEvent(string par)
        {
            string[] container;

            int b = par.IndexOf("\"");
            string str = par.Substring(b+1);

            b = str.IndexOf("\"");
            str = str.Substring(0, b);
            

            container = str.Split('|', '&');
            foreach(string str1 in container)
            {
                Console.WriteLine("events = {0}", str1);
            }
            Console.WriteLine("=============================");
            
            return container[0];

        }

        private string getSub(string parameterData)
        {
            string retVal = "";
            string[] container;
            string a;

            a = parameterData.Substring(3);
            container = a.Split(':', '@');

            retVal = container[1];

            Console.WriteLine("===={0}",retVal);

            return retVal;
        }
    }
}
