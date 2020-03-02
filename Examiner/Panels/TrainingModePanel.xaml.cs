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
        public delegate void BoolValueChangedEvent(bool value);
        public delegate void Start(TaskSettings taskSettings);
        public delegate void EmptyEvent();

        public BoolValueChangedEvent SetNextButton;
        public BoolValueChangedEvent ShowBodyGrid;
        public Start StartTask;
        public EmptyEvent StopTask;
        public EmptyEvent PauseTask;
        public EmptyEvent Continue;
        private bool startButtonPressed = false;
        private bool pauseButtonPressed = false;

        public TrainingModePanel()
        {
            InitializeComponent();
        }

        public void Finished()
        {
            DefaultState();
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
            if (StartTask != null)
            {
                if (!startButtonPressed)
                {
                    startButtonPressed = !startButtonPressed;
                    ImageBrush buttonBackground = new ImageBrush();
                    buttonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Restart);
                    checkBoxesGrid.IsEnabled = false;
                    stopButton.IsEnabled = true;
                    pauseButton.IsEnabled = true;
                    StartTask(GetSettings());
                    ShowBodyGrid(true);
                    startButton.Background = buttonBackground;
                }
                else
                {
                    StopTask();
                    StartTask(GetSettings());
                }
                
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            pauseButtonPressed = !pauseButtonPressed;
            ImageBrush buttonBackground = new ImageBrush();
            if (pauseButtonPressed)
            {
               
                buttonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Start);
                startButton.IsEnabled = false;
                ShowBodyGrid(false);
                PauseTask();
            }
            else
            {
                buttonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Pause);
                startButton.IsEnabled = true;
                ShowBodyGrid(true);
                Continue();
            }
            pauseButton.Background = buttonBackground;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultState();
            ShowBodyGrid(false);
            StopTask();
        }

        private void DefaultState()
        {
            ImageBrush pauseButtonBackground = new ImageBrush();
            ImageBrush startButtonBackground = new ImageBrush();
            startButtonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Start);
            pauseButtonBackground.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Pause);
            startButton.Background = startButtonBackground;
            pauseButton.Background = pauseButtonBackground;
            checkBoxesGrid.IsEnabled = true;
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            pauseButton.IsEnabled = false;
            startButtonPressed = false;
            pauseButtonPressed = false;
    }
    }
}
