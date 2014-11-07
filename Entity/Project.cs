using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public enum state
    {
        IN_PROGRESS,
        DONE
    };

    class Project
    {
        state state { get; set; }
    }
}
