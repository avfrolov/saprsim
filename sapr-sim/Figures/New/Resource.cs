using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace sapr_sim.Figures.New
{
    public class Resource : UIEntity
    {

        private Rect bound;
        private Rect topExternalBound;
        private Rect bottomExternalBound;
        private FormattedText label;

        // TODO move to struct?
        private const int innerLabelOffsetX = 28;
        private const int innerLabelOffsetY = 22;

        public Resource()
        {
            Fill = Brushes.LemonChiffon;

            bound = new Rect(new Size(90, 60));
            topExternalBound = new Rect(new Point(0, 0), new Point(90, 10));
            bottomExternalBound = new Rect(new Point(0, 50), new Point(90, 60));
            label = new FormattedText("Ресурс",
                CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                new Typeface("Times New Roman"), 12, Brushes.Black);
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
