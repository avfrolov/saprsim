using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum State
    {
        IN_PROGRESS,
        DONE
    };

    public class Project
    {
        public State state { get; set; }
    }
}
