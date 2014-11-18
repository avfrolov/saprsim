using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Synchronization : Entity
    {
        public override void execute()
        {
            // getting duplicate values 
            List<Project> projects = getReadyProjectQueue().GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            if (projects != null && projects.Count != 00)
            {
                Project proj = projects[0];
                Entity outputEntity = getOutputs()[0];
                outputEntity.addProjectToQueue(proj);

                getReadyProjectQueue().Remove(proj);
            }
        }
    }
}
