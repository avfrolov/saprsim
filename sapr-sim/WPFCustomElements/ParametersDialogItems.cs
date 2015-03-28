using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace sapr_sim.WPFCustomElements
{

    public interface ParameterInput : ICloneable
    {
        void setValue(IConvertible c);
        IConvertible getValue();
    }

    public class ParameterTextBox : TextBox, ParameterInput
    {

        public ParameterTextBox(IConvertible value, bool enabled)
        {
            Text = value.ToString();
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            MaxWidth = 100;
            MinWidth = 100;
            IsEnabled = enabled;
        }

        public void setValue(IConvertible c)
        {
            Text = c.ToString();
        }

        public IConvertible getValue()
        {
            return Text;
        }

        public object Clone()
        {
            return new ParameterTextBox(Text, IsEnabled);
        }

    }

    public class ParameterCheckBox : CheckBox, ParameterInput
    {

        public ParameterCheckBox()
        {
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
            MaxWidth = 100;
            MinWidth = 100;
        }

        public ParameterCheckBox(IConvertible value) : this()
        {
            IsChecked = Convert.ToBoolean(value);
        }

        public ParameterCheckBox(IConvertible value, bool enabled) : this(value)
        {
            IsEnabled = enabled;
        }

        public void setValue(IConvertible c)
        {
            IsChecked = Convert.ToBoolean(c);
        }

        public IConvertible getValue()
        {
            return IsChecked;
        }

        public object Clone()
        {
            return new ParameterCheckBox(IsChecked, IsEnabled);
        }
    }

    
}
