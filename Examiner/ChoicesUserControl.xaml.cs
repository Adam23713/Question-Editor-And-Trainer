using System;
using Common;
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
    /// Interaction logic for ChoicesUserControl.xaml
    /// </summary>
    public partial class ChoicesUserControl : UserControl
    {
        public delegate void ClickButtonEvent(string answer, bool right);
        public ClickButtonEvent SendClickEvent;

        private bool automaticShift = true;
        private int currentRowIndex = -1;
        private int currentColumnIndex = 0;
        private List<AnswerButton> answersButtons = new List<AnswerButton>();

        public ChoicesUserControl()
        {
            InitializeComponent();
        }

        public void Reset(bool useDefault = true)
        {
            currentRowIndex = -1;
            currentColumnIndex = 0;
            listViewGrid.Children.Clear();
            answersButtons = new List<AnswerButton>();
        }

        public void ShowAnswers(List<Answer> answers)
        {
            Reset();
            foreach (var answer in answers)
            {
                AddNewAnswer(answer);
            }
        }

        public void SetAutomaticShifting(bool value)
        {
            automaticShift = value;
        }

        public void QuestionTimeOut()
        {
            ShowResult();
        }

        private void AddNewAnswer(Answer answer)
        {
            AnswerButton bt = new AnswerButton(answer.answer, answer.right);
            Thickness padding = bt.Padding;
            padding.Left = 10;
            padding.Top = 10;
            padding.Right = 10;
            padding.Bottom = 10;
            bt.Padding = padding;

            int size = answersButtons.Count;
            if (size % 2 == 0)
            {
                currentRowIndex++;
                currentColumnIndex = 0;
                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = GridLength.Auto;
                listViewGrid.RowDefinitions.Add(gridRow);
            }
            else
            {
                currentColumnIndex = 1;
            }
            bt.SendClickEvent += ClickEvent;
            Grid.SetRow(bt, currentRowIndex);
            Grid.SetColumn(bt, currentColumnIndex);
            answersButtons.Add(bt);
            listViewGrid.Children.Add(bt);
        }

        public void ClickEvent(string text, bool isRight)
        {
            ShowResult();
            SendClickEvent(text, isRight);
        }

        private void ShowResult()
        {
            if (!automaticShift)
            {
                foreach (var bt in answersButtons)
                {
                    bt.GetAnswerByColor();
                    bt.IsEnabled = false;
                }
            }
        }
    }
}
