using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Examiner.BaseClasses
{
    class TrainingMode : TaskMode
    {
        private TimeSpan elapsedTime;

        protected override void Start()
        {
            bool isTimerSetted = SetTimers();
            if (!isTimerSetted)
            {
                taskTimer = new DispatcherTimer();
                taskTimer.Interval = new TimeSpan(0, 0, 1);
                taskTimer.Tick += QuestionTimer_Tick;
                elapsedTime = new TimeSpan(0, 0, 0);
                taskTimer.Start();
            }
            else 
            {
            }
        }

        protected override void Stop()
        {
            
        }

        protected override void QuestionTimer_Tick(object sender, EventArgs e)
        {
            elapsedTime += taskTimer.Interval;
            SetTimeLabel(elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
        }

        protected override void TaskTimer_Tick(object sender, EventArgs e)
        {
        }
    }
}
