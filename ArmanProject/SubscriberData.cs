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
        //private List<SubscriberData> _subscriberData = new List<SubscriberData> { };

        public void LineRunner(string path) 
        {
            string[] lines = File.ReadAllLines(path);

            foreach(string par in lines)
            {
                if (par.Contains("par11"))
                {
                    getSub(par);
                }
                if(par.Contains("par24"))
                {
                    checkEvent(par);
                }
            }
        }

        private void checkEvent(string parameterData)
        {
            if (parameterData.Contains("eventrecv"))
            {
                getEvent(parameterData);
            }
        }

        private void getEvent(string par)
        {
            string[] container;
            string readyLine;

            container = par.Split('.', ':');
            par = container[3];
            readyLine = par.Replace( "\"", "");
            
            Console.WriteLine(readyLine);
        }

        private void getSub(string parameterData)
        {
            string[] container;
            string a;

            a = parameterData.Substring(3);
            container = a.Split(':', '@');
            parameterData = container[1];            
        }
    }



}
