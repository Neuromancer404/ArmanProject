﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace ArmanProject
{
    public partial class MainWindow
    {
        /// <summary>
        /// Заполнение Listbox на форме
        /// </summary>
        private void ListBoxFilling()
        {
            foreach (KeyValuePair<string, SubscriberData> item in gData)
            {
                KeySelectionListBox.Items.Add(item.Key);
                Console.WriteLine("--------Listbox fill-----------");
                Console.WriteLine("{0}", item.Key);
            }
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
            Console.WriteLine("gData value writing");
        }


        /// <summary>
        ///  
        /// </summary>
        private void atStart()
        {
            Console.WriteLine("atStart starting");

            string[] filesName = FileExtensionCheking(pathToParameterFilesFolder);
            if (filesName.Length == 0)
            {
                Console.WriteLine("Error: no .par in {0}", pathToParameterFilesFolder);
                return;
            }

            gData.Clear();

            foreach (string pathToFile in filesName)
            {
                parseFile(pathToFile);
            }

            foreach (KeyValuePair<string, SubscriberData> kvp in gData)
            {
                Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value.eventId);
            }

            ListBoxFilling();
        }

        private string[] FileExtensionCheking(string path)
        {
            string[] pathToParFiles = System.IO.Directory.GetFiles(path, "*.par");

            return pathToParFiles;
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
            string str = par.Substring(b + 1);

            b = str.IndexOf("\"");
            str = str.Substring(0, b);


            container = str.Split('|', '&');
            foreach (string str1 in container)
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

            Console.WriteLine("===={0}", retVal);

            return retVal;
        }
    }
}