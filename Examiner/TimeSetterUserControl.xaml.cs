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
    /// Interaction logic for TimeSetterUserControl.xaml
    /// </summary>
    public partial class TimeSetterUserControl : UserControl
    {
        public TimeSpan GetTime
        {
            get { return GenerateTimeSpan(); }
            private set { }
        }

        public TimeSetterUserControl()
        {
            InitializeComponent();
        }

        private TimeSpan GenerateTimeSpan()
        {
            int sec = ConvertStringToInt(secondTextBox.Text);
            int min = ConvertStringToInt(minTextBox.Text);
            int hour = ConvertStringToInt(hourTextBox.Text);
            SetTime(hour.ToString(), min.ToString(), sec.ToString()); //Set right values if there are errors

            return new TimeSpan(hour, min, sec);
        }

        private void SetTime(string hour, string min, string sec)
        {
            secondTextBox.Text = sec;
            minTextBox.Text = min;
            hourTextBox.Text = hour;
        }

        private int ConvertStringToInt(string text)
        {
            try
            {
                int value = Int32.Parse(text);
                if (value < 0)
                {
                    MessageBox.Show(text + " : The number cannot be less than 0", "Invalid value", MessageBoxButton.OK, MessageBoxImage.Error);
                    return 0;
                }
                return value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(text + " : Invalid format", "Invalid value", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;

            }
        }
    }
}
