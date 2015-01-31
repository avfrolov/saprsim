using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace sapr_sim.Figures
{
    public class Resource : UIEntity
    {

        private Rect bound;
        private Rect topExternalBound;
        private Rect bottomExternalBound;
        private FormattedText label;

        private Port port;

        private const int innerLabelOffsetX = 28;
        private const int innerLabelOffsetY = 22;

        public Resource(Canvas canvas) : base(canvas)
        {
            bound = new Rect(new Size(90, 60));
            topExternalBound = new Rect(new Point(0, 0), new Point(90, 10));
            bottomExternalBound = new Rect(new Point(0, 50), new Point(90, 60));
            label = new FormattedText("Ресурс",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
            port = new Port(this, canvas, x + 42.5, y - 3.5);
            canvas.Children.Add(port);
            ports.Add(port);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;

                gg.Children.Add(new RectangleGeometry(bound));
                gg.Children.Add(label.BuildGeometry(new Point(innerLabelOffsetX, innerLabelOffsetY)));
                gg.Children.Add(new RectangleGeometry(topExternalBound));
                gg.Children.Add(new RectangleGeometry(bottomExternalBound));

                return gg;
            }
        }
    }
}
