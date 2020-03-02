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

namespace Examiner.Panels
{
    /// <summary>
    /// Interaction logic for TrainingModePanel.xaml
    /// </summary>
    public partial class TrainingModePanel : UserControl
    {
        public delegate void CheckBoxCheckedChanged(bool value);
        public delegate void Start(TaskSettings taskSettings, bool started);

        public CheckBoxCheckedChanged SetNextButton;
        public Start StartTask;
        private bool startButtonPressed = false;

        public TrainingModePanel()
        {
            InitializeComponent();
        }

        public void SetTimeLabel(int hour, int minute, int second)
        {
            string hourStr = (hour < 10) ? "0" + hour.ToString() : hour.ToString();
            string minuteStr = (minute < 10) ? "0" + minute.ToString() : minute.ToString();
            string secondtr = (second < 10) ? "0" + second.ToString() : second.ToString();
            timeLabel.Content = hourStr + ":" + minuteStr + ":" + secondtr;
        }

        private void TaskTimeCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            taskTimeLimitTimeSetter.IsEnabled = (bool)taskTimeLimitCheckBox.IsChecked;
        }

        private void OneQuestionTimeCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            oneQuestionTimeLimitTimeSetter.IsEnabled = (bool)oneQuestionTimeLimitCheckBox.IsChecked;
        }

        private void AutoShiftingCheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            SetNextButton?.Invoke((bool)autoShiftingCheckBox.IsChecked);
        }

        private TaskSettings GetSettings()
        {
            TaskSettings settings = new TaskSettings();
            settings.TaskTimeLimit = taskTimeLimitTimeSetter.GetTime;
            settings.QuestionTimeLimit = oneQuestionTimeLimitTimeSetter.GetTime;
            settings.isTaskLimitActive = (bool)taskTimeLimitCheckBox.IsChecked;
            settings.isQuestionLimitActive = (bool)oneQuestionTimeLimitCheckBox.IsChecked;
            return settings;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            startButtonPressed = !startButtonPressed;

            if (StartTask != null)
            {
                ImageBrush buttonBackground = new ImageBrush();
                if (startButtonPressed)
                {
                    buttonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Restart);
                }
                else
                {
                    buttonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Start);
                }
                startButton.Background = buttonBackground;
                StartTask(GetSettings(), startButtonPressed);
            }
        }
    }
}
