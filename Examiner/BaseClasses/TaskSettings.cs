using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examiner.BaseClasses
{
    public class TaskSettings
    {
        public TimeSpan TaskTimeLimit;
        public TimeSpan QuestionTimeLimit;
        public bool isTaskLimitActive;
        public bool isQuestionLimitActive;
    }
}
