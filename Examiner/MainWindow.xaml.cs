using Common;
using System;
using Examiner.BaseClasses;
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

        private TaskModeFactory.Mode currentMode = TaskModeFactory.Mode.Unknow;
        private Dictionary<TaskModeFactory.Mode, TaskMode> availableTaskModes = null;
        private System.Windows.Media.Effects.Effect bodyEffect = null;

        public MainWindow()
        {
            InitializeComponent();
            FileName = string.Empty;
            bodyEffect = bodyGrid.Effect;
            bodyGrid.Effect = null;
            this.Icon = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Question);
            InitEvents();
            InitTaskModes();
        }

        private void InitEvents()
        {
            this.Closing += ExitFromApplication;
            filePanel.LoadData += LoadedQuestionsAndAnswers;
            filePanel.ExitFromProgram += ExitFromApplication;
            trainingModePanel.ShowBodyGrid += ShowBodyGrid;
            trainingModePanel.SetNextButton += SetNextButton;
            trainingModePanel.SetNextButton += choicesUserControl.SetAutomaticShifting;
            trainingModePanel.ShowQuestionProgressBar += ShowProgressBar;
            resultsViewer.Close += CloseResultViewer;
        }

        private void ShowProgressBar(bool value)
        {
            questionProgressBar.Visibility = (value) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void InitTaskModes()
        {
            availableTaskModes = new Dictionary<TaskModeFactory.Mode, TaskMode>();

            //Training Mode
            var trainingMode = TaskModeFactory.CreateMode(TaskModeFactory.Mode.TrainingMode);
            var racingMode = TaskModeFactory.CreateMode(TaskModeFactory.Mode.RaceMode);
            availableTaskModes[TaskModeFactory.Mode.TrainingMode] = trainingMode;
            availableTaskModes[TaskModeFactory.Mode.RaceMode] = racingMode;

            foreach(var item in availableTaskModes)
            {
                TaskMode mode = item.Value;

                //Set Events
                mode.Finished += Finished;
                mode.SetProgressBarValue += SetProgressBarValue;
                mode.ChangeRunningState += ChangeRunningState;
                mode.LoadQuestion += LoadCurrentQuestion;
                mode.SetIndexLabel += SetIndexLabel;
            }

            //Training Panel events
            trainingModePanel.StartTask += trainingMode.StartTask;
            trainingModePanel.StopTask += trainingMode.StopTask;
            trainingModePanel.PauseTask += trainingMode.PauseTask;
            trainingModePanel.Continue += trainingMode.ContinueTask;
            trainingModePanel.SetNextButton += trainingMode.SetAutomaticShifting;
            trainingMode.SetTimeLabel += trainingModePanel.SetTimeLabel;
            trainingMode.QuestionTimeOut += choicesUserControl.QuestionTimeOut;
            previousButton.Click += trainingMode.PreviousQuestion;
            nextButton.Click += trainingMode.NextQuestion;

        }

        private void SetProgressBarValue(int value)
        {
            if (value > 100 || value < 0) return;

            questionProgressBar.Value = value;
        }

        private void CloseResultViewer()
        {
            bodyGrid.Visibility = Visibility.Visible;
            resultsViewer.Visibility = Visibility.Collapsed;
            if (currentMode == TaskModeFactory.Mode.TrainingMode)
            {
                trainingModePanel.IsEnabled = true;
            }
        }

        private void ChangeRunningState(string taskMode, TaskMode.State state)
        {
            string txt = string.Empty;
            if (state == TaskMode.State.Runing)
            {
                txt = "[" + taskMode + ": Running]";
            }
            else if (state == TaskMode.State.Default)
            {
                txt = "[" + taskMode + ": Default State]";
            }
            else if (state == TaskMode.State.Pause)
            {
                txt = "[" + taskMode + ": Paused]";
            }
            FileName = fileName;
            this.Title += " " + txt;
        }

        private void Finished(TaskResult taskResult)
        {
            if (currentMode == TaskModeFactory.Mode.TrainingMode)
            {
                trainingModePanel.Finished();
                trainingModePanel.IsEnabled = false;
            }
            ShowBodyGrid(false);
            resultsViewer.SetValues(taskResult);
            bodyGrid.Visibility = Visibility.Collapsed;
            resultsViewer.Visibility = Visibility.Visible;
        }

        private void SetIndexLabel(int currentIndex, int size)
        {
            indexLabel.Content = currentIndex.ToString() + " / " + size.ToString();
        }

        private void ShowBodyGrid(bool value)
        {
            if (value)
            {
                bodyGrid.Effect = null;
                bodyGrid.IsEnabled = true;
            }
            else
            {
                bodyGrid.Effect = bodyEffect;
                bodyGrid.IsEnabled = false;
            }
        }

        private void SetNextButton(bool value)
        {
            value = !value;
            nextButton.IsEnabled = value;
            nextButton.Visibility = (value) ? Visibility.Visible : Visibility.Hidden;
        }

        void LoadNewImage(BitmapImage bitmap)
        {
            CurrentImage.Source = bitmap;
        }

        void LoadedQuestionsAndAnswers(List<Common.QuestionAndAnswer> list, string fileName)
        {
            if (list == null || list.Count == 0) return;

            if (currentMode != TaskModeFactory.Mode.Unknow)
            {
                choicesUserControl.SendClickEvent -= availableTaskModes[currentMode].AnswerArrived;
            }
            FileName = fileName;
            CloseResultViewer();
            foreach (var item in availableTaskModes)
            {
                item.Value.SetQuestionsAndAnswers(list);
            }
            currentMode = TaskModeFactory.Mode.RaceMode;
            choicesUserControl.SendClickEvent += availableTaskModes[currentMode].AnswerArrived;
            trainingModePanel.IsEnabled = true;
            raceModePanel.IsEnabled = true;
            raceModePanel.FilesPath = System.IO.Path.GetDirectoryName(FileName);
            bodyGrid.Effect = bodyEffect;
            tabControl.SelectedIndex = 2;
            ChangeRunningState(availableTaskModes[currentMode].ModeName, TaskMode.State.Default);
        }

        private void LoadCurrentQuestion(QuestionAndAnswer question)
        {
            LoadNewImage(question.Picture);
            questionTextBox.Text = question.Question;
            choicesUserControl.ShowAnswers(question.Answers);
        }

        void ExitFromApplication(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentMode == TaskModeFactory.Mode.Unknow) return;

            TaskMode mode = availableTaskModes[currentMode];
            if (tabControl.SelectedIndex == 0)
            {
                if (mode.IsRuning)
                {
                    filePanel.IsEnabled = false;
                }
                else
                {
                    filePanel.IsEnabled = true;
                }
                return;
            }

            if (mode.IsRuning)
            {
                return;
            }
            else
            {
                //TODO: -= event
            }
        }
    }
}
