using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Examiner.ModeClasses
{
    class TaskMode
    {
        public delegate void LoadQuestionFunction(QuestionAndAnswer question);
        public LoadQuestionFunction LoadQuestion;

        

        private QuestionAndAnswer CurrentQuestion
        {
            get { return currentQuestion; }
            set { currentQuestion = value; if(currentQuestion != null) LoadQuestion?.Invoke(currentQuestion); }
        }
        private QuestionAndAnswer currentQuestion = null;
        private List<QuestionAndAnswer> questionsAndAnswer = null;

        public TaskMode()
        {
           
        }


        public void SetQuestionsAndAnswers(List<QuestionAndAnswer> list)
        {
            questionsAndAnswer = list;
            CurrentQuestion = this.questionsAndAnswer.ElementAt(0);
        }

        virtual public void PreviousQuestion(object sender, RoutedEventArgs e)
        {

        }

        virtual public void NextQuestion(object sender, RoutedEventArgs e)
        {

        }
    }
}
