﻿using System;
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
    /// Interaction logic for AnswerButton.xaml
    /// </summary>
    public partial class AnswerButton : UserControl
    {
        public bool IsRight { get; private set; }
        public string Text 
        {
            get { return text; }
            private set { text = value; textBox.Text = text; this.Height = button.Height; }
        }
        private string text;

        public AnswerButton(string answer, bool right)
        {
            InitializeComponent();
            Text = answer;
            IsRight = right;
        }
    }
}