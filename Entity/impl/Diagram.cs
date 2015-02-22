using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Diagram : Entity
    {

        public override void execute() { }

        public override bool canUseAsInput(Entity entity)
        {
            return true;
        }

        public override bool canUseAsOutput(Entity entity)
        {
            return true;
        }

        public override bool correctInputCount()
        {
            return true;
        }

        public override bool correctOutputCount()
        {
            return true;
        }
    }
}
