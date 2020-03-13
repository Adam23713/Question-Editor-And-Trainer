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
    /// Interaction logic for RaceModePanel.xaml
    /// </summary>
    public partial class RaceModePanel : UserControl
    {
        public string FilesPath = string.Empty;

        public RaceModePanel()
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
           // ShowQuestionProgressBar(oneQuestionTimeLimitTimeSetter.IsEnabled);
        }

        private void createNewCompetitorButton_Click(object sender, RoutedEventArgs e)
        {
            CompetitorInfoWindow win = new CompetitorInfoWindow(FilesPath);
            win.ShowDialog();
            if (win.filesPath == string.Empty)
            {
                
            }
        }
    }
}
