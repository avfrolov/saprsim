using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class EntityDestenation : Entity
    {

        public override void execute() {
            List<Project> projects = getQueue();
            foreach (Project prj in projects)
            {
                prj.state = State.DONE;
            }
        }
    }
}
