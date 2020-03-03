using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Examiner.BaseClasses
{
    class TrainingMode : TaskMode
    {
        private bool waitingForNextEvent = false;
        private TimeSpan elapsedTime;
        private TimeSpan questionElapsedTime;
        private int goodAnswers = 0;
        private KeyValuePair<string, bool> arrivedAnswer = new KeyValuePair<string, bool>(string.Empty, false);

        public TrainingMode()
        {
            ModeName = "Training";
        }

        public override void AnswerArrived(string text, bool isRight)
        {
            if (automaticShifting)
            {
                ProcessArrivedAnswer(text, isRight);
            }
            else
            {
                taskTimer.Stop();
                questionTimer.Stop();
                waitingForNextEvent = true;
                arrivedAnswer = new KeyValuePair<string, bool>(text, isRight);
            }
        }

        private void ProcessArrivedAnswer(string text, bool isRight)
        {
            if (isRight)
            {
                goodAnswers++;
            }

            //Load Next Question
            if (currentQuestionIndex + 1 == questionsAndAnswer.Count)
            {
                Stop();
                IsRuning = false;
                if (taskSettings.isTaskLimitActive)
                {
                    Finished(ActiveTaskTimeLimitResult());
                }
                else
                {
                    Finished(GenerateResult());
                }
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
            res.ElapsedTime = elapsedTime;
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

        protected override void Start()
        {
            goodAnswers = 0;
            taskTimer = new DispatcherTimer();
            questionTimer = new DispatcherTimer();
            taskTimer.Interval = new TimeSpan(0, 0, 1);
            questionTimer.Interval = new TimeSpan(0, 0, 1);
           

            if (taskSettings.isTaskLimitActive)
            {
                elapsedTime = taskSettings.TaskTimeLimit;
                taskTimer.Tick += TaskTimerBack_Tick;
                
            }
            else //Default
            {
                elapsedTime = new TimeSpan(0, 0, 0);
                taskTimer.Tick += TaskTimer_Tick;
            }

            if (taskSettings.isQuestionLimitActive)
            {
                questionElapsedTime = new TimeSpan(0, 0, 0);
                questionTimer.Tick += QuestionTimer_Tick;
            }

            //Set time label
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);

            //Start timers
            taskTimer.Start();
            questionTimer.Start();
        }

        protected override void Stop()
        {
            taskTimer.Stop();
            questionTimer.Stop();

            if (taskSettings.isTaskLimitActive)
            {
                taskTimer.Tick -= TaskTimerBack_Tick;
            }
            else
            {
                taskTimer.Tick -= TaskTimer_Tick;
            }

            if (taskSettings.isQuestionLimitActive)
            {
                questionTimer.Tick -= QuestionTimer_Tick;
            }
            SetProgressBarValue(100);
            SetTimeLabel(0,0,0);
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
            ProcessArrivedAnswer(arrivedAnswer.Key, arrivedAnswer.Value);
            arrivedAnswer = new KeyValuePair<string, bool>(string.Empty, false);
            taskTimer.Start();
            questionTimer.Start();
            waitingForNextEvent = false;
        }

        protected override void TaskTimerBack_Tick(object sender, EventArgs e)
        {
            elapsedTime -= taskTimer.Interval;
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);

            if (elapsedTime.TotalSeconds == 0)
            {
                Stop();
                IsRuning = false;
                Finished(ActiveTaskTimeLimitResult());
            }
        }

        protected override void TaskTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime += taskTimer.Interval;
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }

        protected override void QuestionTimer_Tick(object sender, EventArgs e)
        {
            questionElapsedTime += questionTimer.Interval;
            if (questionElapsedTime == taskSettings.QuestionTimeLimit)
            {
                SetProgressBarValue(0);
                QuestionTimeOut();
                AnswerArrived(string.Empty, false);
                return;
            }

            float value = (float)questionElapsedTime.TotalSeconds / (float)taskSettings.QuestionTimeLimit.TotalSeconds;
            int percent = (int)(value * 100);
            SetProgressBarValue(100 - percent);
        }
    }
}
