using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sapr_sim.Parameters;

namespace sapr_sim.Utils
{
    public class TimeWithMeasure 
    {
        public double doubleValue { get; set;}
        public TimeMeasure measure { get; set;}

        public TimeWithMeasure(double doubleValue, TimeMeasure measure)
        {
            this.doubleValue = doubleValue;
            this.measure = measure;
        }
    }

    public class TimeConverter
    {
        public static double fromHumanToModel(TimeParam time)
        {
            double result = 0;
            switch (time.Measure.Order)
            {
                case 0:
                    result = time.Time;
                    break;
                case 1:
                    result = time.Time * 10;
                    break;
                case 2:
                    result = time.Time * 100;
                    break;
                case 3:
                    result = time.Time * 1000;
                    break;
                default:
                    throw new Exception("Wrong time measure");
            }
            return result;
        }

        public static double fromHumanToModel(TimeWithMeasure time)
        {
            double result = 0;
            switch (time.measure.Order)
            {
                case 0:
                    result = time.doubleValue;
                    break;
                case 1:
                    result = time.doubleValue * 10;
                    break;
                case 2:
                    result = time.doubleValue * 100;
                    break;
                case 3:
                    result = time.doubleValue * 1000;
                    break;
                default:
                    throw new Exception("Wrong time measure");
            }
            return result;
        }

        public static TimeWithMeasure fromModelToHuman(double modelTime)
        {
            // "modelTime / 1000" in return statement is for beautifull output. Further the divider will decrease   
            if ((modelTime / 1000) >= 1)
                return new TimeWithMeasure(modelTime / 1000, TimeMeasure.DAY); 

            if ((modelTime / 1000) >= 0.1)
                return new TimeWithMeasure(modelTime / 100, TimeMeasure.HOUR);

            if ((modelTime / 1000) >= 0.01)
                return new TimeWithMeasure(modelTime / 10, TimeMeasure.MINUTE);

            return new TimeWithMeasure(modelTime , TimeMeasure.SECOND);
        }
    }
}
