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
    public class Sync : UIEntity
    {
        private Rect bound;
        private FormattedText label;

        private Port inputPort1, inputPort2, outputPort;

        private const int innerLabelOffsetX = -30;
        private const int innerLabelOffsetY = -20;

        public Sync(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Black;
            StrokeThickness = .5;

            bound = new Rect(new Size(10, 90));
            label = new FormattedText("Синхронизация",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public override void createAndDrawPorts(double x, double y)
        {
            inputPort1 = new Port(this, canvas, x - 4, y + 22.5);
            inputPort2 = new Port(this, canvas, x - 4, y + 67.5);
            outputPort = new Port(this, canvas, x + 7, y + 45);
            canvas.Children.Add(inputPort1);
            canvas.Children.Add(inputPort2);
            canvas.Children.Add(outputPort);
            ports.Add(inputPort1); 
            ports.Add(inputPort2);
            ports.Add(outputPort);
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
