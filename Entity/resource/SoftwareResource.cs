using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.resource
{
    class SoftwareResource : Resource
    {
        public double price { get; set; }
        public int avaliableInstalletionCount { get; set; }
        public double licenceActiveTime { get; set; }
        public double errorProbability { get; set; }

        public override double totalCount
        {
            get { return count; }
        }
    }
}
