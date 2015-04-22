using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InstrumentResource : Resource
    {

        public double power { get; set; }
        public double price { get; set; }

        public List<MaterialResource> materials { get; set; }

        public InstrumentResource()
        {
            materials = new List<MaterialResource>();
        }

        public override double totalCount
        {
            get 
            {
                double total = count;
                foreach (MaterialResource res in materials)
                    total += res.totalCount;
                return total;
            }
        }
    }
}
