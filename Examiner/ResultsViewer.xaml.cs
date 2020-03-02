using Examiner.BaseClasses;
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
    /// Interaction logic for ResultsViewer.xaml
    /// </summary>
    public partial class ResultsViewer : UserControl
    {
        public delegate void CloseEvent();
        public CloseEvent Close;

        public ResultsViewer()
        {
            InitializeComponent();
        }

        public void SetValues(TaskResult result)
        {
            SetTimeLabel(result.ElapsedTime.Hours, result.ElapsedTime.Minutes, result.ElapsedTime.Seconds);
            goodAnswersLabel.Content = result.GoodAnswers.ToString();
            badAnswersLabel.Content = result.BadAnswers.ToString();
        }

        private void SetTimeLabel(int hour, int minute, int second)
        {
            string hourStr = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
            string minuteStr = (minute < 10) ? "0" + minute.ToString() : minute.ToString();
            string secondtr = (second < 10) ? "0" + second.ToString() : second.ToString();
            timeLabel.Content = hourStr + ":" + minuteStr + ":" + secondtr;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
