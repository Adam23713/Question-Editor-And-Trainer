using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examiner.BaseClasses
{
    public class TaskResult
    {
        public TimeSpan ElapsedTime { get; set; }
        public int GoodAnswers { get; set; }
        public int BadAnswers { get; set; }
    }
}
