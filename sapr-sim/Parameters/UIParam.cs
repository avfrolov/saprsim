using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace sapr_sim.Parameters
{

    // Are C# generics better than Java generics? hmm...
    // http://stackoverflow.com/questions/353126/c-sharp-multiple-generic-types-in-one-list    
    public abstract class UIParam 
    {
        protected ParamValidator validator;
        protected string displayedText;

        // default - textbox
        protected UIElement control;

        public ParamValidator Validator
        {
            get { return validator; }
        }

        public string DisplayedText
        {
            get { return displayedText; }
        }

        public UIElement ContentControl
        {
            get { return control; }
        }

        public abstract IConvertible RawValue { get; set; }
    }

    // for T we should use next types:
    //      Boolean
    //      Byte
    //      Char
    //      DateTime
    //      Decimal
    //      Double
    //      Int (16, 32 and 64-bit)
    //      SByte
    //      Single (float)
    //      String
    //      UInt (16, 32 and 64-bit)
    // http://stackoverflow.com/questions/8745444/c-sharp-generic-constraints-to-include-value-types-and-strings
    public class UIParam<T> : UIParam where T : IConvertible
    {
        
        private T value;

        public UIParam(T value, ParamValidator validator, string displayedText)
        {
            this.value = value;
            this.validator = validator;
            this.displayedText = displayedText;
        }

        public UIParam(T value, ParamValidator validator, string displayedText, UIElement cc) : this(value, validator, displayedText)
        {
            this.control = cc;
        }

        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        // FU C#!!!
        public override IConvertible RawValue
        {
            get { return value; }
            set
            {
                Type valueType = this.value.GetType();
                if (valueType == typeof(string))                    
                    this.value = (T) (object) Convert.ToString(value);
                if (valueType == typeof(Double))
                    this.value = (T) (object)Convert.ToDouble(value);
                if (valueType == typeof(Boolean))
                    this.value = (T)(object)Convert.ToBoolean(value);
                if (valueType == typeof(Int32))
                    this.value = (T)(object)Convert.ToInt32(value);
            }
        }

    }
}
