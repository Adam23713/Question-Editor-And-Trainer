using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examiner.BaseClasses
{
    class RaceMode : TaskMode
    {
        Dictionary<string, List<AnswersResult>> answersResults = null;

        public RaceMode()
        {
            ModeName = "Race";
        }

        public override void AnswerArrived(string text, bool isRight, int selectedIndex)
        {
            return;
        }
    }
}
