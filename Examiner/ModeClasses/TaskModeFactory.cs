using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examiner.ModeClasses
{
    class TaskModeFactory
    {
        public enum Mode { TrainingMode, RaceMode};

        public static TaskMode CreateMode(Mode mode)
        {
            if (mode == Mode.TrainingMode)
            {
                return new TrainingMode();
            }
            if (mode == Mode.TrainingMode)
            {
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
