using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    // TODO make abstract! remove efficiency and price
    public class Resource
    {

        public ResourceType type { get; set; }

        public double efficiency { get; set; }
        public double price { get; set; }
        public int count { get; set; }
        public bool isShared { get; set; }
        public bool isBusy { get; set; }
        public HashSet<long> users = new HashSet<long>();
    }
}
