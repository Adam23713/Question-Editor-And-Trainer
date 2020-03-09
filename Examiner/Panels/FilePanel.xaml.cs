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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examiner
{
    /// <summary>
    /// Interaction logic for FileUserControl.xaml
    /// </summary>
    public partial class FileUserControl : UserControl
    {
        public delegate void LoadedQuestionsAndAnswers(List<Common.QuestionAndAnswer> questionsAndAnswers, string fileName);
        public delegate void ExitFromApplication(object sender, System.ComponentModel.CancelEventArgs e);

        public LoadedQuestionsAndAnswers LoadData;
        public ExitFromApplication ExitFromProgram;

        public FileUserControl()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog win = new OpenFileDialog();
            win.Filter = "XML file (*.xml)|*.xml|QZX file (*.qzx)|*.qzx";
            bool? result = win.ShowDialog();
            if (result == null) return;
            if ((bool)result)
            {
                try
                {
                    List<Common.QuestionAndAnswer> list = null;
                    if (System.IO.Path.GetExtension(win.FileName).ToLower() == ".xml")
                    {
                        list = Common.QuestionsAndAnswers.LoadFromXml(win.FileName);
                    }
                    else if (System.IO.Path.GetExtension(win.FileName).ToLower() == ".qzx")
                    {
                        list = Common.QuestionsAndAnswers.LoadFromQZXFile(win.FileName);
                    }
                    LoadData?.Invoke(list, win.FileName);
                }
                catch (System.IO.FileFormatException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitFromProgram?.Invoke(null, null);
        }
    }
}
