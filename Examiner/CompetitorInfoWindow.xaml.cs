using Microsoft.Win32;
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

namespace Examiner
{
    /// <summary>
    /// Interaction logic for CompetitorInfoWindow.xaml
    /// </summary>
    public partial class CompetitorInfoWindow : Window
    {
        public string filesPath { get; private set; } = string.Empty;

        public CompetitorInfoWindow(string path)
        {
            InitializeComponent();
            filesPath = path;
            pathTextBox.Text = filesPath + "\\";
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            filesPath = string.Empty;
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            filesPath = pathTextBox.Text;
            this.Close();
        }

        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog win = new SaveFileDialog();
            win.Filter = "Pdf file (*.pdf)|*.pdf";
            bool result = (bool)win.ShowDialog();
            if (result)
            {
                pathTextBox.Text = win.FileName;
            }
        }

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            pathTextBox.Text = (filesPath + "\\" + nameTextBox.Text).Replace(" ", "_");
        }
    }
}
