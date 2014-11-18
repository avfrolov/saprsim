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
    public class Source : UIEntity
    {
        private Rect bound;
        protected FormattedText label;

        private const int innerLabelOffsetX = 14;
        private const int innerLabelOffsetY = 23;

        public Source(Canvas canvas) : base(canvas)
        {
            Fill = Brushes.Red;

            bound = new Rect(new Size(60, 60));
            label = new FormattedText("Начало",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                EllipseGeometry eg = new EllipseGeometry(bound);
                Geometry geometry = label.BuildGeometry(new Point(innerLabelOffsetX, innerLabelOffsetY));

                gg.Children.Add(eg);
                gg.Children.Add(geometry);

                return gg;
            }
        }
    }
}
