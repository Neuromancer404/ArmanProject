using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace ArmanProject
{
    public class KeyView
    {
        public string SubNumber { get; set; }
        public int eventId { get; set; }
        public int value_key { get; set; }
    }

    public partial class MainWindow
    {
        /// <summary>
        /// Заполнение Listbox на форме
        /// </summary>
        private void ListviewFilling()
        {
            List<KeyView> lvi = new List<KeyView>();
            foreach (KeyValuePair<string, SubscriberData> item in gData)
            {
                lvi.Add(new KeyView() { SubNumber = item.Value.SubNumber, eventId = item.Value.eventId, value_key = item.Value.value_key });
            }
            KeyTable.ItemsSource = lvi;
        }


        /// <summary>
        /// Вывод на форму значений из словаря gData
        /// </summary>
        /// <param name="key"></param>
        private void FormFilling(string key)
        {
            SubNumLabel.Content = gData[key].SubNumber;
            EventIdLabel.Content = gData[key].eventId;
            KeyNumLabel.Content = gData[key].value_key;
            SubNameTextBox.Text = gData[key].SubName;
            DiscriptTextBox.Text = gData[key].Discript;
            ValueVisibleCheckBox.IsChecked = gData[key].value_visible;
        }


        /// <summary>
        ///  
        /// </summary>
        private void atStart()
        {
            string[] filesName = FileExtensionCheking(pathToParameterFilesFolder);
            if (filesName.Length == 0)
            {
                return;
            }

            foreach (string pathToFile in filesName)
            {
                parseFile(pathToFile);
            }
        }

        /// <summary>
        /// Проверка существования файла(ов) .par
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string[] FileExtensionCheking(string path)
        {
            string[] pathToParFiles = System.IO.Directory.GetFiles(path, "*.par");

            return pathToParFiles;
        }


        /// <summary>
        /// Формирование ключа из файла для gData
        /// </summary>
        /// <param name="path"></param>
        public void parseFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            string sub = "";
            string key;
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
                        
                        key = sd.SubNumber + "_" + sd.eventId.ToString() + "_" + sd.value_key.ToString();
                        if (gData.ContainsKey(key))
                        {
                            continue;
                        }
                        gData.Add(key, sd);
                    }
                }
            }
        }


        /// <summary>
        /// Проверка наличия ивента в строке
        /// </summary>
        /// <param name="parameterData"></param>
        /// <param name="_subscriberData"></param>
        /// <returns></returns>
        private bool checkEvent(string parameterData, SubscriberData _subscriberData)
        {
            if (parameterData.Contains("eventrecv"))
            {
                _subscriberData.value_key = Convert.ToInt32(getButton(parameterData));
                _subscriberData.eventId = Convert.ToInt32(getEvent(parameterData));
                return true;
            }
            return false;
        }


        public string getButton(string par)
        {
            string str = par.Substring(6, par.Length - 6);
            int a = str.IndexOf(":");
            str = str.Substring(0, a);
            return str;
        }

        private string getEvent(string par)
        {
            string[] container;
            int b = par.IndexOf("\"");
            string str = par.Substring(b + 1);
            b = str.IndexOf("\"");
            str = str.Substring(0, b);
            container = str.Split('|', '&');
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

            return retVal;
        }
    }
}