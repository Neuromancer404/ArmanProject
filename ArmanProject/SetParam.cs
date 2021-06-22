using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArmanProject
{
    class SetParam
    {
        bool check=false;
        private string currDir;
        public void SetData(string path) 
        {
            
            if (check)
            {
                using (StreamWriter sw = new StreamWriter(currDir + "ArmanProject.conf", true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(path);
                    sw.Close();
                }
            }
        }

        public void checkConfFile() 
        {
            currDir = Directory.GetCurrentDirectory();

            if (!File.Exists(currDir + "ArmanProject.conf"))
            {
                File.Create(currDir + "ArmanProject.conf");
            }
            else check = true;
        }
    }
}
