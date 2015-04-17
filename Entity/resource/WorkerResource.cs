using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WorkerResource : Resource
    {

        public double efficiency { get; set; }
        public double price { get; set; }

        public List<InstrumentResource> instruments { get; set; }
        public List<MaterialResource> materials { get; set; }
    }
}
