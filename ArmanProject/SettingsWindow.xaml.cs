using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ArmanProject
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            defaultPath();
        }

        public void defaultPath() 
        {
            PathParam PathParam = new PathParam();
            pathToFile.Text = PathParam.setReadFilePath();
        }
    }

    public class PathParam 
    {
        public string setReadFilePath()
        { 
            string readFilePath = System.IO.Directory.GetCurrentDirectory();
            return readFilePath;
        }
    }
    

    
}
