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
        public enum State { Runing, Default, Pause };
        protected int currentQuestionIndex = 0;
        public string ModeName { get; protected set; } = string.Empty;

        public bool IsRuning
        {
            get
            {
                return isRuning;
            }
            protected set
            {
                isRuning = value;
                if (isRuning)
                {
                    ChangeRunningState?.Invoke(ModeName, State.Runing);
                }
                else
                {
                    ChangeRunningState?.Invoke(ModeName, State.Default);
                }
            }
        }
        private bool isRuning = false;

        protected DispatcherTimer taskTimer = null;
        protected DispatcherTimer questionTimer = null;
        protected bool automaticShifting = true;
        protected TaskSettings taskSettings = null;

        #region Events
        public delegate void LoadQuestionFunction(QuestionAndAnswer question);
        public delegate void SetTimeLabelFunction(int hour, int minute, int second);
        public delegate void SetIndexLabelFunction(int currentIndex, int size);
        public delegate void EmptyEvent();
        public delegate void FinishedEvent(TaskResult taskResult);
        public delegate void StateChangedEvent(string taskMode, State state);
        public LoadQuestionFunction LoadQuestion;
        public SetTimeLabelFunction SetTimeLabel;
        public SetIndexLabelFunction SetIndexLabel;
        public StateChangedEvent ChangeRunningState;
        public FinishedEvent Finished;
        #endregion

        protected QuestionAndAnswer CurrentQuestion
        {
            get { return currentQuestion; }
            set { currentQuestion = value; if(currentQuestion != null) LoadQuestion?.Invoke(currentQuestion); }
        }
        private QuestionAndAnswer currentQuestion = null;
        protected List<QuestionAndAnswer> questionsAndAnswer = null;

        public void SetQuestionsAndAnswers(List<QuestionAndAnswer> list)
        {
            questionsAndAnswer = list;
            currentQuestionIndex = 0;
            CurrentQuestion = this.questionsAndAnswer.ElementAt(currentQuestionIndex);
            SetIndexLabel(currentQuestionIndex + 1, questionsAndAnswer.Count);
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

        public virtual void SetAutomaticShifting(bool value)
        {
            automaticShifting = value;
        }

        public virtual void StartTask(TaskSettings taskSettings)
        {
            IsRuning = true;
            this.taskSettings = taskSettings;
            Start();
        }

        public virtual void StopTask()
        {
            IsRuning = false;
            Stop();
        }

        public virtual void PauseTask()
        {
            Pause();
        }

        public virtual void ContinueTask()
        {
            Continue();
        }

        protected virtual void Start() { }

        protected virtual void Stop() { }

        protected virtual void Pause() { }

        protected virtual void Continue() { }

        public virtual void AnswerArrived(string text, bool isRight) { }

        protected virtual void QuestionTimer_Tick(object sender, EventArgs e) { }

        protected virtual void TaskTimer_Tick(object sender, EventArgs e) { }

        virtual public void PreviousQuestion(object sender, RoutedEventArgs e){}

        virtual public void NextQuestion(object sender, RoutedEventArgs e){}
    }
}
