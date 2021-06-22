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
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, SubscriberData> gData;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetParam setParam = new SetParam();
            setParam.checkConfFile();

            gData = new Dictionary<string, SubscriberData>();

            PathFolderParam pfp = new PathFolderParam();
            pfp.ChooseFolder();

            string[] filesName = pfp.FilesName;

            foreach(string pathToFile in filesName)
            {
                parseFile(pathToFile);
            }


            foreach (KeyValuePair<string, SubscriberData> kvp in gData)
            {
                Console.WriteLine("Key = {0}, Value = {1}",
                    kvp.Key, kvp.Value.eventId);
            }
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
                        string key = sd.SubNumber + "_" + sd.eventId;
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
                _subscriberData.eventId = getEvent(parameterData);
                return true;
            }

            return false;
        }

        private string getEvent(string par)
        {
            string retVal = "";
            string[] container;

            container = par.Split('.', ':');
            par = container[3];
            retVal = par.Replace("\"", "");

            Console.WriteLine(retVal);

            return retVal;
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
