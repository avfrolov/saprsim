using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sapr_sim.Figures.Basic.Enums;

namespace sapr_sim.Figures.Basic.Lines
{
    public class LedgePoint : ICloneable
    {
        public float coodinate;
        public LedgeDirection dir;
        public object Clone()
        {
            LedgePoint lp = new LedgePoint();
            lp.coodinate = coodinate;
            lp.dir = dir;
            return lp as Object;
        }
    }
}
