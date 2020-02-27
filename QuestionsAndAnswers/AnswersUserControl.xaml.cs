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
        private bool pushedTemplateButton = false;
        private int currentRowIndex = -1;
        private int currentColumnIndex = 0;
        private List<AnswerTextBox> answersButtons = new List<AnswerTextBox>();
        private List<string> templateAnswers = null;

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
            useTemplateButtonBrush.Source = Common.QuestionsAndAnswers.ToBitmapImage(acceptImg);
        }

        public void SetAnswers(List<Common.Answer> answers)
        {
            Reset(false);
            SetAnswersWithoutReset(answers);
        }

        private void SetAnswersWithoutReset(List<Common.Answer> answers)
        {
            foreach (var answer in answers)
            {
                AddNewAnswer();
                var bt = answersButtons.ElementAt(answersButtons.Count - 1);
                bt.Text = answer.answer;
                bt.SetButton(answer.right);
            }
        }

        public void Reset(bool useDefault = true)
        {
            currentRowIndex = -1;
            currentColumnIndex = 0;
            listViewGrid.Children.Clear();
            answersButtons = new List<AnswerTextBox>();
            if (useDefault && templateAnswers != null)
            {
                List<Common.Answer> answers = new List<Common.Answer>();
                foreach (var item in templateAnswers)
                {
                    answers.Add(new Common.Answer(item, false));
                }
                SetAnswersWithoutReset(answers);
            }
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
            AnswerTextBox bt = new AnswerTextBox();
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

            AnswerTextBox bt = answersButtons.ElementAt(answersButtons.Count - 1);
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

        private void UseTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            if (answersButtons == null || answersButtons.Count == 0) return;

            if (!pushedTemplateButton)
            {
                templateAnswers = new List<string>();
                foreach (var item in answersButtons)
                {
                    templateAnswers.Add(item.Text);
                }
                useTemplateLabel.Content = "Reset template";
                useTemplateButtonBrush.Source = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.reset);
            }
            else
            {
                templateAnswers = null;
                useTemplateLabel.Content = "Use this template";
                useTemplateButtonBrush.Source = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.accept);
            }
            pushedTemplateButton = !pushedTemplateButton;
        }
    }
}
