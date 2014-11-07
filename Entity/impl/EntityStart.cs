using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class EntityStart : Entity
    {
        int projectsCount { get; set; }

        public override void execute() {
            List<Entity> outputs = getOutputs();
            if (outputs != null && outputs.Count != 0)
            {
                outputs[0].setQueue(getQueue()); 
            }
        }
    }
}
