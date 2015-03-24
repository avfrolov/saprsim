using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Resource
    {
        public double efficiency { get; set; }
        public double price { get; set; }
        public int count { get; set; }
        public bool isShared { get; set; }
        public bool isBuisy { get; set; }
        public HashSet<long> users = new HashSet<long>();
    }
}
