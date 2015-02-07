using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim.Parameters
{
    public interface ParamValidator
    {
        bool validate(string value);
    }

    public class StringParamValidator : ParamValidator
    {
        public bool validate(string value)
        {
            return !String.IsNullOrEmpty(value);
        }
    }

    public class IntegerParamValidator : ParamValidator
    {
        public bool validate(string value)
        {
            int result;
            return Int32.TryParse(value, out result);
        }
    }

    public class DoubleParamValidator : ParamValidator
    {
        public bool validate(string value)
        {
            double result;
            return Double.TryParse(value, out result);
        }
    }

}
