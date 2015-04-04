using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Utils
{
    [Serializable]
    public sealed class TimeMeasure
    {

        public static readonly TimeMeasure SECOND = new TimeMeasure(0, "сек.");
        public static readonly TimeMeasure MINUTE = new TimeMeasure(1, "мин.");
        public static readonly TimeMeasure HOUR = new TimeMeasure(2, "час.");
        public static readonly TimeMeasure DAY = new TimeMeasure(3, "дн.");

        public int Order { get; set; }
        public string Name {get; set; }

        public static List<TimeMeasure> list()
        {
            return new List<TimeMeasure>() { SECOND, MINUTE, HOUR, DAY };
        }

        public static TimeMeasure byOrder(int order)
        {
            switch(order)
            {
                case 0:
                    return SECOND;
                case 1:
                    return MINUTE;
                case 2:
                    return HOUR;
                case 3:
                    return DAY;
                default:
                    throw new Exception("WTF, dude!?");
            }
        }

        public TimeMeasure()
        {
            // for serialization only
        }

        private TimeMeasure(int order, string name)
        {
            this.Order = order;
            this.Name = name;
        }

        public override String ToString()
        {
            return Name;
        }        

    }
}
