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
        private TimeSpan elapsedTime;
        private int goodAnswers = 0;
        private int badAnswers = 0;
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
                arrivedAnswer = new KeyValuePair<string, bool>(text, isRight);
            }
        }

        private void ProcessArrivedAnswer(string text, bool isRight)
        {
            if (isRight)
            {
                goodAnswers++;
            }
            else
            {
                badAnswers++;
            }

            //Load Next Question
            if (currentQuestionIndex + 1 == questionsAndAnswer.Count)
            {
                Stop();
                IsRuning = false;
                Finished(GenerateResult());
            }
            else
            {
                currentQuestionIndex++;
                CurrentQuestion = questionsAndAnswer.ElementAt(currentQuestionIndex);
            }
            SetIndexLabel(currentQuestionIndex + 1, questionsAndAnswer.Count);
        }

        private TaskResult GenerateResult()
        {
            TaskResult res = new TaskResult();
            res.ElapsedTime = elapsedTime;
            res.GoodAnswers = goodAnswers;
            res.BadAnswers = badAnswers;

            return res;
        }

        protected override void Start()
        {
            bool isTimerSetted = SetTimers();
            if (!isTimerSetted)
            {
                taskTimer = new DispatcherTimer();
                taskTimer.Interval = new TimeSpan(0, 0, 1);
                taskTimer.Tick += QuestionTimer_Tick;
                elapsedTime = new TimeSpan(0, 0, 0);
                goodAnswers = 0;
                badAnswers = 0;
                taskTimer.Start();
            }
            else
            {
            }
        }

        protected override void Stop()
        {
            taskTimer.Stop();
            taskTimer.Tick -= QuestionTimer_Tick;
            SetTimeLabel(0,0,0);
            currentQuestionIndex = 0;
            CurrentQuestion = questionsAndAnswer.ElementAt(currentQuestionIndex);
            SetIndexLabel(currentQuestionIndex + 1, questionsAndAnswer.Count);
        }

        protected override void Pause()
        {
            ChangeRunningState(ModeName, State.Pause);
            taskTimer.Stop();
        }

        protected override void Continue()
        {
            ChangeRunningState(ModeName, State.Runing);
            taskTimer.Start();
        }

        public override void NextQuestion(object sender, RoutedEventArgs e)
        {
            ProcessArrivedAnswer(arrivedAnswer.Key, arrivedAnswer.Value);
            arrivedAnswer = new KeyValuePair<string, bool>(string.Empty, false);
        }

        protected override void QuestionTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime += taskTimer.Interval;
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }

        protected override void TaskTimer_Tick(object sender, EventArgs e)
        {
        }
    }
}
