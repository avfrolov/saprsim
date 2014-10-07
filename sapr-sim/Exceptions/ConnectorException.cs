using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Exceptions
{
    public class ConnectorException : Exception
    {
        public ConnectorException(string message) : base(message) { }
    }
}
