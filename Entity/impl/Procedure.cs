using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.impl
{
    public class Procedure : Entity
    {
        double manHour { get; set; }
        List<Resource> resources = new List<Resource>();
        
        public override void execute() { }
    }
}
