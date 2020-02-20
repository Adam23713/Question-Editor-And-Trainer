﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string VERSION = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " Beta";

        private string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                if (fileName != string.Empty) Title = "Questions Editor "+ VERSION + " - " + fileName;
                else Title = "Questions Editor " + VERSION;
            }
        }
        private string fileName = string.Empty;

        private Brush originalBrush = null;
        private SolidColorBrush selectedBrush = new SolidColorBrush(Color.FromRgb(37, 144, 104));
        private int currentIndex = 0;
        private BitmapImage currentImage = null; 
        private List<Common.QuestionAndAnswer> questionsAndAnswers = null;

        public MainWindow()
        {
            InitializeComponent();
            SetControlsEnable(false);
            this.Icon = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.editor);
            loadNoImage();
            originalBrush = nextButton.Background;
            FileName = string.Empty;
            this.Closing += CloseProgram;
        }

        private void SetControlsEnable(bool value)
        {
            previousButton.IsEnabled = value;
            nextButton.IsEnabled = value;
            questionTextBox.IsEnabled = value;
            imageDockPanel.IsEnabled = value;
            answersDockPanel.IsEnabled = value;
            editMenuItem.IsEnabled = value;
            saveButton.IsEnabled = value;
            saveAsButton.IsEnabled = value;
        }

        void loadNewImage(BitmapImage bitmap)
        {
            currentImage = bitmap;
            image.Source = currentImage;
        }

        void loadNoImage()
        {
            currentImage = null;
            image.Source = Common.QuestionsAndAnswers.ToBitmapImage(Properties.Resources.noImage);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CloseProgram(null, null);
        }

        private void CloseProgram(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (questionsAndAnswers != null && questionsAndAnswers.Count > 0)
            {
                var result = MessageBox.Show("All unsaved data will be lost!\nWould you like continue?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {
                    if(sender != null) e.Cancel = true;
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void SaveAsClickSaveClick(object sender, RoutedEventArgs e)
        {

        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveCurrentQuestion();
            try
            {
                SaveDataToFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveDataToFile(bool openSaveDialog = false)
        {
            if (questionsAndAnswers == null || questionsAndAnswers.Count == 0)
            {
                MessageBox.Show("No Items!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool? result = null;
            if (FileName == string.Empty || openSaveDialog)
            {
                SaveFileDialog win = new SaveFileDialog();
                win.Filter = "XML file (*.xml)|*.xml";
                result = win.ShowDialog();
                if (result == null) return;
                if ((bool)result) FileName = win.FileName;
            }
            else
            {
                result = true;
            }

            if ((bool)result)
            {
                Common.QuestionsAndAnswers.SaveToXML(FileName, questionsAndAnswers, Common.QuestionsAndAnswers.ImageType.Base64);
                MessageBox.Show("Saved!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CreateNewClick(object sender, RoutedEventArgs e)
        {
            if (FileName != string.Empty)
            {
                var result = MessageBox.Show("All unsaved data will be lost!\nWould you like continue?", "Warrning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    StartNewTask();
                }
            }
            else
            {
                StartNewTask();
            }
        }

        private void StartNewTask()
        {
            FileName = string.Empty;
            currentIndex = 0;
            currentImage = null;
            questionsAndAnswers = new List<Common.QuestionAndAnswer>();
            SetControlsEnable(true);
            UpdateIndexLabel();
            CheckNextButton();
            ResetControls();
            previousButton.IsEnabled = false;
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    List<string> supportedExtension = new List<string>() { ".png", ".jpg" };

                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    string extension = System.IO.Path.GetExtension(files[0]);
                    if (!supportedExtension.Contains(extension))
                    {
                        MessageBox.Show("Extension not supported!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    loadNewImage(new BitmapImage(new Uri(files[0])));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ResetControls()
        {
            questionTextBox.Text = string.Empty;
            answersDockPanel.Reset();
            loadNoImage();
        }

        private Common.QuestionAndAnswer GenerateObject()
        {
            string question = questionTextBox.Text;
            if (question == string.Empty)
            {
                MessageBox.Show("No question!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            List<Common.Answer> list = answersDockPanel.GetAnswers();
            if (list == null)
            {
                return null;
            }

            if (currentImage != null)
            {
                return new Common.QuestionAndAnswer(question, list, currentImage);
            }
            else
            {
                return new Common.QuestionAndAnswer(question, list);
            }
        }

        private void UpdateIndexLabel()
        {
            if (currentIndex == questionsAndAnswers.Count)
            {
                indexLabel.Content = "New Question (" + (questionsAndAnswers.Count + 1).ToString() + ")";
            }
            else
            {
                indexLabel.Content = (currentIndex + 1).ToString() + " / " + (questionsAndAnswers.Count).ToString();
            }
        }

        private void LoadQuestion()
        {
            var item = questionsAndAnswers.ElementAt(currentIndex);
            if (currentIndex == 0)
            {
                previousButton.IsEnabled = false;
            }
            questionTextBox.Text = item.Question;
            answersDockPanel.SetAnswers(item.Answers);
            if (item.Picture != null)
            {
                loadNewImage(item.Picture);
            }
            else
            {
                loadNoImage();
            }
        }

        private bool SaveCurrentQuestion()
        {
            var item = GenerateObject();
            if (item == null)
            {
                return false;
            }
            if (currentIndex + 1 > questionsAndAnswers.Count)
            {
                questionsAndAnswers.Add(item);
            }
            else
            {
                questionsAndAnswers[currentIndex] = item;
            }
            previousButton.IsEnabled = true;
            return true;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveCurrentQuestion())
            {
                currentIndex++;
                CreateOrLoadQuestion();
            }
            UpdateIndexLabel();
        }

        private void CreateOrLoadQuestion()
        {
            if (currentIndex < questionsAndAnswers.Count)
            {
                LoadQuestion();
            }
            else
            {
                ResetControls();
            }
            CheckNextButton();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (questionTextBox.Text == string.Empty)
            {
                //No Save
                currentIndex--;
                LoadQuestion();
            }
            else
            {
                if (SaveCurrentQuestion())
                {
                    currentIndex--;
                    LoadQuestion();
                }
            }
            CheckNextButton();
            UpdateIndexLabel();
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog win = new OpenFileDialog();
            win.Filter = "XML file (*.xml)|*.xml";
            bool? result = win.ShowDialog();
            if (result == null) return;
            if ((bool)result)
            {
                try
                {
                    mainGrid.IsEnabled = false;
                    StartNewTask();
                    questionsAndAnswers = Common.QuestionsAndAnswers.LoadFromXml(win.FileName);
                    LoadQuestion();
                    CheckNextButton();
                    UpdateIndexLabel();
                    FileName = win.FileName;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                mainGrid.IsEnabled = true;
            }
        }

        private void CheckNextButton()
        {
            nextButton.IsEnabled = true;
            if (currentIndex == questionsAndAnswers.Count - 1)
            {
                nextButton.Content = "New";
                nextButton.Background = selectedBrush;
            }
            else if (currentIndex == 0 && questionsAndAnswers.Count == 0)
            {
                nextButton.Content = "New";
                nextButton.IsEnabled = false;
                nextButton.Background = selectedBrush;
            }
            else if (currentIndex == questionsAndAnswers.Count && questionsAndAnswers.Count != 0)
            {
                nextButton.Content = "New";
                nextButton.IsEnabled = false;
                nextButton.Background = selectedBrush;
            }
            else
            {
                nextButton.Content = "Next";
                nextButton.Background = originalBrush;
            }
        }

        private void QuestionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentIndex == questionsAndAnswers.Count)
            {
                if (questionTextBox.Text != string.Empty)
                {
                    nextButton.IsEnabled = true;
                }
                else
                {
                    nextButton.IsEnabled = false;
                }
            }
        }

        private void SaveAsClick(object sender, RoutedEventArgs e)
        {
            SaveDataToFile(true);
        }

        private void CreateNewQuestionClick(object sender, RoutedEventArgs e)
        {
            if (SaveCurrentQuestion())
            {
                currentIndex = questionsAndAnswers.Count;
                CreateOrLoadQuestion();
                UpdateIndexLabel();
            }
        }

        private void DeleteQuestion()
        {
            if (questionsAndAnswers.Count == 0 || currentIndex == questionsAndAnswers.Count)
            {
                MessageBox.Show("There are no questions", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = MessageBox.Show("Are you sure to delete this question?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    questionsAndAnswers.RemoveAt(currentIndex);
                    if (currentIndex > 0)
                    {
                        currentIndex -= 1;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                    CreateOrLoadQuestion();
                    UpdateIndexLabel();
                    CheckNextButton();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteQuestionClick(object sender, RoutedEventArgs e)
        {
            DeleteQuestion();
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AboutBoxWindow().ShowDialog();
        }
    }
}
    