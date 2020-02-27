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
        private int currentRowIndex = -1;
        private int currentColumnIndex = 0;
        private List<AnswerButton> answersButtons = new List<AnswerButton>();

        public ChoicesUserControl()
        {
            InitializeComponent();
        }

        public void ShowAnswers(List<Answer> answers)
        {
            foreach (var answer in answers)
            {
                AddNewAnswer(answer);
            }
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

            Grid.SetRow(bt, currentRowIndex);
            Grid.SetColumn(bt, currentColumnIndex);
            answersButtons.Add(bt);
            listViewGrid.Children.Add(bt);
        }
    }
}
