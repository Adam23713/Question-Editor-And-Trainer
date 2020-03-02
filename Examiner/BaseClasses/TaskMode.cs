using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;

namespace Examiner.BaseClasses
{
    class TaskMode
    {
        protected DispatcherTimer taskTimer = null;
        protected DispatcherTimer questionTimer = null;
        protected bool automaticShifting = true;
        protected TaskSettings taskSettings = null;

        public delegate void LoadQuestionFunction(QuestionAndAnswer question);
        public delegate void SetTimeLabelFunction(int hour, int minute, int second);
        public LoadQuestionFunction LoadQuestion;
        public SetTimeLabelFunction SetTimeLabel;

        private QuestionAndAnswer CurrentQuestion
        {
            get { return currentQuestion; }
            set { currentQuestion = value; if(currentQuestion != null) LoadQuestion?.Invoke(currentQuestion); }
        }
        private QuestionAndAnswer currentQuestion = null;
        private List<QuestionAndAnswer> questionsAndAnswer = null;

        public void SetQuestionsAndAnswers(List<QuestionAndAnswer> list)
        {
            questionsAndAnswer = list;
            CurrentQuestion = this.questionsAndAnswer.ElementAt(0);
        }

        protected bool SetTimers()
        {
            var taskTimeLimit = taskSettings.TaskTimeLimit;
            var questionTimeLimit = taskSettings.QuestionTimeLimit;

            if (taskSettings.isTaskLimitActive && taskSettings.isQuestionLimitActive)
            {
                if (taskTimeLimit.TotalSeconds < questionTimeLimit.TotalSeconds)
                {
                    MessageBox.Show("Task time limit is less than Question time limit", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            if (taskSettings.isQuestionLimitActive && questionTimeLimit.TotalSeconds < 3)
            {
                MessageBox.Show("Question time limit is less than 3 sec", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (taskSettings.isTaskLimitActive && taskTimeLimit.TotalSeconds > 0)
            {
                taskTimer = new DispatcherTimer();
                taskTimer.Tick += TaskTimer_Tick;
                taskTimer.Interval = taskTimeLimit;
            }
            else
            {
                taskTimer = null;
            }

            if (taskSettings.isQuestionLimitActive && questionTimeLimit.TotalSeconds > 0)
            {
                questionTimer = new DispatcherTimer();
                questionTimer.Tick += QuestionTimer_Tick;
                questionTimer.Interval = questionTimeLimit;
            }
            else
            {
                questionTimer = null;
            }

            if (taskTimer == null && questionTimer == null)
                return false;

            return true;
        }

        protected virtual void QuestionTimer_Tick(object sender, EventArgs e) { }

        protected virtual void TaskTimer_Tick(object sender, EventArgs e) { }

        public virtual void SetAutomaticShifting(bool value)
        {
            automaticShifting = value;
        }

        public virtual void StartTask(TaskSettings taskSettings, bool started)
        {
            if (started)
            {
                this.taskSettings = taskSettings;
                Start();
            }
            else
            {
                Stop();
            }
        }

        protected virtual void Start() { }

        protected virtual void Stop() { }

        virtual public void PreviousQuestion(object sender, RoutedEventArgs e){}

        virtual public void NextQuestion(object sender, RoutedEventArgs e){}
    }
}
