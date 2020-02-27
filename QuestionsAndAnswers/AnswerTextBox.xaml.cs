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
    /// Interaction logic for AnswersButtons.xaml
    /// </summary>
    public partial class AnswerTextBox : UserControl
    {
        public bool Check { get; private set; } = false;
        public string Text { get { return textBox.Text; } set { textBox.Text = value; } }
        private BitmapImage rightImg = null;
        private BitmapImage wrongImg = null;

        public AnswerTextBox()
        {
            InitializeComponent();
            rightImg = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.right);
            wrongImg = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.cancel);
            checkButtonBrush.ImageSource = wrongImg;
        }

        public void SetButton(bool value)
        {
            Check = value;
            SetButtonImage();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Check = !Check;
            SetButtonImage();
        }

        private void SetButtonImage()
        {
            if (Check)
            {
                checkButtonBrush.ImageSource = rightImg;
            }
            else
            {
                checkButtonBrush.ImageSource = wrongImg;
            }
        }
    }
}
