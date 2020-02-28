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

namespace Examiner.Panels
{
    /// <summary>
    /// Interaction logic for TrainingModePanel.xaml
    /// </summary>
    public partial class TrainingModePanel : UserControl
    {
        public delegate void CheckBoxCheckedChanged(bool value);
        public CheckBoxCheckedChanged SetNextButton;

        public TrainingModePanel()
        {
            InitializeComponent();
        }

        private void TaskTimeCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            taskTimeLimitTimeSetter.IsEnabled = (bool)taskTimeLimitCheckBox.IsChecked;
        }

        private void OneQuestionTimeCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            oneQuestionTimeLimitTimeSetter.IsEnabled = (bool)oneQuestionTimeLimitCheckBox.IsChecked;
        }

        private void AutoShiftingCheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            SetNextButton?.Invoke(!(bool)autoShiftingCheckBox.IsChecked);
        }
    }
}
