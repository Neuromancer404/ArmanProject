using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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


        /// <summary>
        /// Обработка заполнения описания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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


        /// <summary>
        /// Обработка заполнения Имени
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ActiveKey.Length > 0)
            {
                if (gData.ContainsKey(ActiveKey))
                {
                    gData[ActiveKey].SubName = SubNameTextBox.Text;
                    Console.WriteLine("SubNameTextBox entire: {0}", SubNameTextBox.Text);
                }
            }
        }


        /// <summary>
        /// Запись в gData значения visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValueVisibleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ActiveKey.Length > 0)
            {
                if (gData.ContainsKey(ActiveKey))
                {
                    gData[ActiveKey].value_visible = (bool)ValueVisibleCheckBox.IsChecked;
                    Console.WriteLine("SubNameTextBox entire: {0}", SubNameTextBox.Text);
                }
            }
        }


        /// <summary>
        /// Запись данных с формы в формате json в файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeJsonButton_Click(object sender, RoutedEventArgs e)
        {
            /*=========Formating data to json=========*/
            List<asd> events = new List<asd>();

            foreach(SubscriberData entry in gData.Values)
            {
                Subscriber _subscriber = new Subscriber();
                _subscriber.number = entry.SubNumber;
                if (entry.SubName == null)
                {
                    _subscriber.name = "num " + entry.SubNumber;
                }
                else
                {
                    _subscriber.name = entry.SubName;
                }

                Key _key = new Key();
                _key.value = Convert.ToInt32(entry.value_key);
                _key.visible = entry.value_visible;

                asd _asd = new asd();
                _asd.subscriber = _subscriber;
                _asd.key = _key;
                _asd.id = Convert.ToInt32(entry.eventId);
                if (entry.Discript == null)
                {
                    _asd.description = "some description #" + entry.eventId ;
                }
                else
                {
                    _asd.description = entry.Discript;
                }

                events.Add(_asd);

                
            }

            string json = JsonConvert.SerializeObject(new { events } , Formatting.Indented);
            Console.WriteLine(json);

            /*=========Writing to file=========*/

            using (StreamWriter sw = new StreamWriter(pathToJsonFile, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(json);
                sw.Close();
            }
        }

        struct Subscriber
        {
            public string number;
            public string name;
        }

        struct Key
        {
            public int value;
            public bool visible;
        }

        struct asd
        {
            public Subscriber subscriber;
            public Key key;
            public int id;
            public string description;
        }
    }
}

