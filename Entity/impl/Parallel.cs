using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Parallel : Entity
    {

        public override void execute() {
            Project project = getProjectFromQueue();

            if (project != null)
            {
                Project prj = getProjectFromQueue();
                Entity outputEntity1 = getOutputs()[0];
                outputEntity1.addProjectToQueue(prj);
                Entity outputEntity2 = getOutputs()[1];
                outputEntity2.addProjectToQueue(prj);
            }
        }
    }
}
