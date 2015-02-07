using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class EntityDestination : Entity
    {

        public override void execute() {
            List<Project> projects = getReadyProjectQueue();
            List<Project> projects1 = getNotReadyProjectQueue();
            
            foreach (Project prj in projects)
            {
                prj.state = State.DONE;
            }

            foreach (Project prj in projects1)
            {
                prj.state = State.DONE;
            }
        }
    }
}
