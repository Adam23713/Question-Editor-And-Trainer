using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Examiner.BaseClasses
{
    class TaskMode
    {
        private bool automaticShifting = true;
        private TaskSettings taskSettings = null;
        public delegate void LoadQuestionFunction(QuestionAndAnswer question);
        public LoadQuestionFunction LoadQuestion;

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

        public virtual void Start() { }

        public virtual void Stop() { }

        virtual public void PreviousQuestion(object sender, RoutedEventArgs e){}

        virtual public void NextQuestion(object sender, RoutedEventArgs e){}
    }
}
