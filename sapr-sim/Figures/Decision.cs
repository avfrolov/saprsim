using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace sapr_sim.Figures
{
    public class Decision : UIEntity
    {
        private Rect bound;
        private FormattedText label;

        private Port inputPort, yesPort, noPort;

        private const int innerLabelOffsetX = 20;
        private const int innerLabelOffsetY = 16;

        public Decision(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.LemonChiffon;

            bound = new Rect(new Size(45, 45));
            label = new FormattedText("?",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
            inputPort = new Port(this, canvas, x - 12, y + 19);
            yesPort = new Port(this, canvas, x + 19, y - 12.5);
            noPort = new Port(this, canvas, x + 19, y + 47.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(yesPort);
            canvas.Children.Add(noPort);
            ports.Add(inputPort);
            ports.Add(yesPort);
            ports.Add(noPort);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.Nonzero;

                RectangleGeometry rg = new RectangleGeometry(bound);
                rg.Transform = new RotateTransform(45, bound.Width / 2, bound.Height / 2);

                gg.Children.Add(rg);
                gg.Children.Add(label.BuildGeometry(new Point(innerLabelOffsetX, innerLabelOffsetY)));
                return gg;
            }
        }
    }
}
