using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using sapr_sim.Parameters;

namespace sapr_sim.Figures
{
    public class Destination : Source
    {

        public Destination(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.LightGreen;

            label = new FormattedText("Конец",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
            port = new Port(this, canvas, x - 4, y + 26.5);
            canvas.Children.Add(port);
            ports.Add(port);
        }

        public override List<UIParam> getParams()
        {
            List<UIParam> param = new List<UIParam>();
            return param;
        }

    }
}
