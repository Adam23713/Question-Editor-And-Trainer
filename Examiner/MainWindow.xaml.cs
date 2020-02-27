using Common;
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

namespace Examiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region WindowTitle
        private string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                if (fileName != string.Empty) Title = "Examiner " + VERSION + " - " + fileName;
                else Title = "Examiner " + VERSION;
            }
        }
        private string fileName = string.Empty;
        private readonly string VERSION = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Beta";
        #endregion

        private List<QuestionAndAnswer> questionsAndAnswer = null;
        private QuestionAndAnswer currentQuestion = null;

        public MainWindow()
        {
            InitializeComponent();
            FileName = string.Empty;
            this.Icon = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.Question);


            this.Closing += ExitFromApplication;
            filePanel.LoadData += LoadedQuestionsAndAnswers;
            filePanel.ExitFromProgram += ExitFromApplication;
        }

        void LoadNewImage(BitmapImage bitmap)
        {
            CurrentImage.Source = bitmap;
        }

        void LoadedQuestionsAndAnswers(List<Common.QuestionAndAnswer> questionsAndAnswers, string fileName)
        {
            if (questionsAndAnswers == null || questionsAndAnswers.Count == 0) return;

            this.questionsAndAnswer = questionsAndAnswers;
            currentQuestion = this.questionsAndAnswer.ElementAt(0);
            LoadNewImage(currentQuestion.Picture);
            questionTextBox.Text = currentQuestion.Question;
        }

        void ExitFromApplication(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
