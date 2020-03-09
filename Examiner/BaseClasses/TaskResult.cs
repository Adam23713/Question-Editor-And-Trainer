using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Examiner.BaseClasses
{
    public class AnswersResult
    {
        public Common.QuestionAndAnswer Question { get; set; }

        public int SelectedAnswerIndex { get; set; } = -1;
    }

    public class TaskResult
    {
        public TimeSpan ElapsedTime { get; set; }
        public List<AnswersResult> AnswersResults { get; set; }
        public int GoodAnswers { get; set; }
        public int BadAnswers { get; set; }
    }
}
