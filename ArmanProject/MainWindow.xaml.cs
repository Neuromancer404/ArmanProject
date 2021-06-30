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
        private List<string> keysList;


        public MainWindow()
        {
            InitializeComponent();
            pathToExe = Directory.GetCurrentDirectory();
            pathToConfFile = pathToExe + "\\ArmanProject.conf";
            gData = new Dictionary<string, SubscriberData>();
        }


        /// <summary>
        /// Открывает окно настроек с выбором пути
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow(pathToConfFile);
            try 
            { 
                settingsWindow.ShowDialog(); 
            }
            catch
            {
                settingsWindow.Activate(); 
                pathToParameterFilesFolder = settingsWindow.PathToParametersFolder;
                pathToJsonFile = settingsWindow.PathToJsonFile;
            }
            finally 
            {
                pathToParameterFilesFolder = settingsWindow.PathToParametersFolder;
                pathToJsonFile = settingsWindow.PathToJsonFile;
            }

            if (pathToParameterFilesFolder.Length == 0 || pathToJsonFile.Length == 0)
            {
                return;
            }

            updateData();
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
                return;
            }

            updateData();
        }


        /// <summary>
        /// Обновляет данные на форме
        /// </summary>
        private void updateData()
        {
            //need clear gdata
            gData.Clear();

            //read json before read .par files
            readJson();
            atStart();
            ListviewFilling();
            //KeyTable.SelectedIndex = 0;
        }


        /*========== Entering Data in form ==========*/

        /// <summary>
        /// Проверка корректности json файла и запись данных из него в gData
        /// </summary>
        private void readJson()
        {
            if (File.Exists(pathToJsonFile))
            {
                List<asd> lasd = new List<asd>();
                string jsonString = null;
                try
                {
                    using (StreamReader sr = new StreamReader(pathToJsonFile))
                    {
                        jsonString = sr.ReadToEnd();
                        sr.Close();
                        jsonString = jsonString.Remove(0, jsonString.IndexOf('[') - 1);
                        jsonString = jsonString.Remove(jsonString.IndexOf(']') + 1);
                    }
                }
                catch 
                {
                    System.Windows.Forms.MessageBox.Show(
                    "Содержание json файла не читается",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                    return; 
                }
                try 
                {
                    lasd = JsonConvert.DeserializeObject<List<asd>>(jsonString);
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show(
                    "Имеющийся json файл некорректен и будет перезаписан",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);

                    return;
                }
                finally
                {
                    foreach (asd a in lasd)
                    {
                        string key = a.subscriber.number + "_" + a.id.ToString() + "_" + a.key.value.ToString();
                        SubscriberData sd = new SubscriberData();

                        if (gData.ContainsKey(key))
                        {
                            continue;
                        }

                        sd.Discript = a.description;
                        sd.eventId = a.id;
                        sd.SubName = a.subscriber.name;
                        sd.SubNumber = a.subscriber.number;
                        sd.value_key = a.key.value;
                        sd.value_visible = a.key.visible;
                        gData.Add(key, sd);
                    }
                }
                }
        }

        /// <summary>
        /// Обработка заполнения описания на форме
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
                }
            }
        }


        /// <summary>
        /// Обработка заполнения Имени абонента
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
                }
            }
            ValueVisibleCheckBox.IsChecked = (bool)gData[ActiveKey].value_visible;
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

        /// <summary>
        /// Устанавливает выделение в Multiple режим
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {


        }


        /// <summary>
        /// Удаление выделенных элементов Listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            keysList = new List<string>();

            if (setAnySelections.IsChecked == false)
            {
                try
                {
                    for (int i = 0; i < KeyTable.SelectedItems.Count; i++)
                    {
                        int selectedIndex = KeyTable.Items.IndexOf(KeyTable.SelectedItems[i]);
                        var asd = KeyTable.Items[selectedIndex] as KeyView;
                        if (asd == null)
                        {
                            return;
                        }
                        string k = asd.SubNumber + "_" + asd.eventId + "_" + asd.value_key;
                        keysList.Add(k);
                    }
                }
                catch
                {
                    return;
                }
            }
            bool flag = false;
            if (setAnySelections.IsChecked == true)//not selected items
            {
                for (int i = 0; i < KeyTable.Items.Count; i++)
                {
                    var asd = KeyTable.Items[i] as KeyView;
                    flag = false;
                    for (int j = 0; j < KeyTable.SelectedItems.Count; j++)
                    {
                        var aaa = KeyTable.SelectedItems[j] as KeyView;
                        if (asd == aaa)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if(!flag)
                    {
                        string k = asd.SubNumber + "_" + asd.eventId + "_" + asd.value_key;
                        keysList.Add(k);
                    }
                }
            }

            for (int i = 0; i < keysList.Count; i++)
            {
                gData.Remove(keysList[i]);
            }

            ListviewFilling();
        }

        private void KeyTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = 0;
            selectedIndex = KeyTable.SelectedIndex;
            try
            {
                var asd = KeyTable.Items[selectedIndex] as KeyView;
                string k = asd.SubNumber + "_" + asd.eventId + "_" + asd.value_key;
                ActiveKey = k;
                FormFilling(k);
            }

            catch { return; }
        }

        private void setAnySelections_Click(object sender, RoutedEventArgs e)
        {
            if (setAnySelections.IsChecked == true)
            {
                DeleteButton.Content = "Удалить невыбранное";
            }
            else
            {
                DeleteButton.Content = "Удалить выбранное";
            }
        }
    }
}

