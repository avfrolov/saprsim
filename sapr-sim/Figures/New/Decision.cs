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
    public class Decision : UIEntity
    {
        private Rect bound;
        private FormattedText label;

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
