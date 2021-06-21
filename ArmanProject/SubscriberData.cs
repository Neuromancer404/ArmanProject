using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmanProject
{
    class SubscriberData
    {
        private string MAC;
        private string IP;
        private string SubNumber;
        private string EventNumber;
        private string SubName;
        private string Discript;
        private string value_key;
        private bool value_visible;

        private List<string> LinesList = new List<string>() { };
        private List<SubscriberData> _subscriberData = new List<SubscriberData> { };

        public void LineRunner(string path) 
        {
            string[] lines = File.ReadAllLines(path);

            SubName = lines[11];
            //string[] parts = SubName.Split(':');
            //Console.WriteLine(parts[1]);
            int i = 0;

            char[] separators = new char[] { ':', '.' };
            foreach (var sub in SubName)
            {
                SubName = lines[i];
                string[] parts = SubName.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine(parts[0]);
                i++;
            }
        }
    }



}
