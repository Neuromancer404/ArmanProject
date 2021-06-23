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
        public string pathDir = "";
        string example;
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void selectFilePath_Click(object sender, RoutedEventArgs e)
        {
            PathFolderParam _pathFolderParam = new PathFolderParam();
            _pathFolderParam.ChooseFolder();
            pathToFilePAR.Text = pathDir = _pathFolderParam.getPath();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            return;
            string a = File.ReadLines("ArmanProject.conf").First();
            pathToFilePAR.Text = a;
        }
    }

}
