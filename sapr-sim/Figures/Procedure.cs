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
    public class Procedure : UIEntity
    {
        // not used in current version...
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(Procedure));

        private Rect bound;
        private FormattedText label;

        private Port inputPort, outputPort, resourcePort;

        private const int innerLabelOffsetX = 18;
        private const int innerLabelOffsetY = 22;

        public Procedure(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.LemonChiffon;

            bound = new Rect(new Size(90, 60));
            label = new FormattedText("Процедура",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        public override void createAndDrawPorts(double x, double y)
        {
            inputPort = new Port(this, canvas, x-4, y+26.5);
            outputPort = new Port(this, canvas, x+86, y+26.5);
            resourcePort = new Port(this, canvas, x + 42.5, y + 55.5);
            canvas.Children.Add(inputPort);
            canvas.Children.Add(outputPort);
            canvas.Children.Add(resourcePort);
            ports.Add(inputPort);
            ports.Add(outputPort);
            ports.Add(resourcePort);
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                RectangleGeometry rg = new RectangleGeometry(bound, 10, 10);
                Geometry geometry = label.BuildGeometry(new Point(innerLabelOffsetX, innerLabelOffsetY));

                gg.Children.Add(rg);
                gg.Children.Add(geometry);

                return gg;
            }
        }

    }
}
