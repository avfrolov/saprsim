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

        public Project()
        {
            state = State.IN_PROGRESS;
        }
        public State state { get; set; }
    }
}
