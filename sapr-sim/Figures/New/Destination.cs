using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace sapr_sim.Figures.New
{
    public class Destination : Source
    {
        public Destination()
        {
            Fill = Brushes.LightGreen;

            label = new FormattedText("Конец",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }
    }
}
