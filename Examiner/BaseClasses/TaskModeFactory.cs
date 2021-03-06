﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examiner.BaseClasses
{
    class TaskModeFactory
    {
        public enum Mode { TrainingMode, RaceMode, Unknow };

        public static TaskMode CreateMode(Mode mode)
        {
            if (mode == Mode.TrainingMode)
            {
                return new TrainingMode();
            }
            if (mode == Mode.RaceMode)
            {
                return new RaceMode();
            }
            else
            {
                return null;
            }
        }
    }
}
