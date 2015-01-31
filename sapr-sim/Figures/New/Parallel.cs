using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures.New
{
    public class Parallel : UIEntity
    {
        private Rect bound;
        private FormattedText label;

        private Port inputPort, outputPort1, outputPort2;

        private const int innerLabelOffsetX = -45;
        private const int innerLabelOffsetY = -20;

        public Parallel(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Black;
            StrokeThickness = .5;

            bound = new Rect(new Size(10, 90));
            label = new FormattedText("Распараллеливание",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
            inputPort = new Port(this, canvas, x - 4, y + 45);
            outputPort1 = new Port(this, canvas, x + 7, y + 22.5);
            outputPort2 = new Port(this, canvas, x + 7, y + 67.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort1);
            canvas.Children.Add(outputPort2);
            ports.Add(inputPort);
            ports.Add(outputPort1);
            ports.Add(outputPort2);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;

                gg.Children.Add(new RectangleGeometry(bound));
                gg.Children.Add(label.BuildGeometry(new Point(innerLabelOffsetX, innerLabelOffsetY)));

                return gg;
            }
        }
    }
}
