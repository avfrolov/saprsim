using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Timer
    {
        public double DELTA = 0.01; // SOME CONST

        double timer = 0.0;

        private static Timer instance = new Timer();

        static Timer() { }
        private Timer() { }

        public static Timer Instance
        {
            get
            {
                return instance;
            }
        }

        public void increment()
        {
            timer += DELTA;
        }


        public double getTime()
        {
            return timer;
        }
    }
}
