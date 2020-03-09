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

        private TaskResult result;

        public ResultsViewer()
        {
            InitializeComponent();
        }

        public void SetValues(TaskResult result)
        {
            this.result = result;
            SetTimeLabel(result.ElapsedTime.Hours, result.ElapsedTime.Minutes, result.ElapsedTime.Seconds);
            goodAnswersLabel.Content = result.GoodAnswers.ToString();
            badAnswersLabel.Content = result.BadAnswers.ToString();
            SetChart();
        }

        private void SetChart()
        {
            int allQuestion = result.GoodAnswers + result.BadAnswers;
            float goodPercentage = ((float)result.GoodAnswers / (float)allQuestion) * 100;
            float badPercentage = ((float)result.BadAnswers / (float)allQuestion) * 100;
            percentageLabel.Content = goodPercentage.ToString("0.00") + "%";
            goodPie.Values = new LiveCharts.ChartValues<float> { goodPercentage };
            badPie.Values = new LiveCharts.ChartValues<float> { badPercentage };
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

        private void ShowGoodAnswersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowBadAnswersButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
