using System;
using System.IO;
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
using Microsoft.Win32;

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
        }

        private void selectFilePath_Click(object sender, RoutedEventArgs e)
        {
            PathParam PathParam = new PathParam();
            pathToFile.Text = PathParam.FileDialogForm();
        }
    }

    public class PathParam 
    {

            //string readFilePath = System.IO.Directory.GetCurrentDirectory();

        
        public string FileDialogForm()
        {
            string fileName = null;
            fileName = OpenFileDialogForm();
            bool check = false;

            while (check != true) 
            {
                check = checkExtension(fileName);
            }
            return fileName;
        }

        public string OpenFileDialogForm() 
        {
            string fileName = null;
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = false,
                CheckPathExists = true,
                Multiselect = false,
                Title = "Выберите файл"
            };

            if (dialog.ShowDialog() == true)
            {
                fileName = dialog.FileName;
            }
            return fileName;

        }

        public bool checkExtension(string name) 
        {
            bool check = name.Contains(".par");
            return check;
        }

    }
    

    
}
