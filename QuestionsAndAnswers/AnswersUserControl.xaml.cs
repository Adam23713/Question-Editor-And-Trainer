using System;
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

namespace QuestionsAndAnswers
{
    /// <summary>
    /// Interaction logic for AnswersUserControl.xaml
    /// </summary>
    public partial class AnswersUserControl : UserControl
    {
        private int currentRowIndex = -1;
        private int currentColumnIndex = 0;
        private List<AnswerButton> answersButtons = new List<AnswerButton>();

        public AnswersUserControl()
        {
            InitializeComponent();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            var plusImg = Properties.Resources.plus;
            var removeImg = Properties.Resources.remove;
            var acceptImg = Properties.Resources.accept;
            plusButtonBrush.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(plusImg);
            removeButtonBrush.ImageSource = Common.QuestionsAndAnswers.ToBitmapImage(removeImg);
        }

        public void SetAnswers(List<Common.Answer> answers)
        {
            Reset();
            foreach (var answer in answers)
            {
                AddNewAnswer();
                var bt = answersButtons.ElementAt(answersButtons.Count - 1);
                bt.Text = answer.answer;
                bt.SetButton(answer.right);
            }
        }

        public void Reset()
        {
            currentRowIndex = -1;
            currentColumnIndex = 0;
            listViewGrid.Children.Clear();
            answersButtons = new List<AnswerButton>();
        }

        public List<Common.Answer> GetAnswers()
        {
            bool isRightExsist = false;
            List<Common.Answer> list = new List<Common.Answer>();

            foreach (var item in answersButtons)
            {
                if (item.Check)
                {
                    isRightExsist = true;
                }
                list.Add(new Common.Answer(item.Text, item.Check));
            }

            if (list.Count == 0 || list.Count ==1)
            {
                list.Clear();
                MessageBox.Show("Too few answers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (!isRightExsist)
            {
                list.Clear();
                MessageBox.Show("There is no right answer!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return (list.Count == 0) ? null : list;
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewAnswer();
        }

        private void AddNewAnswer()
        {
            AnswerButton bt = new AnswerButton();
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

            Grid.SetRow(bt, currentRowIndex);
            Grid.SetColumn(bt, currentColumnIndex);
            answersButtons.Add(bt);
            listViewGrid.Children.Add(bt);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveAnswer();
        }

        private void RemoveAnswer()
        {
            if (answersButtons.Count == 0) return;

            AnswerButton bt = answersButtons.ElementAt(answersButtons.Count - 1);
            answersButtons.Remove(bt);
            listViewGrid.Children.Remove(bt);

            int size = answersButtons.Count;
            if (size % 2 == 0)
            {
                currentRowIndex--;
                currentColumnIndex = 1;
                listViewGrid.RowDefinitions.RemoveAt(listViewGrid.RowDefinitions.Count - 1);
            }
            else
            {
                currentColumnIndex = 0;
            }
        }
    }
}
