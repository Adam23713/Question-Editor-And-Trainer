using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Examiner.BaseClasses
{
    public class ArrivedData
    {
        public string Text;
        public bool Right;
        public int SelectedIndex;

        public ArrivedData(string text, bool isRight, int index)
        {
            Text = text;
            Right = isRight;
            SelectedIndex = index;
        }
    }

    class TrainingMode : TaskMode
    {
        private bool stopTimeMeasurer = false;
        private bool waitingForNextEvent = false;
        private TimeSpan elapsedTime;
        private TimeSpan questionElapsedTime;
        private int goodAnswers = 0;
        private List<AnswersResult> answersResults = null; 
        private ArrivedData arrivedAnswer = new ArrivedData(string.Empty, false, -1);

        public TrainingMode()
        {
            ModeName = "Training";
        }

        public override void AnswerArrived(string text, bool isRight, int selectedIndex)
        {
            if (automaticShifting)
            {
                ProcessArrivedAnswer(text, isRight, selectedIndex);
            }
            else
            {
                taskTimer.Stop();
                questionTimer.Stop();
                waitingForNextEvent = true;
                arrivedAnswer = new ArrivedData(text, isRight, selectedIndex);
            }
        }

        private void ProcessArrivedAnswer(string text, bool isRight, int selectedIndex)
        {
            //Save result
            if (isRight) goodAnswers++;
            AnswersResult currentAnswersResult = new AnswersResult();
            currentAnswersResult.Question = CurrentQuestion;
            currentAnswersResult.SelectedAnswerIndex = selectedIndex;
            answersResults.Add(currentAnswersResult);


            //Load Next Question
            if (currentQuestionIndex + 1 == questionsAndAnswer.Count)
            {
                Stop();
                IsRuning = false;
                TaskResult result = null;
                if (taskSettings.isTaskLimitActive)
                {
                    result = ActiveTaskTimeLimitResult();
                }
                else
                {
                    result = GenerateResult();
                }
                result.AnswersResults = answersResults;
                Finished(result);
            }
            else
            {
                currentQuestionIndex++;
                CurrentQuestion = questionsAndAnswer.ElementAt(currentQuestionIndex);

                //Restart Question Timer
                questionTimer.Stop();
                questionElapsedTime = new TimeSpan(0, 0, 0);
                SetProgressBarValue(100);
                if(automaticShifting) questionTimer.Start();
            }
            SetIndexLabel(currentQuestionIndex + 1, questionsAndAnswer.Count);
        }

        private TaskResult GenerateResult()
        {
            TaskResult res = new TaskResult();
            res.ElapsedTime = taskTimer.Elapsed;
            res.GoodAnswers = goodAnswers;
            res.BadAnswers = questionsAndAnswer.Count - goodAnswers;

            return res;
        }

        private TaskResult ActiveTaskTimeLimitResult()
        {
            TaskResult res = new TaskResult();
            res.ElapsedTime = taskSettings.TaskTimeLimit - elapsedTime;
            res.GoodAnswers = goodAnswers;
            res.BadAnswers = questionsAndAnswer.Count - goodAnswers;
            return res;
        }

        private void StartTimeMeasurer()
        {
            Task.Factory.StartNew(new Action(()=> {
                while (!stopTimeMeasurer)
                {
                    System.Threading.Thread.Sleep(10);
                    var tm = taskTimer.Elapsed;
                    SetTimeLabel(tm.Hours, tm.Minutes, tm.Seconds, tm.Milliseconds);
                }
                return;
            }));
        }

        protected override void Start()
        {
            goodAnswers = 0;
            answersResults = new List<AnswersResult>();
            taskTimer = new System.Diagnostics.Stopwatch();
            questionTimer = new DispatcherTimer();
            questionTimer.Interval = new TimeSpan(0, 0, 1);
            stopTimeMeasurer = false;

            if (taskSettings.isTaskLimitActive)
            {
                elapsedTime = taskSettings.TaskTimeLimit;
            }
            else //Default
            {
                elapsedTime = new TimeSpan(0, 0, 0);
            }

            StartTimeMeasurer();
            if (taskSettings.isQuestionLimitActive)
            {
                questionElapsedTime = new TimeSpan(0, 0, 0);
                questionTimer.Tick += QuestionTimer_Tick;
            }

            //Set time label
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);

            //Start timers
            taskTimer.Start();
            questionTimer.Start();
        }

        protected override void Stop()
        {
            taskTimer.Stop();
            questionTimer.Stop();
            stopTimeMeasurer = true;

            if (taskSettings.isTaskLimitActive)
            {
              
            }
            else
            {
                //taskTimer.Tick -= TaskTimer_Tick;
            }

            if (taskSettings.isQuestionLimitActive)
            {
                questionTimer.Tick -= QuestionTimer_Tick;
            }
            SetProgressBarValue(100);
            SetTimeLabel(0,0,0,0);
            currentQuestionIndex = 0;
            CurrentQuestion = questionsAndAnswer.ElementAt(currentQuestionIndex);
            SetIndexLabel(currentQuestionIndex + 1, questionsAndAnswer.Count);
        }

        protected override void Pause()
        {
            ChangeRunningState(ModeName, State.Pause);
            taskTimer.Stop();
            questionTimer.Stop();
        }

        protected override void Continue()
        {
            ChangeRunningState(ModeName, State.Runing);
            if (automaticShifting)
            {
                taskTimer.Start();
                questionTimer.Start();
            }

            if (!automaticShifting && !waitingForNextEvent)
            {
                taskTimer.Start();
                questionTimer.Start();
            }
        }

        public override void NextQuestion(object sender, RoutedEventArgs e)
        {
            ProcessArrivedAnswer(arrivedAnswer.Text, arrivedAnswer.Right, arrivedAnswer.SelectedIndex);
            arrivedAnswer = new ArrivedData(string.Empty, false, -1);
            taskTimer.Start();
            questionTimer.Start();
            waitingForNextEvent = false;
        }

        protected override void TaskTimerBack_Tick(object sender, EventArgs e)
        {
            //elapsedTime -= taskTimer.Interval;
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Milliseconds);

            if (elapsedTime.TotalSeconds == 0)
            {
                Stop();
                IsRuning = false;
                Finished(ActiveTaskTimeLimitResult());
            }
        }

        protected override void TaskTimer_Tick(object sender, EventArgs e)
        {
            //elapsedTime += taskTimer.Interval;
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds,elapsedTime.Milliseconds);
        }

        protected override void QuestionTimer_Tick(object sender, EventArgs e)
        {
            questionElapsedTime += questionTimer.Interval;
            if (questionElapsedTime == taskSettings.QuestionTimeLimit)
            {
                SetProgressBarValue(0);
                QuestionTimeOut();
                AnswerArrived(string.Empty, false, -1);
                return;
            }

            float value = (float)questionElapsedTime.TotalSeconds / (float)taskSettings.QuestionTimeLimit.TotalSeconds;
            int percent = (int)(value * 100);
            SetProgressBarValue(100 - percent);
        }
    }
}
