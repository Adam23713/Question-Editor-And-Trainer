﻿using Common;
using System;
using Examiner.ModeClasses;
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

        private TaskMode taskMode = null;
        private System.Windows.Media.Effects.Effect bodyEffect = null;
        TaskModeFactory.Mode selectedTaskMode = TaskModeFactory.Mode.TrainingMode;

        public MainWindow()
        {
            InitializeComponent();
            FileName = string.Empty;
            bodyEffect = bodyGrid.Effect;
            bodyGrid.Effect = null;
            taskMode = TaskModeFactory.CreateMode(selectedTaskMode);
            this.Icon = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Question);
            InitEvents();
        }

        private void InitEvents()
        {
            this.Closing += ExitFromApplication;
            filePanel.LoadData += LoadedQuestionsAndAnswers;
            filePanel.ExitFromProgram += ExitFromApplication;
            taskMode.LoadQuestion += LoadCurrentQuestion;
            nextButton.Click += taskMode.NextQuestion;
            previousButton.Click += taskMode.PreviousQuestion;
            trainingModePanel.SetNextButton += SetNextButton;
        }

        private void SetNextButton(bool value)
        {
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

            FileName = fileName;
            taskMode.SetQuestionsAndAnswers(list);
            SwitchPanel();
        }

        private void SwitchPanel()
        {
            if (selectedTaskMode == TaskModeFactory.Mode.TrainingMode)
            {
                tabControl.SelectedIndex = 1;
                trainingModePanel.IsEnabled = true;
                bodyGrid.Effect = bodyEffect;
            }
            if (selectedTaskMode == TaskModeFactory.Mode.TrainingMode)
            {
                return;
            }
            else
            {
                return;
            }
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
    }
}
