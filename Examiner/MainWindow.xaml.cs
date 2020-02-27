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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region WindowTitle
        private string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                if (fileName != string.Empty) Title = "Examiner " + VERSION + " - " + fileName;
                else Title = "Examiner " + VERSION;
            }
        }
        private string fileName = string.Empty;
        private readonly string VERSION = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Beta";
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            FileName = string.Empty;
            this.Icon = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Question);
        }
    }
}
