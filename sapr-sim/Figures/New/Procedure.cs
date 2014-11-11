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

namespace sapr_sim.Figures.New
{
    public class Procedure : UIEntity
    {
        // not used in current version...
        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Double), typeof(Procedure));

        private Rect bound;
        private FormattedText label;

        private const int innerLabelOffset = 20;

        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }

        protected override System.Windows.Media.Geometry DefiningGeometry
        {
            get 
            {
                StrokeThickness = 2;
                Stroke = Brushes.Black;
                Fill = Brushes.LemonChiffon;

                bound = new Rect(new Size(90, 60));
                label = new FormattedText("Процедура",
                    CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    new Typeface("Times New Roman"), 12, Brushes.Black);

                GeometryGroup gg = new GeometryGroup();
                gg.FillRule = FillRule.EvenOdd;
                RectangleGeometry rg = new RectangleGeometry(bound, 10, 10);
                Geometry geometry = label.BuildGeometry(new Point(innerLabelOffset, innerLabelOffset));

                gg.Children.Add(rg);
                gg.Children.Add(geometry);

                return gg;

                //Point p1 = new Point(500.0d, 500.0d);
                //Point p2 = new Point(this.Size, 500.0d);
                //Point p3 = new Point(this.Size / 2, this.Size);

                //this.StrokeThickness = 4;
                //this.Stroke = Brushes.Red;
                //Fill = Brushes.Black;

                //List<PathSegment> segments = new List<PathSegment>(3);
                //segments.Add(new LineSegment(p1, true));
                //segments.Add(new LineSegment(p2, true));
                //segments.Add(new LineSegment(p3, true));

                //List<PathFigure> figures = new List<PathFigure>(1);
                //PathFigure pf = new PathFigure(p1, segments, true);
                //figures.Add(pf);

                //return new PathGeometry(figures, FillRule.EvenOdd, null);
            }
        }

    }
}
